using UnityEngine;
using UnityEngine.InputSystem;
using HospitalRescue.Application.Controllers.Player;
using HospitalRescue.Application.GameManagers;

namespace HospitalRescue.Application.Presentation.Input
{
    /// <summary>
    /// Handles player input using Unity's new Input System
    /// </summary>
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private InteractionManager interactionManager;
        
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction sprintAction;
        private InputAction crouchAction;
        private InputAction jumpAction;
        private InputAction interactAction;
        private InputAction pauseAction;
        
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput == null)
            {
                playerInput = gameObject.AddComponent<PlayerInput>();
            }
            
            SetupInputActions();
        }
        
        private void SetupInputActions()
        {
            // These will be configured in the InputActionAsset
            // For now, we set up the callbacks
        }
        
        private void OnEnable()
        {
            EnableInput();
        }
        
        private void OnDisable()
        {
            DisableInput();
        }
        
        public void EnableInput()
        {
            enabled = true;
        }
        
        public void DisableInput()
        {
            enabled = false;
        }
        
        #region Input System Callbacks
        
        public void OnMove(InputAction.CallbackContext context)
        {
            if (playerController != null)
            {
                Vector2 input = context.ReadValue<Vector2>();
                playerController.Move(new Vector3(input.x, 0, input.y));
            }
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            if (playerController != null)
            {
                Vector2 input = context.ReadValue<Vector2>();
                playerController.Rotate(input.x, input.y);
            }
        }
        
        public void OnSprint(InputAction.CallbackContext context)
        {
            if (playerController != null)
            {
                playerController.Sprint(context.performed);
            }
        }
        
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (playerController != null && context.performed)
            {
                playerController.Crouch(!playerController.IsCrouching);
            }
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if (playerController != null && context.performed)
            {
                playerController.RequestJump();
            }
        }
        
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (interactionManager != null)
            {
                interactionManager.OnInteract(context);
            }
        }
        
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (GameManager.Instance != null)
                {
                    if (GameManager.Instance.IsPaused)
                        GameManager.Instance.ResumeGame();
                    else
                        GameManager.Instance.PauseGame();
                }
            }
        }
        
        #endregion
    }
}
