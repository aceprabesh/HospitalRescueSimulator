using UnityEngine;
using TMPro;
using HospitalRescue.Application.Controllers.Player;

namespace HospitalRescue.Application.Presentation.UI.Components
{
    /// <summary>
    /// Main HUD manager displaying health, vitals, timer, and objectives
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        [Header("HUD Elements")]
        [SerializeField] private CanvasGroup hudCanvas;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI heartRateText;
        [SerializeField] private TextMeshProUGUI bloodPressureText;
        [SerializeField] private TextMeshProUGUI oxygenText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI objectiveText;
        [SerializeField] private TextMeshProUGUI scoreText;
        
        [Header("References")]
        [SerializeField] private InteractionManager interactionManager;
        
        [Header("Settings")]
        [SerializeField] private float updateInterval = 0.1f;
        
        private float timer;
        private float nextUpdate;
        private int currentScore;
        private string currentObjective = "Explore the hospital";
        
        public int Score => currentScore;
        
        private void Awake()
        {
            if (hudCanvas != null)
                hudCanvas.alpha = 1f;
        }
        
        private void Update()
        {
            if (Time.time >= nextUpdate)
            {
                UpdateVitals();
                nextUpdate = Time.time + updateInterval;
            }
            
            UpdateTimer();
        }
        
        private void UpdateVitals()
        {
            // Placeholder - these would be connected to patient/npc systems in Phase 2
            if (healthText != null)
                healthText.text = "♥ 100";
            
            if (heartRateText != null)
                heartRateText.text = "❤ 72 BPM";
            
            if (bloodPressureText != null)
                bloodPressureText.text = "BP 120/80";
            
            if (oxygenText != null)
                oxygenText.text = "O₂ 98%";
        }
        
        private void UpdateTimer()
        {
            if (timerText != null)
            {
                timer += Time.deltaTime;
                int minutes = Mathf.FloorToInt(timer / 60f);
                int seconds = Mathf.FloorToInt(timer % 60f);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }
        
        public void UpdateObjective(string objective)
        {
            currentObjective = objective;
            if (objectiveText != null)
                objectiveText.text = objective;
        }
        
        public void AddScore(int points)
        {
            currentScore += points;
            if (scoreText != null)
                scoreText.text = $"Score: {currentScore}";
        }
        
        public void ResetTimer()
        {
            timer = 0f;
        }
        
        public void ShowHUD()
        {
            if (hudCanvas != null)
                hudCanvas.alpha = 1f;
        }
        
        public void HideHUD()
        {
            if (hudCanvas != null)
                hudCanvas.alpha = 0f;
        }
    }
}
