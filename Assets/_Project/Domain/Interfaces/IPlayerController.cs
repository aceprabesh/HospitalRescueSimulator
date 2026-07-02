namespace HospitalRescue.Domain.Interfaces
{
    /// <summary>
    /// Interface for player controller operations
    /// </summary>
    public interface IPlayerController
    {
        void Move(Vector3 direction);
        void Rotate(float mouseX, float mouseY);
        void Sprint(bool isSprinting);
        void Crouch(bool isCrouching);
        bool IsSprinting { get; }
        bool IsCrouching { get; }
        bool IsGrounded { get; }
    }
}
