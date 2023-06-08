using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private CharacterController controller; // Reference to the CharacterController component used for movement
    [SerializeField] private float movementSpeed = 6f; // Movement speed of the character
    [SerializeField] private float jumpForce = 5f; // Jump force of the character
    [SerializeField] private float gravity = 9.8f; // Gravity force applied to the character

    private InputAction moveAction; // Input action for movement
    private InputAction jumpAction; // Input action for jumping
    private InputAction shootAction; // Input action for shooting

    private Vector2 movementInput; // Movement input
    private Vector3 velocity; // Current velocity of the character

    private bool isGrounded; // Indicates whether the character is grounded

    private void Awake()
    {
        var actionAsset = new ThirdPersonActionAssets(); // Create an instance of the input action asset

        moveAction = actionAsset.Player.Move; // Get the movement input action from the asset
        jumpAction = actionAsset.Player.Jump; // Get the jump input action from the asset
        shootAction = actionAsset.Player.Shooting; // Get the shoot input action from the asset
    }

    private void OnEnable()
    {
        moveAction.Enable(); // Enable the movement input action
        moveAction.performed += OnMovePerformed; // Register the event for movement input
        moveAction.canceled += OnMoveCanceled; // Register the event for movement input cancellation

        jumpAction.Enable(); // Enable the jump input action
        jumpAction.performed += OnJumpPerformed; // Register the event for jump input

        shootAction.Enable(); // Enable the shoot input action
        shootAction.performed += OnShootPerformed; // Register the event for shoot input
    }

    private void OnDisable()
    {
        moveAction.Disable(); // Disable the movement input action
        moveAction.performed -= OnMovePerformed; // Remove the event for movement input
        moveAction.canceled -= OnMoveCanceled; // Remove the event for movement input cancellation

        jumpAction.Disable(); // Disable the jump input action
        jumpAction.performed -= OnJumpPerformed; // Remove the event for jump input

        shootAction.Disable(); // Disable the shoot input action
        shootAction.performed -= OnShootPerformed; // Remove the event for shoot input
    }

    private void Update()
    {
        ApplyGravity(); // Apply gravity to the character
        MovePlayer(); // Move the character
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y); // Calculate the movement vector based on the input
        movement = transform.TransformDirection(movement); // Transform the movement vector based on the character's direction
        movement *= movementSpeed; // Apply the movement speed to the movement vector

        controller.Move((movement + velocity) * Time.deltaTime); // Move the character using the CharacterController

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -0.1f; // Set a small downward velocity when the character is grounded to avoid jitter
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>(); // Read the value of the movement input
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero; // Cancel the movement input (character stops moving)
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            Debug.Log("Jump!"); // Print a debug message when the character jumps
            velocity.y = jumpForce; // Apply a jump force to the character
        }
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Shooting!"); // Print a debug message when the character shoots
    }

    private void ApplyGravity()
    {
        velocity.y -= gravity * Time.deltaTime; // Apply gravity to the character's vertical velocity

        if (controller.isGrounded)
        {
            isGrounded = true; // If the CharacterController is grounded, the character is considered grounded
        }
        else
        {
            isGrounded = false; // Otherwise, the character is not grounded
        }
    }
}