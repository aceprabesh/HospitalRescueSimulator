using UnityEngine;
using UnityEngine.InputSystem;
using HospitalRescue.Application.Controllers.Player;
using HospitalRescue.Application.Presentation.Input;
using HospitalRescue.Domain.ScriptableObjects;
using HospitalRescue.Domain.Interfaces;
using HospitalRescue.Application.Presentation.UI.Components;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// Bootstraps the game scene with player, camera, and managers
    /// </summary>
    public class SceneBootstrap : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private Transform playerSpawnPoint;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject uiPrefab;
        
        [Header("Managers")]
        [SerializeField] private bool autoCreateManagers = true;
        
        private GameObject playerInstance;
        private HUDManager hudManager;
        private InteractionManager interactionManager;
        
        private void Awake()
        {
            if (autoCreateManagers)
            {
                EnsureGameManagerExists();
                EnsureSaveManagerExists();
            }
        }
        
        private void Start()
        {
            SetupPlayer();
            SetupUI();
            SetupInteractionManager();
        }
        
        private void EnsureGameManagerExists()
        {
            if (GameManager.Instance == null)
            {
                GameObject gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
                DontDestroyOnLoad(gm);
            }
        }
        
        private void EnsureSaveManagerExists()
        {
            if (FindFirstObjectByType<SaveManager>() == null)
            {
                GameObject sm = new GameObject("SaveManager");
                sm.AddComponent<SaveManager>();
            }
        }
        
        private void SetupPlayer()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("Player prefab not assigned!");
                return;
            }
            
            Vector3 spawnPos = playerSpawnPoint != null 
                ? playerSpawnPoint.position 
                : new Vector3(0, 1.5f, 0);
            
            playerInstance = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            playerInstance.name = "Player";
            
            // Setup input handler
            PlayerInputHandler inputHandler = playerInstance.GetComponent<PlayerInputHandler>();
            if (inputHandler == null)
            {
                inputHandler = playerInstance.AddComponent<PlayerInputHandler>();
            }
            
            // Setup player controller reference
            PlayerController controller = playerInstance.GetComponent<PlayerController>();
            if (controller == null)
            {
                controller = playerInstance.AddComponent<PlayerController>();
            }
            
            // Add CharacterController if missing
            if (playerInstance.GetComponent<CharacterController>() == null)
            {
                CharacterController cc = playerInstance.AddComponent<CharacterController>();
                cc.height = gameSettings.playerHeight;
                cc.radius = 0.3f;
                cc.center = new Vector3(0, gameSettings.playerHeight / 2f, 0);
            }
            
            // Ensure player has a camera
            Camera playerCam = playerInstance.GetComponentInChildren<Camera>();
            if (playerCam == null)
            {
                GameObject camObj = new GameObject("PlayerCamera");
                camObj.transform.SetParent(playerInstance.transform);
                camObj.transform.localPosition = new Vector3(0, 1.6f, 0);
                playerCam = camObj.AddComponent<Camera>();
                playerCam.nearClipPlane = 0.1f;
                playerCam.fieldOfView = 60f;
            }
            
            // Setup interaction manager
            interactionManager = playerInstance.GetComponent<InteractionManager>();
            if (interactionManager == null)
            {
                interactionManager = playerInstance.AddComponent<InteractionManager>();
            }
            interactionManager.interactionCamera = playerCam;
        }
        
        private void SetupUI()
        {
            if (uiPrefab != null)
            {
                Instantiate(uiPrefab);
            }
            
            hudManager = FindFirstObjectByType<HUDManager>();
        }
        
        private void SetupInteractionManager()
        {
            if (interactionManager == null)
            {
                interactionManager = FindFirstObjectByType<InteractionManager>();
            }
            
            if (interactionManager != null)
            {
                interactionManager.OnInteractableFound += OnInteractableFound;
                interactionManager.OnInteractableLost += OnInteractableLost;
            }
        }
        
        private void OnInteractableFound(IInteractable interactable)
        {
            Debug.Log($"Found interactable: {interactable.InteractionPrompt}");
        }
        
        private void OnInteractableLost()
        {
            Debug.Log("Lost interactable");
        }
        
        public Transform GetPlayerTransform()
        {
            return playerInstance?.transform;
        }
        
        public Camera GetPlayerCamera()
        {
            if (playerInstance != null)
            {
                return playerInstance.GetComponentInChildren<Camera>();
            }
            return Camera.main;
        }
    }
}
