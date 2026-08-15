using UnityEngine;

public class AnimatorSync : MonoBehaviour
{
    public Animator sourceAnimator;
    public Animator targetAnimator;

    private void Update()
    {
        if (sourceAnimator == null || targetAnimator == null)
            return;

        CopyFloat("Speed");
        CopyFloat("MotionSpeed");

        CopyBool("Grounded");
        CopyBool("Jump");
        CopyBool("FreeFall");
    }

    private void CopyFloat(string parameterName)
    {
        targetAnimator.SetFloat(
            parameterName,
            sourceAnimator.GetFloat(parameterName)
        );
    }

    private void CopyBool(string parameterName)
    {
        targetAnimator.SetBool(
            parameterName,
            sourceAnimator.GetBool(parameterName)
        );
    }

    // Receives Starter Assets animation events.
    public void OnFootstep(AnimationEvent animationEvent)
    {
        // Intentionally empty.
        // Prevents missing receiver errors on DoctorModel.
    }

    public void OnLand(AnimationEvent animationEvent)
    {
        // Intentionally empty.
        // Prevents missing receiver errors on DoctorModel.
    }
}