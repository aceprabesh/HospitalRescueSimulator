using UnityEngine;
using System;
using System.Collections.Generic;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// Manages achievements and unlockables
    /// </summary>
    public class AchievementSystem : MonoBehaviour
    {
        public static AchievementSystem Instance { get; private set; }
        
        [Header("Achievements")]
        [SerializeField] private List<Achievement> achievements = new List<Achievement>();
        
        private HashSet<string> unlockedAchievements = new HashSet<string>();
        
        // Events
        public event Action<Achievement> OnAchievementUnlocked;
        
        public IReadOnlyList<Achievement> Achievements => achievements;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            InitializeAchievements();
        }
        
        private void InitializeAchievements()
        {
            achievements = new List<Achievement>
            {
                new Achievement { id = "ACH_FIRST_LIFE_SAVED", name = "First Response", description = "Save your first patient", icon = null },
                new Achievement { id = "ACH_QUICK_STARTER", name = "Quick Response", description = "Complete a mission in under 60 seconds", icon = null },
                new Achievement { id = "ACH_CODE_BLUE_MASTER", name = "Code Blue Master", description = "Successfully resuscitate a cardiac arrest patient", icon = null },
                new Achievement { id = "ACH_PERFECT_CPR", name = "Perfect Compressions", description = "Complete CPR with 100% rhythm accuracy", icon = null },
                new Achievement { id = "ACH_LIVES_5", name = "Saving Lives", description = "Save 5 patients", icon = null },
                new Achievement { id = "ACH_LIVES_25", name = "Dedicated Medic", description = "Save 25 patients", icon = null },
                new Achievement { id = "ACH_LIVES_100", name = "Master Medic", description = "Save 100 patients", icon = null },
                new Achievement { id = "ACH_NO_DEATHS_10", name = "Flawless Record", description = "Complete 10 missions without losing a patient", icon = null },
                new Achievement { id = "ACH_ALL_MISSIONS", name = "Hospital Hero", description = "Complete all missions", icon = null },
                new Achievement { id = "ACH_SPEED_DEMON", name = "Speed Demon", description = "Complete any mission with 2 minutes remaining", icon = null },
                new Achievement { id = "ACH_TEAM_PLAYER", name = "Team Player", description = "Use all medical equipment types in one mission", icon = null },
                new Achievement { id = "ACH_LEVEL_5", name = "Rising Star", description = "Reach level 5", icon = null },
                new Achievement { id = "ACH_LEVEL_10", name = "Experienced Medic", description = "Reach level 10", icon = null }
            };
        }
        
        public void UnlockAchievement(string achievementId)
        {
            if (unlockedAchievements.Contains(achievementId))
                return;
            
            Achievement achievement = achievements.Find(a => a.id == achievementId);
            if (achievement == null)
            {
                Debug.LogWarning($"[ACHIEVEMENT] Unknown achievement: {achievementId}");
                return;
            }
            
            achievement.unlocked = true;
            achievement.unlockTime = DateTime.Now;
            unlockedAchievements.Add(achievementId);
            
            Debug.Log($"[ACHIEVEMENT UNLOCKED] {achievement.name}: {achievement.description}");
            OnAchievementUnlocked?.Invoke(achievement);
            
            // Award bonus score for achievement
            ScoringSystem.Instance?.AddScore(500, $"Achievement: {achievement.name}");
        }
        
        public void CheckAchievement(string achievementId)
        {
            if (unlockedAchievements.Contains(achievementId))
                return;
            
            UnlockAchievement(achievementId);
        }
        
        public void CheckLivesSavedAchievements(int livesSaved)
        {
            if (livesSaved >= 1) CheckAchievement("ACH_FIRST_LIFE_SAVED");
            if (livesSaved >= 5) CheckAchievement("ACH_LIVES_5");
            if (livesSaved >= 25) CheckAchievement("ACH_LIVES_25");
            if (livesSaved >= 100) CheckAchievement("ACH_LIVES_100");
        }
        
        public void CheckLevelAchievements(int level)
        {
            if (level >= 5) CheckAchievement("ACH_LEVEL_5");
            if (level >= 10) CheckAchievement("ACH_LEVEL_10");
        }
        
        public void CheckMissionSpeedAchievement(float timeRemaining, float totalTime)
        {
            if (timeRemaining >= 120f) // 2 minutes left
                CheckAchievement("ACH_SPEED_DEMON");
        }
        
        public void CheckMissionTimeAchievement(float missionTime)
        {
            if (missionTime < 60f)
                CheckAchievement("ACH_QUICK_STARTER");
        }
        
        public bool IsUnlocked(string achievementId)
        {
            return unlockedAchievements.Contains(achievementId);
        }
        
        public List<Achievement> GetUnlockedAchievements()
        {
            return achievements.FindAll(a => a.unlocked);
        }
        
        public List<Achievement> GetLockedAchievements()
        {
            return achievements.FindAll(a => !a.unlocked);
        }
    }
    
    [System.Serializable]
    public class Achievement
    {
        public string id;
        public string name;
        public string description;
        public Sprite icon;
        public bool unlocked;
        public DateTime unlockTime;
    }
}
