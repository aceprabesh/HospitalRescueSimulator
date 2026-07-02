using UnityEngine;
using UnityEngine.UI;
using HospitalRescue.Application.GameManagers;

namespace HospitalRescue.Application.Presentation.UI.Screens.PauseMenu
{
    /// <summary>
    /// Pause menu screen controller
    /// </summary>
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup pauseCanvas;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;
        
        [Header("Settings Panel")]
        [SerializeField] private CanvasGroup settingsPanel;
        
        private bool isOpen;
        
        private void Awake()
        {
            SetupButtons();
            ClosePauseMenu();
        }
        
        private void SetupButtons()
        {
            if (resumeButton != null)
                resumeButton.onClick.AddListener(OnResumeClicked);
            
            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettingsClicked);
            
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            
            if (quitButton != null)
                quitButton.onClick.AddListener(OnQuitClicked);
        }
        
        public void TogglePauseMenu()
        {
            if (isOpen)
                ClosePauseMenu();
            else
                OpenPauseMenu();
        }
        
        public void OpenPauseMenu()
        {
            isOpen = true;
            pauseCanvas.alpha = 1f;
            pauseCanvas.blocksRaycasts = true;
            Time.timeScale = 0f;
        }
        
        public void ClosePauseMenu()
        {
            isOpen = false;
            pauseCanvas.alpha = 0f;
            pauseCanvas.blocksRaycasts = false;
            
            if (settingsPanel != null)
            {
                settingsPanel.alpha = 0f;
                settingsPanel.blocksRaycasts = false;
            }
            
            Time.timeScale = 1f;
        }
        
        private void OnResumeClicked()
        {
            ClosePauseMenu();
            GameManager.Instance?.ResumeGame();
        }
        
        private void OnSettingsClicked()
        {
            if (settingsPanel != null)
            {
                settingsPanel.alpha = 1f;
                settingsPanel.blocksRaycasts = true;
            }
        }
        
        private void OnMainMenuClicked()
        {
            ClosePauseMenu();
            GameManager.Instance?.ReturnToMainMenu();
        }
        
        private void OnQuitClicked()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
