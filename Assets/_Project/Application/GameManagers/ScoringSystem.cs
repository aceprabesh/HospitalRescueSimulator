using UnityEngine;
using System;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// Handles scoring, XP, and progression
    /// </summary>
    public class ScoringSystem : MonoBehaviour
    {
        public static ScoringSystem Instance { get; private set; }
        
        [Header("Score Settings")]
        [SerializeField] private int baseScore = 0;
        [SerializeField] private int totalXP = 0;
        [SerializeField] private int level = 1;
        [SerializeField] private int xpToNextLevel = 1000;
        
        [Header("Score Multipliers")]
        [SerializeField] private float timeBonusMultiplier = 1.5f;
        [SerializeField] private float perfectBonusMultiplier = 2.0f;
        [SerializeField] private float noDeathBonus = 500;
        
        private int currentScore;
        private int sessionScore;
        private int patientsSaved;
        private int patientsLost;
        private int perfectMissions;
        private float averageResponseTime;
        
        // Events
        public event Action<int> OnScoreChanged;
        public event Action<int> OnXPChanged;
        public event Action<int> OnLevelUp;
        public event Action<Achievement> OnAchievementUnlocked;
        
        public int CurrentScore => currentScore;
        public int TotalXP => totalXP;
        public int Level => level;
        public int PatientsSaved => patientsSaved;
        public int PatientsLost => patientsLost;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public void AddScore(int points, string reason = "")
        {
            currentScore += points;
            sessionScore += points;
            
            Debug.Log($"[SCORE] +{points} ({reason}) - Total: {currentScore}");
            OnScoreChanged?.Invoke(currentScore);
        }
        
        public void AddXP(int xp, string source = "")
        {
            totalXP += xp;
            
            // Check for level up
            while (totalXP >= xpToNextLevel)
            {
                totalXP -= xpToNextLevel;
                level++;
                xpToNextLevel = CalculateXPForLevel(level + 1);
                
                Debug.Log($"[LEVEL UP] You are now level {level}!");
                OnLevelUp?.Invoke(level);
            }
            
            Debug.Log($"[XP] +{xp} {source} - Total XP: {totalXP}/{xpToNextLevel}");
            OnXPChanged?.Invoke(totalXP);
        }
        
        public void MissionCompleted(int missionScore, int timeBonus, int perfectBonus, bool patientSaved, float responseTime)
        {
            int total = missionScore + timeBonus + perfectBonus;
            
            if (patientSaved)
            {
                patientsSaved++;
                total += (int)noDeathBonus;
            }
            else
            {
                patientsLost++;
            }
            
            if (perfectBonus > 0)
            {
                perfectMissions++;
            }
            
            AddScore(total, "Mission Complete");
            AddXP(missionScore / 10, "Mission Completion");
            
            // Award bonus XP for speed
            if (responseTime < 60f)
            {
                AddXP(50, "Fast Response Bonus");
            }
            
            Debug.Log($"[MISSION SCORE] Base: {missionScore} | Time: +{timeBonus} | Perfect: +{perfectBonus} | Total: {total}");
        }
        
        public void SavePatient()
        {
            patientsSaved++;
            AddScore(100, "Patient Saved");
            AddXP(100, "Life Saved");
        }
        
        public void LosePatient()
        {
            patientsLost++;
        }
        
        private int CalculateXPForLevel(int lvl)
        {
            // Exponential scaling: 1000, 1500, 2250, 3375...
            return Mathf.RoundToInt(1000f * Mathf.Pow(1.5f, lvl - 1));
        }
        
        public string GetStatsReport()
        {
            return $@"
=== RESCUE STATISTICS ===
Level: {level}
Total XP: {totalXP}/{xpToNextLevel}
---
Score: {currentScore}
Session Score: {sessionScore}
---
Patients Saved: {patientsSaved}
Patients Lost: {patientsLost}
Save Rate: {(patientsSaved + patientsLost > 0 ? (float)patientsSaved / (patientsSaved + patientsLost) * 100 : 0):F1}%
Perfect Missions: {perfectMissions}
";
        }
        
        public void ResetSession()
        {
            sessionScore = 0;
        }
    }
}
