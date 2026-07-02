using UnityEngine;
using System.Collections.Generic;
using HospitalRescue.Application.Controllers;
using HospitalRescue.Domain.Entities;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// Manages patient spawning and lifecycle
    /// </summary>
    public class PatientManager : MonoBehaviour
    {
        public static PatientManager Instance { get; private set; }
        
        [Header("Patient Prefabs")]
        [SerializeField] private GameObject[] patientPrefabs;
        
        [Header("Spawn Points")]
        [SerializeField] private Transform[] spawnPoints;
        
        private List<PatientController> activePatients = new List<PatientController>();
        private int patientCounter;
        
        public List<PatientController> ActivePatients => activePatients;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        
        public PatientController SpawnPatient(Vector3 position, PatientData data)
        {
            patientCounter++;
            
            GameObject patientObj = new GameObject($"Patient_{patientCounter}");
            patientObj.transform.position = position;
            patientObj.tag = "Patient";
            
            PatientController controller = patientObj.AddComponent<PatientController>();
            controller.InitializePatient(data.patientName, data.age, data.condition);
            
            // Apply provided data
            var patientData = controller.Data;
            patientData.heartRate = data.heartRate;
            patientData.bloodPressure = data.bloodPressure;
            patientData.oxygenSaturation = data.oxygenSaturation;
            patientData.temperature = data.temperature;
            patientData.respiratoryRate = data.respiratoryRate;
            patientData.currentState = data.currentState;
            
            activePatients.Add(controller);
            
            Debug.Log($"[PATIENT] Spawned {data.patientName} at {position}");
            
            return controller;
        }
        
        public PatientController SpawnRandomPatient(Vector3 position)
        {
            PatientData data = new PatientData
            {
                patientName = GenerateRandomName(),
                age = Random.Range(18, 85),
                condition = GetRandomCondition(),
                currentState = PatientState.Critical
            };
            
            return SpawnPatient(position, data);
        }
        
        public void RemovePatient(PatientController patient)
        {
            if (activePatients.Contains(patient))
            {
                activePatients.Remove(patient);
                Destroy(patient.gameObject);
            }
        }
        
        public void ClearAllPatients()
        {
            foreach (var patient in activePatients)
            {
                if (patient != null)
                    Destroy(patient.gameObject);
            }
            activePatients.Clear();
        }
        
        private string GenerateRandomName()
        {
            string[] firstNames = { "John", "Mary", "Robert", "Patricia", "Michael", "Jennifer", "William", "Linda", "David", "Elizabeth" };
            string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
            
            return $"{firstNames[Random.Range(0, firstNames.Length)]} {lastNames[Random.Range(0, lastNames.Length)]}";
        }
        
        private string GetRandomCondition()
        {
            string[] conditions = { "Chest Pain", "Difficulty Breathing", "Fall Injury", "Allergic Reaction", "Stroke Symptoms", "Diabetic Emergency" };
            return conditions[Random.Range(0, conditions.Length)];
        }
        
        public PatientController GetNearestPatient(Vector3 position, float maxDistance = 10f)
        {
            PatientController nearest = null;
            float nearestDist = maxDistance;
            
            foreach (var patient in activePatients)
            {
                float dist = Vector3.Distance(position, patient.transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = patient;
                }
            }
            
            return nearest;
        }
    }
}
