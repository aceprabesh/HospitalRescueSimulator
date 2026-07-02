using UnityEngine;
using HospitalRescue.Application.Controllers;
using HospitalRescue.Domain.Interfaces;

namespace HospitalRescue.Infrastructure.Services
{
    /// <summary>
    /// Base class for all medical equipment interactions
    /// </summary>
    public abstract class MedicalEquipment : MonoBehaviour, IInteractable
    {
        [Header("Equipment Info")]
        [SerializeField] protected string equipmentName = "Medical Equipment";
        [SerializeField] protected string interactionPrompt = "Press E to use";
        [SerializeField] protected float useTime = 2f;
        [SerializeField] protected bool holdToUse = true;
        
        [Header("Visual Feedback")]
        [SerializeField] protected Renderer equipmentRenderer;
        [SerializeField] protected Color activeColor = Color.cyan;
        [SerializeField] protected Color inUseColor = Color.yellow;
        
        protected bool isBeingUsed;
        protected float useProgress;
        protected PatientController currentPatient;
        
        public string InteractionPrompt => interactionPrompt;
        public float InteractionTime => useTime;
        public bool HoldToInteract => holdToUse;
        
        protected virtual void Awake()
        {
            if (equipmentRenderer == null)
                equipmentRenderer = GetComponent<Renderer>();
        }
        
        public virtual bool CanInteract()
        {
            return !isBeingUsed;
        }
        
        public virtual void OnInteract()
        {
            Debug.Log($"Using {equipmentName}");
            ApplyEffect();
        }
        
        public virtual void OnInteractStart()
        {
            isBeingUsed = true;
            useProgress = 0f;
            
            if (equipmentRenderer != null)
                equipmentRenderer.material.color = inUseColor;
        }
        
        public virtual void OnInteractEnd()
        {
            isBeingUsed = false;
            useProgress = 0f;
            
            if (equipmentRenderer != null)
                equipmentRenderer.material.color = Color.white;
        }
        
        protected virtual void ApplyEffect()
        {
            // Override in subclasses
        }
        
        protected void FindNearbyPatient()
        {
            // Find patient within range
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
            foreach (var col in colliders)
            {
                PatientController patient = col.GetComponent<PatientController>();
                if (patient != null)
                {
                    currentPatient = patient;
                    return;
                }
            }
            currentPatient = null;
        }
    }
    
    /// <summary>
    /// Defibrillator for cardiac arrest patients
    /// </summary>
    public class Defibrillator : MedicalEquipment
    {
        [Header("Defibrillator Settings")]
        [SerializeField] private int shockPower = 200; // Joules
        [SerializeField] private int maxShocks = 6;
        [SerializeField] private float rechargeTime = 3f;
        
        private int shocksUsed;
        private float rechargeTimer;
        private bool isCharged = true;
        
        protected override void Awake()
        {
            base.Awake();
            equipmentName = "Defibrillator";
            interactionPrompt = "Press E to shock";
            useTime = 1.5f;
        }
        
        private void Update()
        {
            if (!isCharged)
            {
                rechargeTimer += Time.deltaTime;
                if (rechargeTimer >= rechargeTime)
                {
                    isCharged = true;
                    rechargeTimer = 0f;
                    Debug.Log("[DEFIBRILLATOR] Charged and ready");
                }
            }
        }
        
        protected override void ApplyEffect()
        {
            FindNearbyPatient();
            
            if (currentPatient == null)
            {
                Debug.Log("[DEFIBRILLATOR] No patient nearby!");
                return;
            }
            
            if (!isCharged)
            {
                Debug.Log("[DEFIBRILLATOR] Not charged!");
                return;
            }
            
            if (shocksUsed >= maxShocks)
            {
                Debug.Log("[DEFIBRILLATOR] No charges remaining!");
                return;
            }
            
            // Apply shock
            currentPatient.ApplyTreatment("defibrillator", 0.8f);
            shocksUsed++;
            isCharged = false;
            
            Debug.Log($"[DEFIBRILLATOR] Shock delivered! ({shocksUsed}/{maxShocks})");
            
            if (equipmentRenderer != null)
                equipmentRenderer.material.color = activeColor;
        }
    }
    
    /// <summary>
    /// CPR procedure for unconscious patients
    /// </summary>
    public class CPRProcedure : MedicalEquipment
    {
        [Header("CPR Settings")]
        [SerializeField] private int compressionsRequired = 30;
        [SerializeField] private float compressionRate = 100f; // per minute
        [SerializeField] private float compressionDepth = 2f; // inches
        
        private int compressionCount;
        private float compressionTimer;
        
        protected override void Awake()
        {
            base.Awake();
            equipmentName = "CPR";
            interactionPrompt = "Hold E to perform CPR";
            useTime = 2f;
            holdToUse = true;
        }
        
        protected override void ApplyEffect()
        {
            FindNearbyPatient();
            
            if (currentPatient == null)
            {
                Debug.Log("[CPR] No patient nearby!");
                return;
            }
            
            // CPR increases heart rate and consciousness
            currentPatient.ApplyTreatment("cpr", 0.6f);
            compressionCount = 0;
            
            Debug.Log($"[CPR] Performing chest compressions...");
        }
        
        public override void OnInteractStart()
        {
            base.OnInteractStart();
            compressionCount = 0;
            compressionTimer = 0f;
        }
        
        private void Update()
        {
            if (isBeingUsed)
            {
                useProgress += Time.deltaTime / useTime;
                
                // Simulate compressions over time
                compressionTimer += Time.deltaTime;
                float expectedCompressions = (compressionTimer / 60f) * compressionRate;
                
                if ((int)expectedCompressions > compressionCount)
                {
                    compressionCount = (int)expectedCompressions;
                    
                    // Apply small healing per compression
                    if (currentPatient != null)
                    {
                        currentPatient.Data.heartRate.value = Mathf.Lerp(
                            currentPatient.Data.heartRate.value, 
                            70f, 
                            0.02f
                        );
                    }
                }
                
                if (useProgress >= 1f)
                {
                    Debug.Log($"[CPR] Cycle complete: {compressionCount} compressions");
                    compressionCount = 0;
                    useProgress = 0f;
                }
            }
        }
    }
    
    /// <summary>
    /// Oxygen mask for patients with low O2
    /// </summary>
    public class OxygenMask : MedicalEquipment
    {
        [Header("Oxygen Settings")]
        [SerializeField] private float oxygenFlowRate = 6f; // L/min
        [SerializeField] private float maxOxygenTime = 30f;
        
        private float oxygenUsed;
        
        protected override void Awake()
        {
            base.Awake();
            equipmentName = "Oxygen Mask";
            interactionPrompt = "Press E to apply O₂";
            useTime = 1f;
            holdToUse = false;
        }
        
        protected override void ApplyEffect()
        {
            FindNearbyPatient();
            
            if (currentPatient == null)
            {
                Debug.Log("[OXYGEN] No patient nearby!");
                return;
            }
            
            currentPatient.ApplyTreatment("oxygen", 0.7f);
            oxygenUsed += useTime * oxygenFlowRate / 60f;
            
            Debug.Log($"[OXYGEN] Applied to {currentPatient.Data.patientName}");
        }
    }
    
    /// <summary>
    /// IV drip for blood loss patients
    /// </summary>
    public class IVDrip : MedicalEquipment
    {
        [Header("IV Settings")]
        [SerializeField] private float flowRate = 100f; // mL/hour
        [SerializeField] private float maxVolume = 500f; // mL
        
        private float volumeUsed;
        
        protected override void Awake()
        {
            base.Awake();
            equipmentName = "IV Drip";
            interactionPrompt = "Press E to set up IV";
            useTime = 3f;
            holdToUse = true;
        }
        
        protected override void ApplyEffect()
        {
            FindNearbyPatient();
            
            if (currentPatient == null)
            {
                Debug.Log("[IV] No patient nearby!");
                return;
            }
            
            currentPatient.ApplyTreatment("iv", 0.5f);
            volumeUsed += flowRate * useTime / 3600f;
            
            Debug.Log($"[IV] IV established for {currentPatient.Data.patientName}");
        }
    }
    
    /// <summary>
    /// Bandage for wound treatment
    /// </summary>
    public class Bandage : MedicalEquipment
    {
        [Header("Bandage Settings")]
        [SerializeField] private float bandageQuality = 0.8f;
        
        protected override void Awake()
        {
            base.Awake();
            equipmentName = "Bandage";
            interactionPrompt = "Press E to bandage";
            useTime = 2f;
            holdToUse = true;
        }
        
        protected override void ApplyEffect()
        {
            FindNearbyPatient();
            
            if (currentPatient == null)
            {
                Debug.Log("[BANDAGE] No patient nearby!");
                return;
            }
            
            currentPatient.ApplyTreatment("bandage", bandageQuality);
            
            Debug.Log($"[BANDAGE] Applied to {currentPatient.Data.patientName}");
        }
    }
}
