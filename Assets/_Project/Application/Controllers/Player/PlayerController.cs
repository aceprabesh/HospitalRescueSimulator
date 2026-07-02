using UnityEngine;
using UnityEngine.InputSystem;
using HospitalRescue.Domain.Interfaces;
using HospitalRescue.Domain.ScriptableObjects;

namespace HospitalRescue.Application.Controllers.Player
{
    /// <summary>
    /// First-person player controller with movement, sprint, crouch, and jump
    /// </summary>
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [Header("References")]
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private Transform playerCamera;
        
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float sprintSpeed = 8f;
        [SerializeField] private float crouchSpeed = 2.5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = -20f;
        
        [Header("Mouse Look")]
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float maxLookAngle = 80f;
        
        [Header("Crouch")]
        [SerializeField] private float playerHeight = 1.8f;
        [SerializeField] private float crouchHeight = 1.2f;
        [SerializeField] private float crouchTransitionSpeed = 10f;
        
        private CharacterController characterController;
        private Vector2 moveInput;
        private Vector2 lookInput;
        private Vector3 velocity;
        private float currentHeight;
        private float verticalRotation;
        private bool isSprinting;
        private bool isCrouching;
        private bool isGrounded;
        private bool jumpRequested;
        
        public bool IsSprinting => isSprinting;
        public bool IsCrouching => isCrouching;
        public bool IsGrounded => isGrounded;
        
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                characterController = gameObject.AddComponent<CharacterController>();
            }
            
            // Apply settings
            walkSpeed = gameSettings.walkSpeed;
            sprintSpeed = gameSettings.sprintSpeed;
            crouchSpeed = gameSettings.crouchSpeed;
            jumpForce = gameSettings.jumpForce;
            gravity = gameSettings.gravity;
            mouseSensitivity = gameSettings.mouseSensitivity;
            playerHeight = gameSettings.playerHeight;
            crouchHeight = gameSettings.playerCrouchHeight;
            
            currentHeight = playerHeight;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void Update()
        {
            CheckGrounded();
            HandleMovement();
            HandleJump();
            HandleCrouch();
            ApplyGravity();
            ApplyMovement();
        }
        
        private void CheckGrounded()
        {
            isGrounded = characterController.isGrounded;
        }
        
        public void Move(Vector3 direction)
        {
            moveInput = new Vector2(direction.x, direction.z);
        }
        
        public void Rotate(float mouseX, float mouseY)
        {
            lookInput = new Vector2(mouseX, mouseY);
            
            // Horizontal rotation (yaw)
            transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);
            
            // Vertical rotation (pitch)
            verticalRotation -= lookInput.y * mouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
        
        public void Sprint(bool sprinting)
        {
            if (isCrouching) return;
            isSprinting = sprinting && moveInput.y > 0;
        }
        
        public void Crouch(bool crouching)
        {
            isCrouching = crouching;
        }
        
        private void HandleMovement()
        {
            float targetSpeed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : walkSpeed);
            
            Vector3 forward = transform.forward * moveInput.y;
            Vector3 right = transform.right * moveInput.x;
            Vector3 moveDirection = (forward + right).normalized * targetSpeed;
            
            velocity.x = moveDirection.x;
            velocity.z = moveDirection.z;
        }
        
        private void HandleJump()
        {
            if (jumpRequested && isGrounded && !isCrouching)
            {
                velocity.y = jumpForce;
                jumpRequested = false;
            }
        }
        
        public void RequestJump()
        {
            if (isGrounded && !isCrouching)
            {
                jumpRequested = true;
            }
        }
        
        private void HandleCrouch()
        {
            float targetHeight = isCrouching ? crouchHeight : playerHeight;
            currentHeight = Mathf.Lerp(currentHeight, targetHeight, crouchTransitionSpeed * Time.deltaTime);
            characterController.height = currentHeight;
            characterController.center = new Vector3(0, currentHeight / 2f, 0);
        }
        
        private void ApplyGravity()
        {
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }
        }
        
        private void ApplyMovement()
        {
            Vector3 move = new Vector3(velocity.x, velocity.y, velocity.z);
            characterController.Move(move * Time.deltaTime);
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
            Rotate(lookInput.x, lookInput.y);
        }
        
        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
                Sprint(true);
            else if (context.canceled)
                Sprint(false);
        }
        
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
                Crouch(!isCrouching);
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
                RequestJump();
        }
    }
}
