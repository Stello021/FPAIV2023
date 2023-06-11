using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private CharacterController controller; // Reference to the CharacterController component for player movement.
    [SerializeField] private float movementSpeed = 6f; // Speed at which the player moves.
    [SerializeField] private float jumpForce = 5f; // Force applied when the player jumps.
    [SerializeField] private float gravity = 9.8f; // Gravity value affecting player movement.
    [SerializeField] private GameObject Bullet; // Reference to Bullet prefab.
    [SerializeField] private Transform BulletSpawn; // Reference to BulletSpown transform.
    [SerializeField] private Transform CentreCameraTarget; // Reference to the transform of an EmptyObject located in the center of the camera frame. 

    private Animator animator; // Reference to the Animator component for controlling animations.
    private InputAction moveAction; // Input action for player movement.
    private InputAction jumpAction; // Input action for player jump.
    private InputAction shootAction; // Input action for player shooting.

    private Vector2 movementInput; // Player movement input values.
    private Vector3 velocity; // Player's current velocity.

    private bool isGrounded; // Flag indicating if the player is grounded.
    private bool isJumping; // Flag indicating if the player is currently jumping.

    public Transform cam;

    public LayerMask aimMask;

    private void Awake()
    {
        var actionAsset = new ThirdPersonActionAssets(); // Create an instance of the ThirdPersonActionAssets class.

        moveAction = actionAsset.Player.Move; // Get the move input action from the action asset.
        jumpAction = actionAsset.Player.Jump; // Get the jump input action from the action asset.
        shootAction = actionAsset.Player.Shooting; // Get the shooting input action from the action asset.

        animator = GetComponent<Animator>(); // Get the Animator component attached to the same GameObject.
        cam = Camera.main.transform;
    }

    private void OnEnable()
    {
        moveAction.Enable(); // Enable the move input action.
        moveAction.performed += OnMovePerformed; // Register the move performed event callback.
        moveAction.canceled += OnMoveCanceled; // Register the move canceled event callback.

        jumpAction.Enable(); // Enable the jump input action.
        jumpAction.performed += OnJumpPerformed; // Register the jump performed event callback.

        shootAction.Enable(); // Enable the shoot input action.
        shootAction.performed += OnShootPerformed; // Register the shoot performed event callback.
    }

    private void OnDisable()
    {
        moveAction.Disable(); // Disable the move input action.
        moveAction.performed -= OnMovePerformed; // Unregister the move performed event callback.
        moveAction.canceled -= OnMoveCanceled; // Unregister the move canceled event callback.

        jumpAction.Disable(); // Disable the jump input action.
        jumpAction.performed -= OnJumpPerformed; // Unregister the jump performed event callback.

        shootAction.Disable(); // Disable the shoot input action.
        shootAction.performed -= OnShootPerformed; // Unregister the shoot performed event callback.
    }

    private void Update()
    {
        ApplyGravity(); // Apply gravity to the player's velocity.
        MovePlayer(); // Move the player based on the input and current velocity.
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y); // Create a movement vector from the input values.
        Vector3 movementNormalized = Vector3.Normalize(movement);
        if (movementNormalized != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(movementNormalized.x, movementNormalized.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Quaternion angle = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, 100 * Time.deltaTime);
            Vector3 rotatedMovement = angle * Vector3.forward;
            controller.Move(rotatedMovement * 10 * Time.deltaTime);
        }



        //movement = transform.TransformDirection(movement); // Transform the movement vector relative to the player's orientation.
        //movement *= movementSpeed; // Scale the movement vector by the movement speed.
        //controller.Move((movement + velocity) * Time.deltaTime); // Move the player using the CharacterController component.

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -0.1f; // Ensure a small negative value to stick the player to the ground.
            if (isJumping)
            {
                isJumping = false;
                animator.SetBool("Jump_b", false); // Set the "Jump_b" parameter in the animator to false.
            }
        }

        float currentSpeed = movement.magnitude; // Calculate the magnitude of the movement vector.
        animator.SetFloat("Speed_f", currentSpeed); // Set the "Speed_f" parameter in the animator based on the current speed.

        bool isMoving = currentSpeed > 0f; // Check if the player is currently moving.
        animator.SetBool("Static_b", !isMoving); // Set the "Static_b" parameter in the animator based on the movement state.
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
        if (isGrounded)
        {
            velocity.y = jumpForce; // Apply the jump force to the player's velocity.
            isJumping = true; // Set the jumping flag to true.
            animator.SetBool("Jump_b", true); // Set the "Jump_b" parameter in the animator to true.
            animator.SetBool("Jump_b", false); // Set the "Jump_b" parameter in the animator to false in order to prevent a doublejump.

        }
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("shoot");
        Vector2 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
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

        //Vector3 shootDir = (CentreCameraTarget.position - BulletSpawn.position).normalized; // Calculate the shooting direction

        GameObject bullet = Instantiate(Bullet, BulletSpawn.position, BulletSpawn.rotation); // Instantiate the bullet

        bullet.GetComponent<BulletLogic>().dir = shootDir; // Set the bullet direction
    }




    private void ApplyGravity()
    {
        velocity.y -= gravity * Time.deltaTime; // Apply gravity to the player's velocity.

        if (controller.isGrounded)
        {
            isGrounded = true; // Set the grounded flag to true if the player is on the ground.
        }
        else
        {
            isGrounded = false; // Set the grounded flag to false if the player is not on the ground.
        }
    }
}