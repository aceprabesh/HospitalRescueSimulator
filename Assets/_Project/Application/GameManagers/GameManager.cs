using UnityEngine;
using UnityEngine.InputSystem;
using HospitalRescue.Domain.Interfaces;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// Central game manager handling game state, initialization, and lifecycle
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("Managers")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private SaveManager saveManager;
        
        private GameState currentState;
        private bool isPaused;
        
        public GameState CurrentState => currentState;
        public bool IsPaused => isPaused;
        
        public enum GameState
        {
            MainMenu,
            Playing,
            Paused,
            GameOver,
            MissionComplete
        }
        
        // Events
        public System.Action<GameState> OnGameStateChanged;
        public System.Action OnGamePaused;
        public System.Action OnGameResumed;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            Initialize();
        }
        
        private void Initialize()
        {
            // Subscribe to events
            UnityEngine.Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 1;
            
            SetGameState(GameState.MainMenu);
        }
        
        public void SetGameState(GameState newState)
        {
            if (currentState == newState) return;
            
            GameState previousState = currentState;
            currentState = newState;
            
            HandleStateChange(previousState, newState);
            OnGameStateChanged?.Invoke(newState);
        }
        
        private void HandleStateChange(GameState from, GameState to)
        {
            switch (to)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    isPaused = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                    
                case GameState.Playing:
                    Time.timeScale = 1f;
                    isPaused = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                    
                case GameState.Paused:
                    Time.timeScale = 0f;
                    isPaused = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    OnGamePaused?.Invoke();
                    break;
                    
                case GameState.GameOver:
                case GameState.MissionComplete:
                    Time.timeScale = 1f;
                    isPaused = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
            }
        }
        
        public void StartGame()
        {
            SetGameState(GameState.Playing);
        }
        
        public void PauseGame()
        {
            if (currentState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
            }
        }
        
        public void ResumeGame()
        {
            if (currentState == GameState.Paused)
            {
                SetGameState(GameState.Playing);
                OnGameResumed?.Invoke();
            }
        }
        
        public void ReturnToMainMenu()
        {
            SetGameState(GameState.MainMenu);
        }
        
        public void TriggerGameOver()
        {
            SetGameState(GameState.GameOver);
        }
        
        public void MissionComplete()
        {
            SetGameState(GameState.MissionComplete);
        }
        
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
