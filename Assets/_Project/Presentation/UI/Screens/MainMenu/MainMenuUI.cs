using UnityEngine;
using UnityEngine.UI;
using HospitalRescue.Application.GameManagers;

namespace HospitalRescue.Application.Presentation.UI.Screens.MainMenu
{
    /// <summary>
    /// Main menu screen controller
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup mainMenuCanvas;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;
        
        private void Awake()
        {
            SetupButtons();
        }
        
        private void Start()
        {
            // Show main menu by default
            ShowMainMenu();
        }
        
        private void SetupButtons()
        {
            if (newGameButton != null)
                newGameButton.onClick.AddListener(OnNewGameClicked);
            
            if (continueButton != null)
                continueButton.onClick.AddListener(OnContinueClicked);
            
            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettingsClicked);
            
            if (quitButton != null)
                quitButton.onClick.AddListener(OnQuitClicked);
        }
        
        public void ShowMainMenu()
        {
            mainMenuCanvas.alpha = 1f;
            mainMenuCanvas.blocksRaycasts = true;
        }
        
        public void HideMainMenu()
        {
            mainMenuCanvas.alpha = 0f;
            mainMenuCanvas.blocksRaycasts = false;
        }
        
        private void OnNewGameClicked()
        {
            HideMainMenu();
            GameManager.Instance?.StartGame();
        }
        
        private void OnContinueClicked()
        {
            HideMainMenu();
            // Load saved game
            GameManager.Instance?.StartGame();
        }
        
        private void OnSettingsClicked()
        {
            Debug.Log("Settings menu not yet implemented");
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
