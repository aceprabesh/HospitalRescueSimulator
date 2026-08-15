using UnityEngine;

public class PatientInteractable : MonoBehaviour
{
    [Header("Patient Info")]
    public string patientName = "Hero";

    [TextArea]
    public string symptoms =
        "Heavy bleeding from the arm\nFeeling dizzy and weak";

    public string treatmentInstruction =
        "Choose the best treatment:";

    [Header("Interaction")]
    public float interactionDistance = 2.5f;

    public GameObject interactionPrompt;
    public AssessmentUIController assessmentUI;

    private Transform player;
    private PatientTreatment treatment;

    private bool interactedThisVisit = false;
    private bool wasClose = false;

    // Only one patient may control the shared prompt at a time
    private static PatientInteractable promptOwner;

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        treatment = GetComponent<PatientTreatment>();
    }

    private void Update()
    {
        if (player == null || treatment == null)
            return;

        float distance =
            Vector3.Distance(transform.position, player.position);

        bool close =
            distance <= interactionDistance;

        // Treated patient should NOT keep disabling
        // another patient's shared prompt.
        if (treatment.treatmentCompleted)
        {
            if (promptOwner == this)
            {
                HidePrompt();
            }

            wasClose = close;
            return;
        }

        if (!close)
        {
            if (wasClose)
            {
                interactedThisVisit = false;
            }

            if (promptOwner == this)
            {
                HidePrompt();
            }

            wasClose = false;
            return;
        }

        // Player is close to this untreated patient
        if (!interactedThisVisit)
        {
            ShowPrompt();

            if (Input.GetKeyDown(KeyCode.E))
            {
                interactedThisVisit = true;

                HidePrompt();

                if (assessmentUI != null)
                {
                    assessmentUI.OpenAssessment(
                        this,
                        treatment
                    );
                }
            }
        }

        wasClose = true;
    }

    private void ShowPrompt()
    {
        // Hide prompt owned by another patient first
        if (promptOwner != null &&
            promptOwner != this &&
            promptOwner.interactionPrompt != null)
        {
            promptOwner.interactionPrompt.SetActive(false);
        }

        promptOwner = this;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
    }

    private void HidePrompt()
    {
        if (promptOwner != this)
            return;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        promptOwner = null;
    }

    public string GetPatientName()
    {
        return patientName;
    }

    public string GetSymptoms()
    {
        return symptoms;
    }

    public string GetInstruction()
    {
        return treatmentInstruction;
    }
}