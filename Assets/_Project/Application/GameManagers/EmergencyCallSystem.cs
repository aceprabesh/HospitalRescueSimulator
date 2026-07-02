using System.Collections.Generic;
using UnityEngine;
using HospitalRescue.Application.Controllers;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// Handles incoming emergency calls and dispatches to the player
    /// </summary>
    public class EmergencyCallSystem : MonoBehaviour
    {
        public static EmergencyCallSystem Instance { get; private set; }
        
        [Header("Emergency Settings")]
        [SerializeField] private float callRingDuration = 5f;
        [SerializeField] private float responseTimeLimit = 30f;
        [SerializeField] private bool autoDispatch = false;
        
        private List<EmergencyCall> pendingCalls = new List<EmergencyCall>();
        private EmergencyCall activeCall;
        private bool isRinging;
        private float ringTimer;
        
        // Events
        public System.Action<EmergencyCall> OnEmergencyReceived;
        public System.Action<EmergencyCall> OnEmergencyAccepted;
        public System.Action<EmergencyCall> OnEmergencyCompleted;
        public System.Action OnEmergencyExpired;
        public System.Action<float> OnRingProgress;
        
        public EmergencyCall ActiveCall => activeCall;
        public bool IsRinging => isRinging;
        public List<EmergencyCall> PendingCalls => pendingCalls;
        
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
            if (isRinging)
            {
                ringTimer += Time.deltaTime;
                OnRingProgress?.Invoke(ringTimer / callRingDuration);
                
                if (ringTimer >= callRingDuration)
                {
                    DismissCall();
                }
            }
            
            if (activeCall != null && !isRinging)
            {
                activeCall.responseTimer -= Time.deltaTime;
                
                if (activeCall.responseTimer <= 0)
                {
                    ExpireCall();
                }
            }
        }
        
        public void ReceiveEmergencyCall(string location, string description, int severity, PatientController patient)
        {
            EmergencyCall call = new EmergencyCall
            {
                callId = System.Guid.NewGuid().ToString(),
                location = location,
                description = description,
                severity = severity,
                patient = patient,
                timestamp = Time.time,
                responseTimer = responseTimeLimit
            };
            
            pendingCalls.Add(call);
            
            if (!isRinging && activeCall == null)
            {
                StartRinging(call);
            }
            
            OnEmergencyReceived?.Invoke(call);
            Debug.Log($"[DISPATCH] Emergency at {location}: {description} (Severity: {severity})");
        }
        
        public void AcceptCall()
        {
            if (activeCall == null)
            {
                Debug.LogWarning("No active call to accept");
                return;
            }
            
            StopRinging();
            
            // Move patient to critical state
            if (activeCall.patient != null)
            {
                activeCall.patient.Data.currentState = PatientState.Critical;
            }
            
            OnEmergencyAccepted?.Invoke(activeCall);
            Debug.Log($"[DISPATCH] Call accepted. Proceed to {activeCall.location}");
        }
        
        public void DismissCall()
        {
            StopRinging();
            
            if (pendingCalls.Count > 0)
            {
                pendingCalls.RemoveAt(0);
                
                if (pendingCalls.Count > 0)
                {
                    StartRinging(pendingCalls[0]);
                }
            }
            
            Debug.Log("[DISPATCH] Call dismissed");
        }
        
        public void CompleteCall(bool success)
        {
            if (activeCall == null) return;
            
            activeCall.completed = true;
            activeCall.success = success;
            
            OnEmergencyCompleted?.Invoke(activeCall);
            
            pendingCalls.Remove(activeCall);
            activeCall = null;
            
            // Start next call if pending
            if (pendingCalls.Count > 0)
            {
                StartRinging(pendingCalls[0]);
            }
        }
        
        private void ExpireCall()
        {
            if (activeCall == null) return;
            
            Debug.Log($"[DISPATCH] Call expired at {activeCall.location}");
            OnEmergencyExpired?.Invoke();
            
            pendingCalls.Remove(activeCall);
            activeCall = null;
            
            if (pendingCalls.Count > 0)
            {
                StartRinging(pendingCalls[0]);
            }
        }
        
        private void StartRinging(EmergencyCall call)
        {
            activeCall = call;
            isRinging = true;
            ringTimer = 0f;
            
            // Play emergency sound/visual here
            Debug.Log($"[DISPATCH] Ringing: {call.location}");
        }
        
        private void StopRinging()
        {
            isRinging = false;
            ringTimer = 0f;
        }
    }
    
    [System.Serializable]
    public class EmergencyCall
    {
        public string callId;
        public string location;
        public string description;
        public int severity; // 1-5, 5 being most severe
        public PatientController patient;
        public float timestamp;
        public float responseTimer;
        public bool completed;
        public bool success;
        
        public string GetSeverityString()
        {
            return severity switch
            {
                1 => "Minor",
                2 => "Moderate",
                3 => "Serious",
                4 => "Severe",
                5 => "Critical",
                _ => "Unknown"
            };
        }
    }
}
