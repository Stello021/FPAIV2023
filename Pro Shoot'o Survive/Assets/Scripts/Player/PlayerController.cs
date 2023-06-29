using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Player variables")]
    [SerializeField] private float playerMoveSpeed = 6f; // Speed at which the player moves.
    [SerializeField] private float playerRotationSpeed = 6f; // Speed at which the player moves.
    [SerializeField] private float jumpForce = 5f; // Force applied when the player jumps.
    private CharacterController charController; // Reference to the CharacterController component for player movement.
    private Animator animator; // Reference to the Animator component for controlling animations.
    private float gravityVelocity; // Player's current velocity.
    private bool isJumping; // Flag indicating if the player is currently jumping.

    public bool IsAiming
    {
        get
        {
            return isAiming;
        }

        private set
        {
            isAiming = value;
            crossHairTransform.gameObject.SetActive(value);
            animator.SetBool("IsAiming", value);
        }
    }

    private bool isAiming;

    [Header("\nWeapon reference variables")]
    [SerializeField] private GameObject defaultWeapon;
    [SerializeField] private GameObject assaultWeapon;
    private GameObject currentWeapon;

    [Header("\nCrossHair variables")]
    [SerializeField] private RectTransform crossHairTransform;

    [Header("\nBullet reference variables")]
    [SerializeField] private GameObject Bullet; // Reference to Bullet prefab.
    [SerializeField] private Transform BulletSpawn; // Reference to BulletSpown transform.
    [SerializeField] private Transform CentreCameraTarget; // Reference to the transform of an EmptyObject located in the center of the camera frame. 

    [Header("\nGrenade variables")]
    [SerializeField] GameObject grenadePrefab;
    public int Grenades { get { return grenades; } set { grenades = value; UpdateGrenadeText(); } }
    private int grenades;
    [SerializeField] Transform grenadeSpawnPoint;
    [SerializeField] TMP_Text grenadeNumberText;



    [Header("\nHoming variables")]
    [SerializeField] float homingTimer;
    [SerializeField] float homingTime;
    [SerializeField] float viewRadius;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float angleOfVision;
    [SerializeField] SkinnedMeshRenderer smr;
    [SerializeField] Material normalMaterial;
    [SerializeField] Material homingMaterial;
    private bool activeHoming;

    [Header("Camera reference variables")]
    [HideInInspector] public Transform cam;

    //Input system controller
    public InputSysController InputsController { get; private set; }

    [SerializeField] LayerMask aimMask;
    [SerializeField] GameObject pausePanel;
    private bool isInPause;
    [SerializeField] List<AudioClip> shootClips;

    private void Start()
    {
        InputsController = new InputSysController();

        animator = GetComponent<Animator>(); // Get the Animator component attached to the same GameObject.
        charController = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        IsAiming = false;

        currentWeapon = defaultWeapon;
        BulletSpawn = currentWeapon.transform;
    }

    private void Update()
    {
        TogglePauseMenu();

        ThrowGrenade();
        Aim();
        Shoot();

        updateStats();

        //Jump();
        ApplyGravity();
        CheckIsOnGround();
    }

    private void LateUpdate()
    {
        MovePlayer();
    }

    private void TogglePauseMenu()
    {
        if (!InputsController.OnInputTrigger("PauseMenu"))
        {
            return;
        }

        isInPause = !isInPause;

        Time.timeScale = Convert.ToInt32(!isInPause);
        pausePanel.SetActive(isInPause);
        Cursor.visible = isInPause;
        Cursor.lockState = isInPause? CursorLockMode.Confined: CursorLockMode.Locked;
    }

    public void ActivateHoming()
    {
        activeHoming = true;
        homingTimer = homingTime;
        smr.material = homingMaterial;
    }

    private void CheckIsOnGround()
    {
        if (charController.isGrounded && gravityVelocity < 0f)
        {
            gravityVelocity = -0.1f; // Ensure a small negative value to stick the player to the ground.

            if (isJumping)
            {
                isJumping = false;
                animator.SetBool("Jump_b", false); // Set the "Jump_b" parameter in the animator to false.
            }
        }
    }

    private void MovePlayer()
    {
        Vector3 moveDir = InputsController.GetInputValue<Vector2>("MoveDir"); // gets normalized input move directions from the input system.
        Vector3 playerVelocity = Vector3.zero;

        playerVelocity += cam.transform.right * moveDir.x * playerMoveSpeed;
        playerVelocity += cam.transform.forward * moveDir.y * playerMoveSpeed;
        playerVelocity *= 0.5f;
        playerVelocity.y = 0;

        if (playerVelocity != Vector3.zero)
        {
            Vector3 playerForward = playerVelocity;

            if (IsAiming)
            {
                playerForward = Vector3.zero;

                if (moveDir.y != 0)
                {
                    playerForward += cam.transform.right * moveDir.x * playerMoveSpeed;
                }

                playerForward += cam.transform.forward * Mathf.Abs(moveDir.y) * playerMoveSpeed;
                playerForward *= 0.5f;
                playerForward.y = 0;
            }

            transform.forward = Vector3.Lerp(transform.forward, playerForward, playerRotationSpeed * Time.deltaTime);
        }

        float idleMoveBlender = moveDir.magnitude; // Calculate the magnitude of the movement vector.
        animator.SetFloat("Speed_f", idleMoveBlender); // Set the "Speed_f" parameter in the animator based on the current speed.
        animator.SetBool("Static_b", !InputsController.OnInputTrigger("MoveDir")); // Set the "Static_b" parameter in the animator based on the movement state.

        playerVelocity.y = gravityVelocity;

        charController.Move(playerVelocity * Time.deltaTime);
    }

    private void ThrowGrenade()
    {
        if (InputsController.OnInputTrigger("ThrowGrenade") && Grenades > 0)
        {
            animator.SetInteger("WeaponType_int", 10);
            GameObject grenade = Instantiate(grenadePrefab, grenadeSpawnPoint.position, grenadeSpawnPoint.rotation);
            grenade.GetComponent<Grenade>().Throw(transform.forward);

            Grenades--;
        }
    }

    private void Aim()
    {
        if (IsAiming)
        {
            Vector3 cameraEulerAnglesY = transform.eulerAngles;

            cameraEulerAnglesY.y = cam.transform.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(cameraEulerAnglesY), playerRotationSpeed * Time.deltaTime);
        }

        if (!InputsController.OnInputTrigger("Aim"))
        {
            return;
        }

        IsAiming = !IsAiming;

        cam.GetComponent<CameraController>().ToggleCameraOffsets();

        animator.SetInteger("WeaponType_int", Convert.ToInt32(IsAiming));
        animator.SetBool("Shoot_b", false);
        animator.SetBool("Reload_b", false);
    }

    private void Shoot()
    {
        if (activeHoming)
        {
            homingTimer -= Time.deltaTime;
            if (homingTimer <= 0)
            {
                activeHoming = false;
                smr.material = normalMaterial;
            }
        }

        if (!InputsController.OnInputTrigger("Shoot") || !IsAiming)
        {
            if (!IsAiming)
            {
                animator.SetInteger("WeaponType_int", 0);
                animator.SetBool("Shoot_b", false);
            }

            else
            {
                animator.SetBool("Shoot_b", false);
            }

            return;
        }

        PlayShootClip();

        Transform bulletTarget = SetFirstTarget();
        if (bulletTarget != null)
        {
            Debug.Log(bulletTarget.parent.name); 
        }

        if (!activeHoming || bulletTarget == null)
        {
            Ray rayToCenter = new Ray(cam.position, cam.forward);

            Vector3 shootDir = Vector3.zero;

            if (Physics.Raycast(rayToCenter, out RaycastHit target, Mathf.Infinity, aimMask))
            {
                shootDir = (target.point - BulletSpawn.position).normalized;
                Debug.DrawRay(BulletSpawn.position, shootDir, Color.yellow);
            }
            else
            {
                shootDir = ((cam.position + cam.forward * 100) - BulletSpawn.position).normalized;
            }

            GameObject bullet = Instantiate(Bullet, BulletSpawn.position, BulletSpawn.rotation); // Instantiate the bullet
            bullet.GetComponent<PlayerBullet>().dir = shootDir; // Set the bullet direction
            //Set bullet damage
            WeaponLogic wL = currentWeapon.GetComponent<WeaponLogic>();
            bullet.GetComponent<PlayerBullet>().DamageDealt = wL.Damage;
        }
        else
        {
            GameObject go = Instantiate(Bullet, BulletSpawn.position, BulletSpawn.rotation); // Instantiate the bullet
            PlayerBullet bullet = go.GetComponent<PlayerBullet>();
            bullet.target = bulletTarget;
            //Set bullet damage
            WeaponLogic wL = currentWeapon.GetComponent<WeaponLogic>();
            bullet.GetComponent<PlayerBullet>().DamageDealt = wL.Damage;
            bullet.IsHoming = true;
        }

        animator.SetInteger("WeaponType_int", 1);
        animator.SetBool("Reload_b", false);
        animator.SetBool("Shoot_b", true);
    }

    private void PlayShootClip()
    {
        int randIndex = Random.Range(0, shootClips.Count);
        float randVolume = Random.Range(0.8f, 1.0f);
        AudioSource.PlayClipAtPoint(shootClips[randIndex], BulletSpawn.position, randVolume);
    }

    private void Jump()
    {
        if (charController.isGrounded && InputsController.OnInputTrigger("Jump"))
        {
            gravityVelocity = jumpForce; // Apply the jump force to the player's velocity.
            isJumping = true; // Set the jumping flag to true.
            animator.SetBool("Jump_b", true); // Set the "Jump_b" parameter in the animator to true.
            animator.SetBool("Jump_b", false); // Set the "Jump_b" parameter in the animator to false in order to prevent a doublejump.
        }
    }

    private void ApplyGravity()
    {
        gravityVelocity += Physics.gravity.y * Time.deltaTime; // Apply gravity to the player's velocity.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            other.GetComponent<PowerUp>().PickUp(gameObject);
        }
        else if (other.CompareTag("Weapon"))
        {
            // Disattiva l'arma di default
            defaultWeapon.SetActive(false);

            // Attiva l'arma d'assalto
            assaultWeapon.SetActive(true);
            animator.SetInteger("WeaponType_int", 2);

            other.gameObject.SetActive(false);
        }
    }

    internal void UpdateGrenadeText()
    {
        grenadeNumberText.text = grenades.ToString();
    }

    private Transform SetTarget()
    {
        Transform target = null;
        Collider[] enemyTargets = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);
        Debug.Log("Enemies detected: " + enemyTargets.Length);
        if (enemyTargets.Length > 0)
        {
            Transform nextTarget = null;
            Vector3 lowestDist = Vector3.zero;

            for (int i = 0; i < enemyTargets.Length; i++)
            {
                Transform possibleTarget = enemyTargets[i].transform.GetChild(2);
                Vector3 distToTarget = possibleTarget.position - transform.GetChild(0).position;
                float angleToTarget = Vector3.Angle(transform.forward, distToTarget);
                // check if enemy is within angle of vision
                if (angleToTarget < angleOfVision * 0.5f)
                {
                    Debug.Log("Enemy angle: " + angleToTarget);
                    Vector3 raycastStart = transform.GetChild(0).position;
                    Debug.DrawRay(raycastStart, distToTarget, Color.red, 10);
                    // check if enemy is NOT behind an obstacle
                    if (!Physics.Raycast(transform.GetChild(0).position, distToTarget.normalized, distToTarget.magnitude, obstacleMask))
                    {
                        if (nextTarget == null)
                        {
                            lowestDist = distToTarget;
                            nextTarget = possibleTarget;
                        }
                        else if (distToTarget.sqrMagnitude < lowestDist.sqrMagnitude)
                        {
                            lowestDist = distToTarget;
                            nextTarget = possibleTarget;
                        }
                    }
                }
            }
            Debug.Log(nextTarget);
            target = nextTarget;
        }

        return target;
    }

    private Transform SetFirstTarget()
    {
        Transform target = null;
        Collider[] enemyTargets = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);
        Vector3 myCentralPosition = transform.GetChild(0).position;

        if (enemyTargets.Length > 0)
        {
            Transform closestTarget = null;
            float lowestDist = float.MaxValue;
            Debug.Log("Enemies detected: " + enemyTargets.Length);
            for (int i = 0; i < enemyTargets.Length; i++)
            {
                //sometimes may happen the enemy could get null
                //'cause killed, so we'll skip the current enemy
                //if killed
                if (enemyTargets[i] == null)
                {
                    continue;
                }

                Transform possibleTarget = enemyTargets[i].GetComponent<EnemyLogic>().Center;
                Vector3 distToTarget = possibleTarget.position - myCentralPosition;
                float distance = Vector3.Distance(myCentralPosition, possibleTarget.position);
                float angleToTarget = Vector3.Angle(transform.forward, distToTarget.normalized);

                Debug.Log(enemyTargets[i].name);
                Debug.Log(distance);
                Debug.DrawRay(myCentralPosition, distToTarget, Color.red, 10);

                // check if enemy is inside the player viewRadius
                if (angleToTarget < angleOfVision * 0.5f)
                {
                    //check if enemy is not behind a wall
                    if (!Physics.Raycast(myCentralPosition, distToTarget.normalized, distToTarget.magnitude, obstacleMask))
                    {
                        if (closestTarget == null)
                        {
                            lowestDist = distance;
                            closestTarget = possibleTarget;
                        }
                        else if(distance < lowestDist)
                        {
                            lowestDist = distance;
                            closestTarget = possibleTarget;
                        }
                    }
                }
            }
            target = closestTarget;
        }
        //Debug.Log("Target is: " + target.parent.name);
        return target;
    }

    private void updateStats()
    {
        playerMoveSpeed = PlayerLogic.Instance.speed * PlayerLogic.Instance.speedMultiplier;
    }
}