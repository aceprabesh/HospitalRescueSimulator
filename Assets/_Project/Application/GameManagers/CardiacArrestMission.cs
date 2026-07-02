using UnityEngine;
using HospitalRescue.Application.Controllers;
using HospitalRescue.Domain.Entities;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// First mission: Cardiac Arrest in Waiting Room
    /// </summary>
    [CreateAssetMenu(fileName = "CardiacArrestMission", menuName = "HospitalRescue/Missions/CardiacArrest")]
    public class CardiacArrestMission : MissionScenario
    {
        [Header("Mission Specific")]
        [SerializeField] private int requiredCPRCycles = 5;
        [SerializeField] private int requiredDefibrillatorShocks = 3;
        
        private void Awake()
        {
            missionId = 1;
            missionName = "Code Blue: Waiting Room";
            description = "An elderly patient has collapsed in the waiting room. They are in cardiac arrest. " +
                        "You must perform CPR, use the defibrillator, and stabilize the patient before it's too late.";
            location = "Emergency Waiting Room A";
            severity = 5;
            baseScore = 2000;
            timeBonus = 1000;
            timeLimit = 180f; // 3 minutes
            requirePatientSurvival = true;
            
            patientName = "Harold Mitchell";
            patientAge = 67;
            patientCondition = "Cardiac Arrest - V-Fib";
            initialPatientState = PatientState.CardiacArrest;
            heartRate = 0; // Flatline initially
            bloodPressure = 0;
            oxygenSaturation = 72;
            
            xpReward = 500;
            unlockAchievements = new[] { "ACH_FIRST_LIFE_SAVED" };
            
            objectives = new ObjectiveData[]
            {
                new ObjectiveData { objectiveId = "reach_patient", description = "Reach the patient in Waiting Room A" },
                new ObjectiveData { objectiveId = "assess_vitals", description = "Assess patient vitals" },
                new ObjectiveData { objectiveId = "start_cpr", description = "Begin CPR compressions" },
                new ObjectiveData { objectiveId = "apply_defibrillator", description = "Apply defibrillator and shock" },
                new ObjectiveData { objectiveId = "administer_oxygen", description = "Administer oxygen therapy" },
                new ObjectiveData { objectiveId = "stabilize_patient", description = "Stabilize patient vitals" },
                new ObjectiveData { objectiveId = "prepare_transport", description = "Prepare patient for transport to ICU" }
            };
        }
    }
}
