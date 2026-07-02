using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HospitalRescue.Application.GameManagers;

namespace HospitalRescue.Application.Presentation.UI.Components
{
    /// <summary>
    /// Mission UI showing objectives, progress, and call information
    /// </summary>
    public class MissionUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup missionCanvas;
        [SerializeField] private TextMeshProUGUI missionTitleText;
        [SerializeField] private TextMeshProUGUI objectiveText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Image missionProgressBar;
        [SerializeField] private TextMeshProUGUI scoreText;
        
        [Header("Emergency Call UI")]
        [SerializeField] private CanvasGroup callCanvas;
        [SerializeField] private Image callRingAnimation;
        [SerializeField] private TextMeshProUGUI callLocationText;
        [SerializeField] private TextMeshProUGUI callDescriptionText;
        [SerializeField] private TextMeshProUGUI severityText;
        [SerializeField] private Button acceptCallButton;
        [SerializeField] private Button dismissCallButton;
        
        [Header("Vital Monitor")]
        [SerializeField] private TextMeshProUGUI heartRateText;
        [SerializeField] private TextMeshProUGUI bloodPressureText;
        [SerializeField] private TextMeshProUGUI oxygenText;
        [SerializeField] private Image heartRateBar;
        
        private MissionManager missionManager;
        private EmergencyCallSystem callSystem;
        private ScoringSystem scoringSystem;
        
        private void Awake()
        {
            missionManager = MissionManager.Instance;
            callSystem = EmergencyCallSystem.Instance;
            scoringSystem = ScoringSystem.Instance;
            
            SetupCallbacks();
        }
        
        private void Start()
        {
            if (missionCanvas != null)
                missionCanvas.alpha = 0f;
            
            if (callCanvas != null)
                callCanvas.alpha = 0f;
        }
        
        private void SetupCallbacks()
        {
            if (missionManager != null)
            {
                missionManager.OnMissionStarted += ShowMission;
                missionManager.OnMissionCompleted += OnMissionCompleted;
                missionManager.OnObjectiveStarted += UpdateObjective;
                missionManager.OnMissionProgress += UpdateProgress;
            }
            
            if (callSystem != null)
            {
                callSystem.OnEmergencyReceived += ShowIncomingCall;
                callSystem.OnRingProgress += UpdateCallRing;
            }
            
            if (acceptCallButton != null)
                acceptCallButton.onClick.AddListener(OnAcceptCall);
            
            if (dismissCallButton != null)
                dismissCallButton.onClick.AddListener(OnDismissCall);
        }
        
        private void Update()
        {
            UpdateMissionTimer();
            UpdateVitals();
            UpdateScore();
        }
        
        private void UpdateMissionTimer()
        {
            if (missionManager == null || !missionManager.IsMissionActive) return;
            
            if (timerText != null && missionManager.CurrentMission != null)
            {
                float remaining = missionManager.CurrentMission.timeLimit - missionManager.MissionTime;
                if (remaining < 0) remaining = 0;
                
                int minutes = Mathf.FloorToInt(remaining / 60f);
                int seconds = Mathf.FloorToInt(remaining % 60f);
                timerText.text = $"{minutes:00}:{seconds:00}";
                
                // Color based on time remaining
                if (remaining < 30f)
                    timerText.color = Color.red;
                else if (remaining < 60f)
                    timerText.color = Color.yellow;
                else
                    timerText.color = Color.white;
            }
        }
        
        private void UpdateVitals()
        {
            // This would be connected to the current patient being treated
            // Placeholder for now
        }
        
        private void UpdateScore()
        {
            if (scoringSystem != null && scoreText != null)
            {
                scoreText.text = $"Score: {scoringSystem.CurrentScore}";
            }
        }
        
        private void UpdateProgress(float progress)
        {
            if (missionProgressBar != null)
            {
                missionProgressBar.fillAmount = progress;
            }
        }
        
        private void UpdateObjective(Objective objective)
        {
            if (objectiveText != null)
            {
                objectiveText.text = $"► {objective.description}";
            }
        }
        
        private void ShowMission(Mission mission)
        {
            if (missionCanvas != null)
            {
                missionCanvas.alpha = 1f;
            }
            
            if (missionTitleText != null)
            {
                missionTitleText.text = mission.missionName;
            }
        }
        
        private void OnMissionCompleted(Mission mission)
        {
            if (missionCanvas != null)
            {
                // Show completion for a moment then fade
                Invoke(nameof(HideMission), 3f);
            }
        }
        
        private void HideMission()
        {
            if (missionCanvas != null)
            {
                missionCanvas.alpha = 0f;
            }
        }
        
        private void ShowIncomingCall(EmergencyCall call)
        {
            if (callCanvas != null)
            {
                callCanvas.alpha = 1f;
            }
            
            if (callLocationText != null)
            {
                callLocationText.text = call.location;
            }
            
            if (callDescriptionText != null)
            {
                callDescriptionText.text = call.description;
            }
            
            if (severityText != null)
            {
                severityText.text = $"Severity: {call.GetSeverityString()}";
                severityText.color = call.severity >= 4 ? Color.red : (call.severity >= 3 ? Color.yellow : Color.green);
            }
        }
        
        private void UpdateCallRing(float progress)
        {
            if (callRingAnimation != null)
            {
                callRingAnimation.fillAmount = progress;
            }
        }
        
        private void OnAcceptCall()
        {
            callSystem?.AcceptCall();
            
            if (callCanvas != null)
            {
                callCanvas.alpha = 0f;
            }
            
            // Start the mission
            if (missionManager != null)
            {
                missionManager.StartMission(1);
            }
        }
        
        private void OnDismissCall()
        {
            callSystem?.DismissCall();
            
            if (callCanvas != null)
            {
                callCanvas.alpha = 0f;
            }
        }
        
        private void OnDestroy()
        {
            if (missionManager != null)
            {
                missionManager.OnMissionStarted -= ShowMission;
                missionManager.OnMissionCompleted -= OnMissionCompleted;
                missionManager.OnObjectiveStarted -= UpdateObjective;
                missionManager.OnMissionProgress -= UpdateProgress;
            }
            
            if (callSystem != null)
            {
                callSystem.OnEmergencyReceived -= ShowIncomingCall;
                callSystem.OnRingProgress -= UpdateCallRing;
            }
        }
    }
}
