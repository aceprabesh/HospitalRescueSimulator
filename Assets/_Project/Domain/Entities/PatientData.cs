using System;
using UnityEngine;

namespace HospitalRescue.Domain.Entities
{
    public enum PatientState
    {
        Stable,
        Critical,
        Unconscious,
        CardiacArrest,
        Deceased
    }
    
    public enum VitalType
    {
        HeartRate,
        BloodPressure,
        OxygenSaturation,
        Temperature,
        RespiratoryRate
    }
    
    [Serializable]
    public class VitalReading
    {
        public VitalType type;
        public float value;
        public float minNormal;
        public float maxNormal;
        public bool isCritical;
        
        public VitalReading(VitalType t, float val, float min, float max)
        {
            type = t;
            value = val;
            minNormal = min;
            maxNormal = max;
            isCritical = val < min || val > max;
        }
        
        public string GetDisplayString()
        {
            return type switch
            {
                VitalType.HeartRate => $"❤ {value:F0} BPM",
                VitalType.BloodPressure => $"BP {value:F0}",
                VitalType.OxygenSaturation => $"O₂ {value:F0}%",
                VitalType.Temperature => $"TEMP {value:F1}°F",
                VitalType.RespiratoryRate => $"RR {value:F0}/min",
                _ => $"{value}"
            };
        }
    }
    
    [Serializable]
    public class PatientData
    {
        public string patientId;
        public string patientName;
        public int age;
        public string condition;
        public PatientState currentState;
        
        public VitalReading heartRate;
        public VitalReading bloodPressure;
        public VitalReading oxygenSaturation;
        public VitalReading temperature;
        public VitalReading respiratoryRate;
        
        public float consciousnessLevel; // 0-100
        public float painLevel; // 0-100
        public float bloodLoss; // 0-100 percentage
        
        public bool isResponsive;
        public bool hasAllergies;
        public string allergiesList;
        public string bloodType;
        public string medications;
        
        public PatientData()
        {
            patientId = Guid.NewGuid().ToString();
            currentState = PatientState.Stable;
            consciousnessLevel = 100f;
            painLevel = 0f;
            bloodLoss = 0f;
            isResponsive = true;
            bloodType = "Unknown";
            
            // Normal vital ranges
            heartRate = new VitalReading(VitalType.HeartRate, 72, 60, 100);
            bloodPressure = new VitalReading(VitalType.BloodPressure, 120, 90, 140);
            oxygenSaturation = new VitalReading(VitalType.OxygenSaturation, 98, 95, 100);
            temperature = new VitalReading(VitalType.Temperature, 98.6f, 97f, 99.5f);
            respiratoryRate = new VitalReading(VitalType.RespiratoryRate, 16, 12, 20);
        }
        
        public void UpdateState()
        {
            // Determine patient state based on vitals
            if (heartRate.value <= 0 || oxygenSaturation.value <= 0)
            {
                currentState = PatientState.Deceased;
            }
            else if (heartRate.value < 30 || heartRate.value > 180)
            {
                currentState = PatientState.Critical;
            }
            else if (oxygenSaturation.value < 90)
            {
                currentState = PatientState.Critical;
            }
            else if (consciousnessLevel < 20)
            {
                currentState = PatientState.Unconscious;
            }
            else if (heartRate.value > 150 && respiratoryRate.value > 30)
            {
                currentState = PatientState.CardiacArrest;
            }
            else
            {
                currentState = PatientState.Stable;
            }
        }
        
        public void ApplyTreatment(string treatment, float effectiveness)
        {
            switch (treatment.ToLower())
            {
                case "cpr":
                    if (currentState == PatientState.CardiacArrest)
                    {
                        heartRate.value = Mathf.Lerp(heartRate.value, 60, effectiveness * 0.5f);
                        consciousnessLevel = Mathf.Lerp(consciousnessLevel, 30, effectiveness * 0.3f);
                    }
                    break;
                    
                case "defibrillator":
                    if (currentState == PatientState.CardiacArrest)
                    {
                        heartRate.value = Mathf.Lerp(heartRate.value, 80, effectiveness);
                    }
                    break;
                    
                case "oxygen":
                    oxygenSaturation.value = Mathf.Lerp(oxygenSaturation.value, 99, effectiveness * 0.8f);
                    break;
                    
                case "iv":
                    bloodLoss = Mathf.Max(0, bloodLoss - effectiveness * 30f);
                    bloodPressure.value = Mathf.Lerp(bloodPressure.value, 110, effectiveness * 0.4f);
                    break;
                    
                case "bandage":
                    bloodLoss = Mathf.Max(0, bloodLoss - effectiveness * 20f);
                    painLevel = Mathf.Max(0, painLevel - effectiveness * 30f);
                    break;
            }
            
            UpdateState();
        }
    }
}
