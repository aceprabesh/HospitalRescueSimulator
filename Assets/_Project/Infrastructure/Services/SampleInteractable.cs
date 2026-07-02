using UnityEngine;
using HospitalRescue.Domain.Interfaces;

namespace HospitalRescue.Infrastructure.Services
{
    /// <summary>
    /// Sample interactable object for testing the interaction system
    /// </summary>
    public class SampleInteractable : MonoBehaviour, IInteractable
    {
        [Header("Interaction Settings")]
        [SerializeField] private string interactionPrompt = "Press E to interact";
        [SerializeField] private float interactionTime = 1f;
        [SerializeField] private bool holdToInteract = false;
        
        [Header("Visual Feedback")]
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Color highlightColor = Color.yellow;
        
        private Material originalMaterial;
        private Renderer objectRenderer;
        private bool isHighlighted;
        
        public string InteractionPrompt => interactionPrompt;
        public float InteractionTime => interactionTime;
        public bool HoldToInteract => holdToInteract;
        
        private void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                originalMaterial = objectRenderer.material;
            }
        }
        
        public bool CanInteract()
        {
            return isHighlighted;
        }
        
        public void OnInteract()
        {
            Debug.Log($"[Interact] {gameObject.name} activated!");
            
            // Perform the interaction action here
            // Example: Open door, pick up item, activate machine, etc.
        }
        
        public void OnInteractStart()
        {
            Debug.Log($"[Interact] Started interacting with {gameObject.name}");
        }
        
        public void OnInteractEnd()
        {
            Debug.Log($"[Interact] Stopped interacting with {gameObject.name}");
        }
        
        public void SetHighlighted(bool highlighted)
        {
            isHighlighted = highlighted;
            
            if (objectRenderer != null && highlightMaterial != null)
            {
                objectRenderer.material = highlighted ? highlightMaterial : originalMaterial;
            }
            else if (objectRenderer != null && highlightMaterial == null)
            {
                // Fallback: use emission
                objectRenderer.material.EnableKeyword("_EMISSION");
                if (highlighted)
                {
                    objectRenderer.material.SetColor("_EmissionColor", highlightColor * 0.5f);
                }
                else
                {
                    objectRenderer.material.SetColor("_EmissionColor", Color.black);
                }
            }
        }
        
        private void OnDestroy()
        {
            if (objectRenderer != null && originalMaterial != null)
            {
                objectRenderer.material = originalMaterial;
            }
        }
    }
}
