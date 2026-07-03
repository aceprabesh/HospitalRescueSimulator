using UnityEngine;
using UnityEngine.InputSystem;
using HospitalRescue.Domain.Interfaces;
using HospitalRescue.Domain.ScriptableObjects;
using System.Collections.Generic;

namespace HospitalRescue.Application.Controllers.Player
{
    /// <summary>
    /// Manages player interactions with objects in the world
    /// </summary>
    public class InteractionManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] public Camera interactionCamera;
        
        [Header("Interaction Settings")]
        [SerializeField] private float interactionRange = 3f;
        [SerializeField] private LayerMask interactionLayer;
        
        private IInteractable currentInteractable;
        private IInteractable heldInteractable;
        private float interactionProgress;
        private bool isInteracting;
        
        // Events
        public System.Action<IInteractable> OnInteractableFound;
        public System.Action OnInteractableLost;
        public System.Action<IInteractable> OnInteractionStart;
        public System.Action<IInteractable> OnInteractionComplete;
        
        public IInteractable CurrentInteractable => currentInteractable;
        
        private void Awake()
        {
            if (interactionCamera == null)
            {
                interactionCamera = Camera.main;
            }
            
            if (gameSettings != null)
            {
                interactionRange = gameSettings.interactionRange;
            }
        }
        
        private void Update()
        {
            CheckForInteractables();
            HandleHoldInteraction();
        }
        
        private void CheckForInteractables()
        {
            IInteractable interactable = null;
            
            // Raycast from camera center
            Ray ray = new Ray(interactionCamera.transform.position, interactionCamera.transform.forward);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
            {
                IInteractable foundInteractable = hit.collider.GetComponent<IInteractable>();
                if (foundInteractable != null && foundInteractable.CanInteract())
                {
                    interactable = foundInteractable;
                }
            }
            
            // Update current interactable
            if (interactable != currentInteractable)
            {
                if (currentInteractable != null && isInteracting)
                {
                    CancelInteraction();
                }
                
                currentInteractable = interactable;
                
                if (currentInteractable != null)
                {
                    OnInteractableFound?.Invoke(currentInteractable);
                }
                else
                {
                    OnInteractableLost?.Invoke();
                }
            }
        }
        
        private void HandleHoldInteraction()
        {
            if (heldInteractable == null) return;
            
            if (isInteracting)
            {
                interactionProgress += Time.deltaTime / heldInteractable.InteractionTime;
                
                if (interactionProgress >= 1f)
                {
                    CompleteInteraction();
                }
            }
        }
        
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                StartInteraction();
            }
            else if (context.canceled)
            {
                CancelInteraction();
            }
        }
        
        private void StartInteraction()
        {
            if (currentInteractable == null) return;
            
            if (currentInteractable.HoldToInteract)
            {
                heldInteractable = currentInteractable;
                isInteracting = true;
                interactionProgress = 0f;
                OnInteractionStart?.Invoke(currentInteractable);
            }
            else
            {
                currentInteractable.OnInteract();
                OnInteractionComplete?.Invoke(currentInteractable);
            }
        }
        
        private void CancelInteraction()
        {
            if (heldInteractable != null)
            {
                heldInteractable.OnInteractEnd();
                heldInteractable = null;
            }
            isInteracting = false;
            interactionProgress = 0f;
        }
        
        private void CompleteInteraction()
        {
            if (heldInteractable != null)
            {
                heldInteractable.OnInteract();
                heldInteractable.OnInteractEnd();
                OnInteractionComplete?.Invoke(heldInteractable);
            }
            
            heldInteractable = null;
            isInteracting = false;
            interactionProgress = 0f;
        }
        
        public float GetInteractionProgress()
        {
            return interactionProgress;
        }
    }
}
