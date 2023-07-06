using System;
using TMPro;
using UnityEngine;

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

    [Header("Shooting variables")]
    [SerializeField] private float nextShootTimer;
    private float nextShootTimeElapsed;

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
    public GameObject CurrentWeapon { get; private set; }
    public WeaponLogic CurrentWeaponController { get { return CurrentWeapon.GetComponent<WeaponLogic>(); } }

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
    public bool IsPlayerInputsActive { get; private set; }

    private void Start()
    {
        InputsController = new InputSysController();

        animatorController = GetComponent<Animator>(); // Get the Animator component attached to the same GameObject.
        charController = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        IsAiming = false;

        CurrentWeapon = defaultWeapon;
        animatorController.SetInteger("WeaponType_int", 1);

        BulletSpawn = CurrentWeapon.transform;
    }

    private void Update()
    {
        if (!IsPlayerInputsActive)
        {
            return;
        }

        TogglePauseMenu();

        if (isInPause)
        {
            return;
        }

        ThrowGrenade();
        Aim();
        Shoot();
        Reload();
    }

    private void LateUpdate()
    {
        UpdateStats();

        ApplyGravity();
        CheckIsOnGround();

        MovePlayer();
    }

    //this method is called on start UI button click
    //on tutorial panel (check the button inspector)
    public void TogglePlayerInputs()
    {
        IsPlayerInputsActive = !IsPlayerInputsActive;
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
        Cursor.lockState = isInPause ? CursorLockMode.Confined : CursorLockMode.Locked;
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

                if (Mathf.Abs(moveDir.y) > 0.5f)
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

        if (InputsController.OnInputTrigger("ThrowGrenade") &&
            Grenades > 0 &&
            nextGrenadeThrowElapsedTime <= 0 &&
            !CurrentWeaponController.IsReloading)
        {
            if (!isThrowingGrenade)
            {
                throwGrenadeElapsedTime = throwGrenadeTimer;
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
    }

    private void Reload()
    {
        if (!InputsController.OnInputTrigger("Reload") ||
            isThrowingGrenade ||
            nextGrenadeThrowElapsedTime > 0)
        {
            return;
        }

        CurrentWeaponController.ReloadManually();
    }

    private void Shoot()
    {
        nextShootTimeElapsed -= Time.deltaTime;

        if (activeHoming)
        {
            homingTimer -= Time.deltaTime;
            if (homingTimer <= 0)
            {
                activeHoming = false;
                smr.material = normalMaterial;
            }
        }

        bool shootInputTriggered = CurrentWeapon == assaultWeapon ? InputsController.OnInputPressed("Shoot") : InputsController.OnInputTrigger("Shoot");

        if (!shootInputTriggered ||
            !IsAiming ||
            nextShootTimeElapsed > 0 ||
            CurrentWeaponController.IsReloading ||
            isThrowingGrenade ||
            nextGrenadeThrowElapsedTime > 0)
        {
            animatorController.SetBool("Shoot_b", false);

            return;
        }

        Transform bulletTarget = SetFirstTarget();

        if (!activeHoming || bulletTarget == null)
        {
            Ray rayToCenter = new Ray(cam.position, cam.forward);
            Vector3 shootDir = Vector3.zero;

            if (Physics.Raycast(rayToCenter, out RaycastHit target, Mathf.Infinity, aimMask))
            {
                shootDir = (target.point - BulletSpawn.position).normalized;
            }
            else
            {
                shootDir = ((cam.position + cam.forward * 100) - BulletSpawn.position).normalized;
            }

            CurrentWeaponController.Fire(shootDir);
        }

        else
        {
            CurrentWeaponController.Fire(transform.forward, activeHoming, bulletTarget);
        }

        nextShootTimeElapsed = nextShootTimer;

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

        else if (other.CompareTag("Weapon") && !other.GetComponent<WeaponLogic>().IsWeaponHanded)
        {
            if (CurrentWeapon != assaultWeapon)
            {
                SwitchCurrentWeapon();
            }

            Destroy(other.gameObject);
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

            for (int i = 0; i < enemyTargets.Length; i++)
            {
                try
                {
                    Transform possibleTarget = enemyTargets[i].GetComponent<EnemyLogic>().Center;
                    Vector3 distToTarget = possibleTarget.position - myCentralPosition;
                    float distance = Vector3.Distance(myCentralPosition, possibleTarget.position);
                    float angleToTarget = Vector3.Angle(transform.forward, distToTarget.normalized);

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
                catch
                {
                    //sometimes may happen the enemy could get null
                    //'cause killed, so we'll catch the exception and
                    //skip the current enemy if killed
                    continue;
                }
            }

            target = closestTarget;
        }

        return target;
    }

    public void SwitchCurrentWeapon()
    {
        CurrentWeaponController.ReloadUI.SetActive(false);

        if (CurrentWeapon == defaultWeapon)
        {
            // Disattiva l'arma di default
            defaultWeapon.SetActive(false);

            // Attiva l'arma d'assalto
            assaultWeapon.SetActive(true);
            CurrentWeapon = assaultWeapon;
            animatorController.SetInteger("WeaponType_int", 2);
        }
        else
        {
            // Disattiva l'arma d'assalto
            assaultWeapon.SetActive(false);

            // Attiva l'arma di dedfault
            defaultWeapon.SetActive(true);
            CurrentWeapon = defaultWeapon;
            animatorController.SetInteger("WeaponType_int", 1);
        }

        CurrentWeaponController.InitUI_WeaponAmmo();
    }

    private void UpdateStats()
    {
        playerMoveSpeed = PlayerLogic.Instance.speed * PlayerLogic.Instance.speedMultiplier;
    }
}