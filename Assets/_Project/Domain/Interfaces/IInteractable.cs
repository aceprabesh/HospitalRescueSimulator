using UnityEngine;

namespace HospitalRescue.Domain.Interfaces
{
    /// <summary>
    /// Interface for objects that can be interacted with
    /// </summary>
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        float InteractionTime { get; }
        bool HoldToInteract { get; }
        
        bool CanInteract();
        void OnInteract();
        void OnInteractStart();
        void OnInteractEnd();
    }
}
