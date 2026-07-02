using System.Collections.Generic;
using UnityEngine;
using HospitalRescue.Application.Controllers;
using HospitalRescue.Domain.Entities;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// Manages missions, objectives, and progression
    /// </summary>
    public class MissionManager : MonoBehaviour
    {
        public static MissionManager Instance { get; private set; }
        
        [Header("Mission Settings")]
        [SerializeField] private List<Mission> availableMissions = new List<Mission>();
        [SerializeField] private bool autoStartNextMission = false;
        
        private Mission currentMission;
        private List<Objective> completedObjectives = new List<Objective>();
        private int currentObjectiveIndex;
        private float missionTime;
        private bool isMissionActive;
        
        // Events
        public System.Action<Mission> OnMissionStarted;
        public System.Action<Mission> OnMissionCompleted;
        public System.Action<Objective> OnObjectiveStarted;
        public System.Action<Objective> OnObjectiveCompleted;
        public System.Action<string> OnMissionFailed;
        public System.Action<float> OnMissionProgress;
        
        public Mission CurrentMission => currentMission;
        public bool IsMissionActive => isMissionActive;
        public float MissionTime => missionTime;
        public List<Objective> CompletedObjectives => completedObjectives;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        
        private void Update()
        {
            if (isMissionActive && currentMission != null)
            {
                missionTime += Time.deltaTime;
                UpdateObjectiveProgress();
                
                // Check for time limit
                if (currentMission.timeLimit > 0 && missionTime >= currentMission.timeLimit)
                {
                    FailMission("Time ran out!");
                }
            }
        }
        
        public void StartMission(int missionId)
        {
            Mission mission = availableMissions.Find(m => m.missionId == missionId);
            if (mission != null)
            {
                StartMission(mission);
            }
            else
            {
                Debug.LogError($"Mission {missionId} not found!");
            }
        }
        
        public void StartMission(Mission mission)
        {
            if (isMissionActive)
            {
                Debug.LogWarning("Mission already in progress!");
                return;
            }
            
            currentMission = mission;
            completedObjectives.Clear();
            currentObjectiveIndex = 0;
            missionTime = 0f;
            isMissionActive = true;
            
            // Subscribe to patient events
            SubscribeToPatientEvents();
            
            // Spawn patients for this mission
            SpawnMissionPatients();
            
            OnMissionStarted?.Invoke(mission);
            Debug.Log($"[MISSION] Started: {mission.missionName}");
            
            // Start first objective
            if (mission.objectives.Count > 0)
            {
                StartObjective(0);
            }
            
            // Send emergency call if configured
            if (mission.sendsEmergencyCall)
            {
                EmergencyCallSystem.Instance.ReceiveEmergencyCall(
                    mission.emergencyLocation,
                    mission.emergencyDescription,
                    mission.severity,
                    null
                );
            }
        }
        
        public void CompleteMission()
        {
            if (currentMission == null || !isMissionActive) return;
            
            // Calculate score
            int score = CalculateMissionScore();
            
            isMissionActive = false;
            currentMission.completed = true;
            currentMission.completionTime = missionTime;
            currentMission.scoreEarned = score;
            
            UnsubscribeFromPatientEvents();
            
            OnMissionCompleted?.Invoke(currentMission);
            Debug.Log($"[MISSION] Completed: {currentMission.missionName} | Score: {score}");
            
            // Award XP and achievements
            AwardRewards(score);
            
            currentMission = null;
        }
        
        public void FailMission(string reason)
        {
            if (currentMission == null) return;
            
            isMissionActive = false;
            currentMission.completed = true;
            currentMission.success = false;
            
            UnsubscribeFromPatientEvents();
            
            OnMissionFailed?.Invoke(reason);
            Debug.Log($"[MISSION] Failed: {currentMission.missionName} | Reason: {reason}");
            
            currentMission = null;
        }
        
        private void StartObjective(int index)
        {
            if (currentMission == null || index >= currentMission.objectives.Count) return;
            
            currentObjectiveIndex = index;
            Objective objective = currentMission.objectives[index];
            objective.isActive = true;
            
            OnObjectiveStarted?.Invoke(objective);
            Debug.Log($"[OBJECTIVE] Started: {objective.description}");
        }
        
        public void CompleteObjective(int index)
        {
            if (currentMission == null || index >= currentMission.objectives.Count) return;
            
            Objective objective = currentMission.objectives[index];
            objective.isCompleted = true;
            completedObjectives.Add(objective);
            
            OnObjectiveCompleted?.Invoke(objective);
            Debug.Log($"[OBJECTIVE] Completed: {objective.description}");
            
            // Start next objective or complete mission
            if (index + 1 < currentMission.objectives.Count)
            {
                StartObjective(index + 1);
            }
            else
            {
                CompleteMission();
            }
        }
        
        public void CompleteObjective(string objectiveId)
        {
            if (currentMission == null) return;
            
            int index = currentMission.objectives.FindIndex(o => o.objectiveId == objectiveId);
            if (index >= 0)
            {
                CompleteObjective(index);
            }
        }
        
        private void UpdateObjectiveProgress()
        {
            if (currentMission == null || currentObjectiveIndex >= currentMission.objectives.Count) return;
            
            Objective current = currentMission.objectives[currentObjectiveIndex];
            
            if (current.isProgressBased && current.targetCount > 0)
            {
                float progress = (float)current.currentCount / current.targetCount;
                OnMissionProgress?.Invoke(progress);
            }
        }
        
        public void IncrementObjectiveProgress(string objectiveId, int amount = 1)
        {
            if (currentMission == null) return;
            
            Objective objective = currentMission.objectives.Find(o => o.objectiveId == objectiveId);
            if (objective != null && objective.isActive)
            {
                objective.currentCount += amount;
                
                if (objective.isProgressBased && objective.currentCount >= objective.targetCount)
                {
                    CompleteObjective(objective.objectiveId);
                }
            }
        }
        
        private void SubscribeToPatientEvents()
        {
            // Subscribe to patient treatment events
            PatientController[] patients = Object.FindObjectsOfType<PatientController>();
            foreach (var patient in patients)
            {
                patient.OnPatientDeceased += OnPatientDeceased;
                patient.OnPatientTreated += OnPatientTreated;
            }
        }
        
        private void UnsubscribeFromPatientEvents()
        {
            PatientController[] patients = Object.FindObjectsOfType<PatientController>();
            foreach (var patient in patients)
            {
                patient.OnPatientDeceased -= OnPatientDeceased;
                patient.OnPatientTreated -= OnPatientTreated;
            }
        }
        
        private void OnPatientDeceased(PatientData patient)
        {
            // Check if mission requires patient survival
            if (currentMission != null && currentMission.requirePatientSurvival)
            {
                FailMission($"Patient {patient.patientName} died!");
            }
        }
        
        private void OnPatientTreated(PatientData patient)
        {
            // Track treatment objectives
            IncrementObjectiveProgress("treat_patient");
        }
        
        private void SpawnMissionPatients()
        {
            if (currentMission == null) return;
            
            foreach (var spawn in currentMission.patientSpawns)
            {
                Debug.Log($"[SPAWN] Would spawn patient at {spawn.location}");
                // Patient spawning handled by PatientManager in full implementation
            }
        }
        
        private int CalculateMissionScore()
        {
            if (currentMission == null) return 0;
            
            int baseScore = currentMission.baseScore;
            int timeBonus = currentMission.timeLimit > 0 
                ? Mathf.RoundToInt((1f - missionTime / currentMission.timeLimit) * currentMission.timeBonus) 
                : 0;
            int objectiveBonus = completedObjectives.Count * 100;
            int survivalBonus = currentMission.requirePatientSurvival ? 500 : 0;
            
            return baseScore + timeBonus + objectiveBonus + survivalBonus;
        }
        
        private void AwardRewards(int score)
        {
            // Add to total XP
            // Unlock achievements
            // Save progress
            Debug.Log($"[REWARDS] Score: {score}");
        }
        
        public Objective GetCurrentObjective()
        {
            if (currentMission != null && currentObjectiveIndex < currentMission.objectives.Count)
            {
                return currentMission.objectives[currentObjectiveIndex];
            }
            return null;
        }
        
        public string GetMissionStatus()
        {
            if (currentMission == null) return "No active mission";
            
            float progress = 0f;
            if (currentMission.objectives.Count > 0)
            {
                progress = (float)completedObjectives.Count / currentMission.objectives.Count;
            }
            
            return $"Mission: {currentMission.missionName}\n" +
                   $"Progress: {completedObjectives.Count}/{currentMission.objectives.Count} ({progress * 100:F0}%)\n" +
                   $"Time: {missionTime:F1}s";
        }
    }
    
    [System.Serializable]
    public class Mission
    {
        public int missionId;
        public string missionName;
        public string description;
        public List<Objective> objectives = new List<Objective>();
        
        [Header("Settings")]
        public int baseScore = 1000;
        public int timeBonus = 500;
        public float timeLimit = 300f; // 0 = no limit
        public bool sendsEmergencyCall = true;
        public string emergencyLocation = "Emergency Room";
        public string emergencyDescription = "Cardiac arrest";
        public int severity = 3;
        public bool requirePatientSurvival = true;
        
        [Header("Spawns")]
        public List<PatientSpawn> patientSpawns = new List<PatientSpawn>();
        
        // Runtime
        public bool completed;
        public bool success;
        public float completionTime;
        public int scoreEarned;
    }
    
    [System.Serializable]
    public class Objective
    {
        public string objectiveId;
        public string description;
        public bool isCompleted;
        public bool isActive;
        
        // Progress tracking
        public bool isProgressBased;
        public int currentCount;
        public int targetCount;
        
        // Optional requirements
        public string requiredItem;
        public string requiredAction;
    }
    
    [System.Serializable]
    public class PatientSpawn
    {
        public string location;
        public Vector3 position;
        public string patientType;
        public int initialSeverity;
    }
}
