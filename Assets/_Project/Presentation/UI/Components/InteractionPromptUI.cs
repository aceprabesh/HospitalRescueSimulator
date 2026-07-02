using UnityEngine;
using TMPro;
using HospitalRescue.Domain.Interfaces;

namespace HospitalRescue.Application.Presentation.UI.Components
{
    /// <summary>
    /// Displays interaction prompts when player looks at interactable objects
    /// </summary>
    public class InteractionPromptUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private RectTransform progressBarFill;
        
        [Header("Settings")]
        [SerializeField] private float fadeSpeed = 5f;
        
        private float targetAlpha;
        private float currentProgress;
        
        private void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            
            Hide();
        }
        
        private void Update()
        {
            // Smooth fade
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
        }
        
        public void Show(IInteractable interactable)
        {
            if (interactable == null)
            {
                Hide();
                return;
            }
            
            promptText.text = interactable.InteractionPrompt;
            targetAlpha = 1f;
            gameObject.SetActive(true);
        }
        
        public void Show(string prompt)
        {
            promptText.text = prompt;
            targetAlpha = 1f;
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            targetAlpha = 0f;
            currentProgress = 0f;
            UpdateProgressBar();
        }
        
        public void UpdateProgress(float progress)
        {
            currentProgress = progress;
            UpdateProgressBar();
        }
        
        private void UpdateProgressBar()
        {
            if (progressBarFill != null)
            {
                progressBarFill.localScale = new Vector3(currentProgress, 1f, 1f);
            }
        }
        
        public bool IsVisible => canvasGroup.alpha > 0.1f;
    }
}
