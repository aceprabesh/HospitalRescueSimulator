using UnityEngine;
using UnityEngine.UI;
using HospitalRescue.Domain.Interfaces;

namespace HospitalRescue.Application.Controllers.Player
{
    /// <summary>
    /// Crosshair UI that responds to interaction system
    /// </summary>
    public class CrosshairUI : MonoBehaviour
    {
        [Header("Crosshair Elements")]
        [SerializeField] private Image crosshairImage;
        [SerializeField] private Image interactionProgressImage;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color interactableColor = Color.yellow;
        [SerializeField] private Color holdingColor = Color.green;
        
        private InteractionManager interactionManager;
        
        private void Awake()
        {
            if (interactionProgressImage != null)
                interactionProgressImage.fillAmount = 0f;
        }
        
        private void Start()
        {
            interactionManager = FindFirstObjectByType<InteractionManager>();
            
            if (interactionManager != null)
            {
                interactionManager.OnInteractableFound += OnInteractableFound;
                interactionManager.OnInteractableLost += OnInteractableLost;
                interactionManager.OnInteractionStart += OnInteractionStart;
                interactionManager.OnInteractionComplete += OnInteractionComplete;
            }
        }
        
        private void OnInteractableFound(IInteractable interactable)
        {
            if (crosshairImage != null)
                crosshairImage.color = interactableColor;
        }
        
        private void OnInteractableLost()
        {
            if (crosshairImage != null)
                crosshairImage.color = normalColor;
            
            if (interactionProgressImage != null)
                interactionProgressImage.fillAmount = 0f;
        }
        
        private void OnInteractionStart(IInteractable interactable)
        {
            if (crosshairImage != null)
                crosshairImage.color = holdingColor;
        }
        
        private void OnInteractionComplete(IInteractable interactable)
        {
            if (crosshairImage != null)
                crosshairImage.color = interactableColor;
            
            if (interactionProgressImage != null)
                interactionProgressImage.fillAmount = 0f;
        }
        
        private void Update()
        {
            if (interactionManager != null && interactionManager.CurrentInteractable != null)
            {
                float progress = interactionManager.GetInteractionProgress();
                if (interactionProgressImage != null)
                    interactionProgressImage.fillAmount = progress;
            }
        }
        
        private void OnDestroy()
        {
            if (interactionManager != null)
            {
                interactionManager.OnInteractableFound -= OnInteractableFound;
                interactionManager.OnInteractableLost -= OnInteractableLost;
                interactionManager.OnInteractionStart -= OnInteractionStart;
                interactionManager.OnInteractionComplete -= OnInteractionComplete;
            }
        }
    }
}
