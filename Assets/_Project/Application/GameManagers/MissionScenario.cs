using UnityEngine;
using HospitalRescue.Application.Controllers;
using HospitalRescue.Domain.Entities;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// Mission scenarios - defines all playable missions
    /// </summary>
    [CreateAssetMenu(fileName = "MissionScenario", menuName = "HospitalRescue/Missions/Scenario")]
    public class MissionScenario : ScriptableObject
    {
        [Header("Mission Info")]
        public int missionId;
        public string missionName;
        public string description;
        public string location;
        public int severity; // 1-5
        public Sprite missionImage;
        
        [Header("Objectives")]
        public ObjectiveData[] objectives;
        
        [Header("Settings")]
        public int baseScore = 1000;
        public int timeBonus = 500;
        public float timeLimit = 300f;
        public bool requirePatientSurvival = true;
        
        [Header("Patient Setup")]
        public string patientName;
        public int patientAge;
        public string patientCondition;
        public PatientState initialPatientState;
        public float heartRate;
        public float bloodPressure;
        public float oxygenSaturation;
        
        [Header("Rewards")]
        public int xpReward;
        public string[] unlockAchievements;
        
        public Mission ToMission()
        {
            Mission mission = new Mission
            {
                missionId = missionId,
                missionName = missionName,
                description = description,
                baseScore = baseScore,
                timeBonus = timeBonus,
                timeLimit = timeLimit,
                requirePatientSurvival = requirePatientSurvival,
                sendsEmergencyCall = true,
                emergencyLocation = location,
                emergencyDescription = patientCondition,
                severity = severity
            };
            
            foreach (var obj in objectives)
            {
                mission.objectives.Add(new Objective
                {
                    objectiveId = obj.objectiveId,
                    description = obj.description,
                    isProgressBased = obj.isProgressBased,
                    targetCount = obj.targetCount,
                    requiredAction = obj.requiredAction
                });
            }
            
            return mission;
        }
        
        public PatientData CreatePatientData()
        {
            PatientData data = new PatientData
            {
                patientName = patientName,
                age = patientAge,
                condition = patientCondition,
                currentState = initialPatientState
            };
            
            data.heartRate = new VitalReading(VitalType.HeartRate, heartRate, 40, 150);
            data.bloodPressure = new VitalReading(VitalType.BloodPressure, bloodPressure, 80, 180);
            data.oxygenSaturation = new VitalReading(VitalType.OxygenSaturation, oxygenSaturation, 85, 100);
            
            return data;
        }
    }
    
    [System.Serializable]
    public class ObjectiveData
    {
        public string objectiveId;
        public string description;
        public bool isProgressBased;
        public int targetCount;
        public string requiredAction;
    }
}
