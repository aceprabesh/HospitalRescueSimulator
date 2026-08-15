using UnityEngine;
using TMPro;

public enum TreatmentType
{
    Bandage,
    Water,
    AED,
    OxygenMask,
    Glucose,
    FirstAidKit,
     Splint
}

public class PatientTreatment : MonoBehaviour
{
    [Header("Treatment Setup")]
    public TreatmentType correctTreatment;

    public int correctScore = 100;
    public int wrongPenalty = 25;

    public GameObject patientStatusText;

    [Header("Option 1")]
    public string option1Label = "Apply Bandage";
    public TreatmentType option1Type = TreatmentType.Bandage;

    [TextArea]
    public string option1Feedback = "Incorrect treatment. Try again.";

    [Header("Option 2")]
    public string option2Label = "Give Water";
    public TreatmentType option2Type = TreatmentType.Water;

    [TextArea]
    public string option2Feedback = "Incorrect treatment. Try again.";

    [Header("Option 3")]
    public string option3Label = "Use AED";
    public TreatmentType option3Type = TreatmentType.AED;

    [TextArea]
    public string option3Feedback = "Incorrect treatment. Try again.";

    [HideInInspector]
    public bool treatmentCompleted = false;

    private bool option1PenaltyGiven = false;
    private bool option2PenaltyGiven = false;
    private bool option3PenaltyGiven = false;

    public string GetOptionLabel(int option)
    {
        if (option == 1)
            return option1Label;

        if (option == 2)
            return option2Label;

        if (option == 3)
            return option3Label;

        return "";
    }

    public bool ChooseOption(int option, TMP_Text feedbackText)
    {
        if (treatmentCompleted)
            return true;

        TreatmentType selectedTreatment;
        string selectedFeedback;

        if (option == 1)
        {
            selectedTreatment = option1Type;
            selectedFeedback = option1Feedback;
        }
        else if (option == 2)
        {
            selectedTreatment = option2Type;
            selectedFeedback = option2Feedback;
        }
        else if (option == 3)
        {
            selectedTreatment = option3Type;
            selectedFeedback = option3Feedback;
        }
        else
        {
            return false;
        }

        // CORRECT TREATMENT
        if (selectedTreatment == correctTreatment)
        {
            treatmentCompleted = true;

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(correctScore);
            }
            if (MissionManager.Instance != null)
{
    MissionManager.Instance.PatientStabilized();
}

            if (feedbackText != null)
            {
                feedbackText.text = selectedFeedback;
                feedbackText.color = new Color32(50, 220, 80, 255);
            }

            if (patientStatusText != null)
            {
                patientStatusText.SetActive(true);
            }

            Debug.Log("Correct treatment chosen.");

            return true;
        }

        // WRONG TREATMENT
        bool applyPenalty = false;

        if (option == 1 && !option1PenaltyGiven)
        {
            option1PenaltyGiven = true;
            applyPenalty = true;
        }
        else if (option == 2 && !option2PenaltyGiven)
        {
            option2PenaltyGiven = true;
            applyPenalty = true;
        }
        else if (option == 3 && !option3PenaltyGiven)
        {
            option3PenaltyGiven = true;
            applyPenalty = true;
        }

        if (applyPenalty)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.RemoveScore(wrongPenalty);
            }

            Debug.Log("Wrong treatment penalty applied: -" + wrongPenalty);
        }

        if (feedbackText != null)
        {
            feedbackText.text = selectedFeedback;
            feedbackText.color = new Color32(235, 50, 50, 255);

            // Force TMP to redraw immediately
            feedbackText.ForceMeshUpdate();
        }
        else
        {
            Debug.LogWarning("FeedbackText reference is missing.");
        }

        return false;
    }
}