using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float playerMoveSpeed = 6f; // Speed at which the player moves.
    [SerializeField] private float playerRotationSpeed = 6f; // Speed at which the player moves.
    [SerializeField] private float jumpForce = 5f; // Force applied when the player jumps.
    [SerializeField] private GameObject Bullet; // Reference to Bullet prefab.
    [SerializeField] private Transform BulletSpawn; // Reference to BulletSpown transform.
    [SerializeField] private Transform CentreCameraTarget; // Reference to the transform of an EmptyObject located in the center of the camera frame. 

    private CharacterController charController; // Reference to the CharacterController component for player movement.
    private Animator animator; // Reference to the Animator component for controlling animations.
    private InputAction moveAction; // Input action for player movement.
    private InputAction jumpAction; // Input action for player jump.
    private InputAction shootAction; // Input action for player shooting.

    private Vector2 movementInput; // Player movement input values.
    private Vector3 gravityVelocity; // Player's current velocity.

    [SerializeField] bool isGrounded; // Flag indicating if the player is grounded.
    private bool isJumping; // Flag indicating if the player is currently jumping.

    [HideInInspector] public Transform cam;

    public LayerMask aimMask;

    [Header("PowerUp variables")]
    public float hp;
    public float armour;
    public int grenades;

    [SerializeField] GameObject grenadePrefab;
    [SerializeField] Transform grenadeSpawnPoint;

    [Header("Homing variables")]
    public bool activeHoming;
    [SerializeField] float homingTimer;
    [SerializeField] float homingTime;
    [SerializeField] float viewRadius = 300;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float rotSpeed;
    [SerializeField] float angleOfVision = 180;



    private void Awake()
    {
        var actionAsset = new ThirdPersonActionAssets(); // Create an instance of the ThirdPersonActionAssets class.

        moveAction = actionAsset.Player.Move; // Get the move input action from the action asset.
        jumpAction = actionAsset.Player.Jump; // Get the jump input action from the action asset.
        shootAction = actionAsset.Player.Shooting; // Get the shooting input action from the action asset.

        animator = GetComponent<Animator>(); // Get the Animator component attached to the same GameObject.
        cam = Camera.main.transform;
    }

    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.Enable(); // Enable the move input action.
        moveAction.performed += OnMovePerformed; // Register the move performed event callback.
        moveAction.canceled += OnMoveCanceled; // Register the move canceled event callback.

        //jumpAction.Enable(); // Enable the jump input action.
        //jumpAction.performed += OnJumpPerformed; // Register the jump performed event callback.

        shootAction.Enable(); // Enable the shoot input action.
        shootAction.performed += OnShootPerformed; // Register the shoot performed event callback.
    }

    private void OnDisable()
    {
        moveAction.Disable(); // Disable the move input action.
        moveAction.performed -= OnMovePerformed; // Unregister the move performed event callback.
        moveAction.canceled -= OnMoveCanceled; // Unregister the move canceled event callback.

        //jumpAction.Disable(); // Disable the jump input action.
        //jumpAction.performed -= OnJumpPerformed; // Unregister the jump performed event callback.

        shootAction.Disable(); // Disable the shoot input action.
        shootAction.performed -= OnShootPerformed; // Unregister the shoot performed event callback.
    }

    private void Update()
    {
        Jump();
        ApplyGravity();
        CheckIsOnGround();

        MovePlayer();

        if (activeHoming)
        {
            homingTimer = homingTime;
            homingTimer -= Time.deltaTime;

            if (homingTimer <= 0)
            {
                activeHoming = false;
            }
        }

        // implementare il tasto per lanciare la granata col nuovo input system
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject grenade = Instantiate(grenadePrefab, grenadeSpawnPoint.position, grenadeSpawnPoint.rotation);
            grenade.GetComponent<Grenade>().Throw(transform.forward);
        }
    }

    public void Jump()
    {
        if (charController.isGrounded && InputSysController.Instance.Inputs["Jump"].triggered)
        {
            gravityVelocity.y = jumpForce; // Apply the jump force to the player's velocity.
            isJumping = true; // Set the jumping flag to true.
            animator.SetBool("Jump_b", true); // Set the "Jump_b" parameter in the animator to true.
            animator.SetBool("Jump_b", false); // Set the "Jump_b" parameter in the animator to false in order to prevent a doublejump.

        }
    }

    public void CheckIsOnGround()
    {
        if (charController.isGrounded && gravityVelocity.y < 0f)
        {
            gravityVelocity.y = -0.1f; // Ensure a small negative value to stick the player to the ground.

            if (isJumping)
            {
                isJumping = false;
                animator.SetBool("Jump_b", false); // Set the "Jump_b" parameter in the animator to false.
            }
        }
    }

    private void MovePlayer()
    {
        Vector3 moveDir = new Vector3(movementInput.x, 0f, movementInput.y); // Create a movement vector from the input values.
        Vector3 playerVelocity = Vector3.zero;

        // movement influenced by camera forward (added by Giulio)
        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();
        }

        playerVelocity += cam.transform.right * moveDir.x * playerMoveSpeed;
        playerVelocity += cam.transform.forward * moveDir.z * playerMoveSpeed;
        playerVelocity *= 0.5f;
        playerVelocity.y = 0;

        if (playerVelocity != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, playerVelocity, playerRotationSpeed * Time.deltaTime);
        }

        float currentSpeed = moveDir.magnitude; // Calculate the magnitude of the movement vector.
        animator.SetFloat("Speed_f", currentSpeed); // Set the "Speed_f" parameter in the animator based on the current speed.

        bool isMoving = currentSpeed > 0f; // Check if the player is currently moving.
        animator.SetBool("Static_b", !isMoving); // Set the "Static_b" parameter in the animator based on the movement state.

        playerVelocity.y = gravityVelocity.y;

        charController.Move(playerVelocity * Time.deltaTime);
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>(); // Read the movement input values from the input action.
        animator.SetBool("Static_b", false); // Set the "Static_b" parameter in the animator to false.
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero; // Reset the movement input values.
        animator.SetBool("Static_b", true); // Set the "Static_b" parameter in the animator to true.
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("jump");

        if (isGrounded)
        {
            gravityVelocity.y = jumpForce; // Apply the jump force to the player's velocity.
            isJumping = true; // Set the jumping flag to true.
            animator.SetBool("Jump_b", true); // Set the "Jump_b" parameter in the animator to true.
            animator.SetBool("Jump_b", false); // Set the "Jump_b" parameter in the animator to false in order to prevent a doublejump.

        }
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (!activeHoming)
        {
            Ray rayToCenter = new Ray(cam.position, cam.forward);

            Vector3 shootDir = Vector3.zero;

            if (Physics.Raycast(rayToCenter, out RaycastHit target, Mathf.Infinity, aimMask))
            {
                shootDir = (target.point - BulletSpawn.position).normalized;
            }
            else
            {
                shootDir = (cam.position + cam.forward * 1000) - BulletSpawn.position;
            }

            GameObject bullet = Instantiate(Bullet, BulletSpawn.position, BulletSpawn.rotation); // Instantiate the bullet

            //Destroy(bullet, 10);
            bullet.GetComponent<BulletLogic>().dir = shootDir; // Set the bullet direction 
        }
        else
        {
            GameObject bullet = Instantiate(Bullet, BulletSpawn.position, BulletSpawn.rotation); // Instantiate the bullet

            bullet.GetComponent<BulletLogic>().IsHoming = true; 
            bullet.GetComponent<BulletLogic>().target = SetTarget(); // Set the bullet direction 

        }
    }

    private void ApplyGravity()
    {
        gravityVelocity.y += Physics.gravity.y * Time.deltaTime; // Apply gravity to the player's velocity.

        if (charController.isGrounded)
        {
            isGrounded = true; // Set the grounded flag to true if the player is on the ground.
        }

        else
        {
            isGrounded = false; // Set the grounded flag to false if the player is not on the ground.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            other.GetComponent<PowerUp>().PickUp(gameObject);
        }
    }

    internal void UpdateHPText()
    {
        throw new NotImplementedException();
    }

    internal void UpdateArmourText()
    {
        throw new NotImplementedException();
    }

    private Transform SetTarget()
    {
        Transform target = null;
        Collider[] enemyTargets = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);
        //Debug.Log("Detected enemies: " + enemyTargets.Length);
        if (enemyTargets.Length > 0)
        {
            Transform nextTarget = null;
            Vector3 lowestDist = Vector3.zero;

            for (int i = 0; i < enemyTargets.Length; i++)
            {
                Transform possibleTarget = enemyTargets[i].transform;
                Vector3 distToTarget = possibleTarget.position - transform.position;
                float angleToTarget = Vector3.Angle(transform.forward, distToTarget.normalized);
                // check if enemy is within angle of vision
                if (angleToTarget < angleOfVision * 0.5f)
                {
                    // check if enemy is NOT behind a wall
                    if (!Physics.Raycast(transform.position, distToTarget.normalized, distToTarget.magnitude, obstacleMask))
                    {
                        if (nextTarget == null)
                        {
                            lowestDist = distToTarget;
                            nextTarget = possibleTarget;
                        }
                        else if (distToTarget.magnitude < lowestDist.magnitude)
                        {
                            lowestDist = distToTarget;
                            nextTarget = possibleTarget;
                        }
                    }
                }
            }

            target = nextTarget;
            Debug.Log(target);
        }

        return target;
    }
}