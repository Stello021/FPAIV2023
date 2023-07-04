using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Player variables")]
    [SerializeField] private float playerMoveSpeed = 6f; // Speed at which the player moves.
    [SerializeField] private float playerRotationSpeed = 6f; // Speed at which the player moves.
    [SerializeField] private float playerTurnRate = 6f; // Speed at which the player moves.
    [SerializeField] private float accelerationSpeed = 2f; // Speed at which the player moves.

    private CharacterController charController; // Reference to the CharacterController component for player movement.
    private Animator animatorController; // Reference to the Animator component for controlling animations.

    private float gravityVelocity; // Player's current velocity.
    private Vector2 currentMoveDirAccelerated; // Player's move direction multiplied by acceleration.
    private bool isJumping; // Flag indicating if the player is currently jumping.

    [Header("Grenade variables")]
    private bool isThrowingGrenade; // Flag indicating if the player is currently launching a grenade.
    private bool hasThrownGrenade; // Flag indicating if the player has thrown a grenade.
    [SerializeField] private float throwGrenadeTimer;
    [SerializeField] private float nextGrenadeThrowTimer;
    private float throwGrenadeElapsedTime;
    private float nextGrenadeThrowElapsedTime;

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
            animatorController.SetBool("IsAiming", value);
        }
    }

    private bool isAiming;

    [Header("\nWeapon reference variables")]
    [SerializeField] private GameObject defaultWeapon;
    [SerializeField] private GameObject assaultWeapon;
    public GameObject currentWeapon;

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

    private void Start()
    {
        InputsController = new InputSysController();

        animatorController = GetComponent<Animator>(); // Get the Animator component attached to the same GameObject.
        charController = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        IsAiming = false;

        currentWeapon = defaultWeapon;
        BulletSpawn = currentWeapon.transform;
    }

    private void Update()
    {
        TogglePauseMenu();

        if (!isInPause)
        {
            ThrowGrenade();
            Aim();
            Shoot();

            updateStats();

            ApplyGravity();
            CheckIsOnGround();

            MovePlayer();
        }
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
        Cursor.lockState = isInPause ? CursorLockMode.None : CursorLockMode.Locked;
        // da mettere confined nella build finale
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
                animatorController.SetBool("Jump_b", false); // Set the "Jump_b" parameter in the animator to false.
            }
        }
    }

    private void MovePlayer()
    {
        Vector2 moveDir = InputsController.GetInputValue<Vector2>("MoveDir"); // gets normalized input move directions from the input system.
        Vector3 playerVelocity = Vector3.zero;

        currentMoveDirAccelerated = Vector2.Lerp(currentMoveDirAccelerated, moveDir, accelerationSpeed * Time.deltaTime);

        playerVelocity += cam.transform.right * currentMoveDirAccelerated.x * playerMoveSpeed;
        playerVelocity += cam.transform.forward * currentMoveDirAccelerated.y * playerMoveSpeed;
        playerVelocity *= 0.5f;
        playerVelocity.y = 0;

        if (playerVelocity != Vector3.zero)
        {
            Vector3 playerForward = playerVelocity;

            if (IsAiming)
            {
                playerForward = Vector3.forward;

                if (moveDir.y != 0)
                {
                    playerForward += cam.transform.right * moveDir.x * playerMoveSpeed;
                }

                playerForward += cam.transform.forward * Mathf.Abs(moveDir.y) * playerMoveSpeed;
                playerForward *= 0.5f;
                playerForward.y = 0;
            }

            transform.forward = Vector3.Lerp(transform.forward, playerForward, playerTurnRate * Time.deltaTime);
        }

        float idleMoveBlender = currentMoveDirAccelerated.magnitude; // Calculate the magnitude of the movement vector.
        animatorController.SetFloat("Speed_f", idleMoveBlender); // Set the "Speed_f" parameter in the animator based on the current speed.
        animatorController.SetBool("Static_b", !InputsController.OnInputTrigger("MoveDir")); // Set the "Static_b" parameter in the animator based on the movement state.

        playerVelocity.y = gravityVelocity;

        charController.Move(playerVelocity * Time.deltaTime);
    }

    private void ThrowGrenade()
    {
        if (hasThrownGrenade)
        {
            nextGrenadeThrowElapsedTime -= Time.deltaTime;
        }

        if (InputsController.OnInputTrigger("ThrowGrenade") && Grenades > 0 && nextGrenadeThrowElapsedTime <= 0)
        {
            if (!isThrowingGrenade)
            {
                throwGrenadeElapsedTime = isAiming || InputsController.GetInputValue<Vector2>("MoveDir") != Vector2.zero ? throwGrenadeTimer + 0.2f : throwGrenadeTimer;
            }

            animatorController.SetBool("IsThrowingGrenade", true);
            isThrowingGrenade = true;
            hasThrownGrenade = false;
        }

        if (!isThrowingGrenade)
        {
            return;
        }

        throwGrenadeElapsedTime -= Time.deltaTime;

        if (throwGrenadeElapsedTime <= 0)
        {
            GameObject grenade = Instantiate(grenadePrefab, grenadeSpawnPoint.position, grenadeSpawnPoint.rotation);
            grenade.GetComponent<Grenade>().Throw(transform.forward);

            Grenades--;

            isThrowingGrenade = false;
            hasThrownGrenade = true;

            nextGrenadeThrowElapsedTime = nextGrenadeThrowTimer;
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

        animatorController.SetInteger("WeaponType_int", Convert.ToInt32(IsAiming));
        animatorController.SetBool("Shoot_b", false);
        animatorController.SetBool("Reload_b", false);
    }

    private void Shoot()
    {
        if (animatorController.GetBool("IsThrowingGrenade") || nextGrenadeThrowElapsedTime > 0)
        {
            return;
        }

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
                animatorController.SetInteger("WeaponType_int", 0);
                animatorController.SetBool("Shoot_b", false);
            }
            else
            {
                animatorController.SetBool("Shoot_b", false);
            }

            return;
        }


        Transform bulletTarget = SetFirstTarget();
        GameObject bulletPrefab = currentWeapon.GetComponent<WeaponLogic>().bulletPrefab;

        if (!activeHoming || bulletTarget == null)
        {
            Ray rayToCenter = new Ray(cam.position, cam.forward);

            Vector3 shootDir = Vector3.zero;

            if (Physics.Raycast(rayToCenter, out RaycastHit target, Mathf.Infinity, aimMask))
            {
                shootDir = (target.point - BulletSpawn.position).normalized;
                //Debug.DrawRay(BulletSpawn.position, shootDir, Color.yellow);
            }
            else
            {
                shootDir = ((cam.position + cam.forward * 100) - BulletSpawn.position).normalized;
            }

            //GameObject bullet = WeaponManager.Instance.GetPlayerBullet(); // Get a bullet from the pool
            //bullet.transform.position = BulletSpawn.position;
            //bullet.transform.rotation = BulletSpawn.rotation;
            //bullet.GetComponent<PlayerBullet>().dir = shootDir; // Set the bullet direction

            //WeaponLogic wL = currentWeapon.GetComponent<WeaponLogic>();
            //bullet.GetComponent<PlayerBullet>().DamageDealt = wL.damage; // Set bullet damage
            //bullet.SetActive(true);

            WeaponLogic weaponLogic = currentWeapon.GetComponent<WeaponLogic>();
            weaponLogic.Fire(shootDir);
        }
        else
        {
            //GameObject bullet = WeaponManager.Instance.GetPlayerBullet(); // Get a bullet from the pool
            //bullet.transform.position = BulletSpawn.position;
            //bullet.transform.rotation = BulletSpawn.rotation;
            //PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
            //playerBullet.target = bulletTarget;
            //// Set bullet damage
            //WeaponLogic wL = currentWeapon.GetComponent<WeaponLogic>();
            //playerBullet.DamageDealt = wL.damage;
            //playerBullet.IsHoming = true;
            //bullet.SetActive(true);

            WeaponLogic weaponLogic = currentWeapon.GetComponent<WeaponLogic>();
            weaponLogic.Fire(transform.forward, activeHoming, bulletTarget);
        }

        animatorController.SetInteger("WeaponType_int", 1);
        animatorController.SetBool("Reload_b", false);
        animatorController.SetBool("Shoot_b", true);
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
        else if (other.CompareTag("Weapon") && other.transform.parent == null)
        {
            //// Disattiva l'arma di default
            //defaultWeapon.SetActive(false);

            //// Attiva l'arma d'assalto
            //assaultWeapon.SetActive(true);
            //currentWeapon = assaultWeapon;
            //animatorController.SetInteger("WeaponType_int", 2);
            switchCurrentWeapon();

            other.gameObject.SetActive(false);
        }
    }

    internal void UpdateGrenadeText()
    {
        grenadeNumberText.text = grenades.ToString();
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
            //Debug.Log("Enemies detected: " + enemyTargets.Length);

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

                //Debug.Log(enemyTargets[i].name);
                //Debug.Log(distance);
                //Debug.DrawRay(myCentralPosition, distToTarget, Color.red, 10);

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
                        else if (distance < lowestDist)
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
    public void switchCurrentWeapon()
    {
        if (currentWeapon = defaultWeapon)
        {
            // Disattiva l'arma di default
            defaultWeapon.SetActive(false);

            // Attiva l'arma d'assalto
            assaultWeapon.SetActive(true);
            currentWeapon = assaultWeapon;
            animatorController.SetInteger("WeaponType_int", 2);
        }
        else
        {
            // Disattiva l'arma d'assalto
            assaultWeapon.SetActive(false);

            // Attiva l'arma di dedfault
            defaultWeapon.SetActive(true);
            currentWeapon = defaultWeapon;
            animatorController.SetInteger("WeaponType_int", 1);
        }
    }

    private void updateStats()
    {
        playerMoveSpeed = PlayerLogic.Instance.speed * PlayerLogic.Instance.speedMultiplier;
    }
}