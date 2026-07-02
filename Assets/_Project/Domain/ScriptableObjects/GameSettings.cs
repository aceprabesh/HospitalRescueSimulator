using UnityEngine;

namespace HospitalRescue.Domain.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "HospitalRescue/Settings/Game")]
    public class GameSettings : ScriptableObject
    {
        [Header("Player Settings")]
        public float mouseSensitivity = 2f;
        public float playerHeight = 1.8f;
        public float playerCrouchHeight = 1.2f;
        public float walkSpeed = 5f;
        public float sprintSpeed = 8f;
        public float crouchSpeed = 2.5f;
        public float jumpForce = 5f;
        public float gravity = -20f;
        
        [Header("Interaction Settings")]
        public float interactionRange = 3f;
        public KeyCode interactionKey = KeyCode.E;
        
        [Header("Game Settings")]
        public int maxLives = 3;
        public float gameTimeLimit = 600f;
        public bool autoSaveEnabled = true;
        public float autoSaveInterval = 300f;
    }
}
