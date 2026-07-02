using UnityEngine;
using HospitalRescue.Domain.Entities;
using HospitalRescue.Domain.Interfaces;

namespace HospitalRescue.Application.Controllers
{
    /// <summary>
    /// Controls patient NPC with vitals, states, and treatment interactions
    /// </summary>
    public class PatientController : MonoBehaviour, IInteractable
    {
        [Header("Patient Data")]
        [SerializeField] private PatientData patientData;
        
        [Header("Vital Thresholds")]
        [SerializeField] private float criticalHeartRateMin = 40f;
        [SerializeField] private float criticalHeartRateMax = 150f;
        [SerializeField] private float criticalOxygenMin = 90f;
        
        [Header("Visual Feedback")]
        [SerializeField] private Renderer bodyRenderer;
        [SerializeField] private Color stableColor = Color.green;
        [SerializeField] private Color criticalColor = Color.red;
        [SerializeField] private Color unconsciousColor = Color.gray;
        
        [Header("AI Behavior")]
        [SerializeField] private bool simulateVitalsDeterioration = true;
        [SerializeField] private float deteriorationRate = 1f;
        
        // IInteractable
        public string InteractionPrompt => patientData.isResponsive ? "Press E to examine patient" : "Press E to check vitals";
        public float InteractionTime => 1f;
        public bool HoldToInteract => true;
        
        private bool isBeingExamined;
        private float examinationProgress;
        
        public PatientData Data => patientData;
        public PatientState CurrentState => patientData.currentState;
        
        // Events
        public System.Action<PatientData> OnPatientStateChanged;
        public System.Action<PatientData> OnPatientTreated;
        public System.Action<PatientData> OnPatientDeceased;
        
        private void Awake()
        {
            if (patientData == null)
            {
                patientData = new PatientData();
            }
            
            if (bodyRenderer == null)
            {
                bodyRenderer = GetComponentInChildren<Renderer>();
            }
        }
        
        private void Update()
        {
            if (simulateVitalsDeterioration && patientData.currentState != PatientState.Deceased)
            {
                SimulateVitalChanges();
            }
            
            UpdateVisuals();
        }
        
        private void SimulateVitalChanges()
        {
            float delta = Time.deltaTime * deteriorationRate * 0.1f;
            
            switch (patientData.currentState)
            {
                case PatientState.Critical:
                    patientData.heartRate.value = Mathf.Lerp(patientData.heartRate.value, 
                        patientData.heartRate.value < 60 ? 20f : 200f, delta);
                    patientData.oxygenSaturation.value = Mathf.Max(0, patientData.oxygenSaturation.value - delta * 2f);
                    patientData.consciousnessLevel = Mathf.Max(0, patientData.consciousnessLevel - delta * 5f);
                    break;
                    
                case PatientState.CardiacArrest:
                    patientData.heartRate.value = Mathf.Max(0, patientData.heartRate.value - delta * 10f);
                    patientData.oxygenSaturation.value = Mathf.Max(0, patientData.oxygenSaturation.value - delta * 5f);
                    patientData.consciousnessLevel = Mathf.Max(0, patientData.consciousnessLevel - delta * 10f);
                    break;
            }
            
            patientData.UpdateState();
            
            if (patientData.currentState == PatientState.Deceased)
            {
                OnPatientDeceased?.Invoke(patientData);
            }
        }
        
        private void UpdateVisuals()
        {
            if (bodyRenderer == null) return;
            
            Color targetColor = patientData.currentState switch
            {
                PatientState.Stable => stableColor,
                PatientState.Critical => criticalColor,
                PatientState.Unconscious => unconsciousColor,
                PatientState.CardiacArrest => criticalColor * 0.7f,
                PatientState.Deceased => Color.white * 0.3f,
                _ => stableColor
            };
            
            bodyRenderer.material.color = Color.Lerp(bodyRenderer.material.color, targetColor, Time.deltaTime * 2f);
        }
        
        public bool CanInteract()
        {
            return patientData.currentState != PatientState.Deceased;
        }
        
        public void OnInteract()
        {
            Debug.Log($"Examining patient: {patientData.patientName}");
            OnPatientStateChanged?.Invoke(patientData);
        }
        
        public void OnInteractStart()
        {
            isBeingExamined = true;
            examinationProgress = 0f;
        }
        
        public void OnInteractEnd()
        {
            isBeingExamined = false;
            examinationProgress = 0f;
        }
        
        public void ApplyTreatment(string treatmentType, float effectiveness)
        {
            patientData.ApplyTreatment(treatmentType, effectiveness);
            OnPatientTreated?.Invoke(patientData);
            Debug.Log($"Applied {treatmentType} to {patientData.patientName}. Effectiveness: {effectiveness * 100}%");
        }
        
        public string GetVitalReport()
        {
            return $@"
=== PATIENT VITALS ===
Name: {patientData.patientName}
Condition: {patientData.condition}
State: {patientData.currentState}
---
Heart Rate: {patientData.heartRate.GetDisplayString()}
Blood Pressure: {patientData.bloodPressure.GetDisplayString()}
O₂ Saturation: {patientData.oxygenSaturation.GetDisplayString()}
Temperature: {patientData.temperature.GetDisplayString()}
Respiratory Rate: {patientData.respiratoryRate.GetDisplayString()}
---
Consciousness: {patientData.consciousnessLevel:F0}%
Pain Level: {patientData.painLevel:F0}%
Blood Loss: {patientData.bloodLoss:F0}%
---
Blood Type: {patientData.bloodType}
Allergies: {(patientData.hasAllergies ? patientData.allergiesList : "NKDA")}
";
        }
        
        public void InitializePatient(string name, int age, string condition)
        {
            patientData = new PatientData
            {
                patientName = name,
                age = age,
                condition = condition
            };
        }
    }
}
