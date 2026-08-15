using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssessmentUIController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject assessmentPanel;

    public TMP_Text patientNameText;
    public TMP_Text symptomsText;
    public TMP_Text instructionText;
    public TMP_Text feedbackText;

    [Header("Buttons")]
    public Button treatmentButton1;
    public Button treatmentButton2;
    public Button treatmentButton3;

    public TMP_Text treatmentButton1Text;
    public TMP_Text treatmentButton2Text;
    public TMP_Text treatmentButton3Text;

    [Header("Player")]
    public StarterAssets.ThirdPersonController thirdPersonController;
    public StarterAssets.StarterAssetsInputs starterAssetsInputs;

    private PatientTreatment currentTreatment;
    private bool closingPanel = false;

    private void Start()
    {
        if (assessmentPanel != null)
            assessmentPanel.SetActive(false);
    }

    public void OpenAssessment(
        PatientInteractable patient,
        PatientTreatment treatment)
    {
        currentTreatment = treatment;
        closingPanel = false;

        if (patientNameText != null)
            patientNameText.text =
                "Patient: " + patient.GetPatientName();

        if (symptomsText != null)
        {
            string formatted =
                patient.GetSymptoms()
                .Replace("\n", "\n• ");

            symptomsText.text =
                "Symptoms:\n• " + formatted;
        }

        if (instructionText != null)
            instructionText.text =
                patient.GetInstruction();

        if (feedbackText != null)
        {
            feedbackText.text = "";
            feedbackText.color = Color.white;
        }

        if (treatmentButton1Text != null)
            treatmentButton1Text.text =
                treatment.GetOptionLabel(1);

        if (treatmentButton2Text != null)
            treatmentButton2Text.text =
                treatment.GetOptionLabel(2);

        if (treatmentButton3Text != null)
            treatmentButton3Text.text =
                treatment.GetOptionLabel(3);

        treatmentButton1.interactable = true;
        treatmentButton2.interactable = true;
        treatmentButton3.interactable = true;

        if (assessmentPanel != null)
            assessmentPanel.SetActive(true);

        if (thirdPersonController != null)
            thirdPersonController.enabled = false;

        if (starterAssetsInputs != null)
            starterAssetsInputs.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ChooseOption1()
    {
        ProcessOption(1);
    }

    public void ChooseOption2()
    {
        ProcessOption(2);
    }

    public void ChooseOption3()
    {
        ProcessOption(3);
    }

    private void ProcessOption(int option)
    {
        if (currentTreatment == null || closingPanel)
            return;

        bool correct =
            currentTreatment.ChooseOption(
                option,
                feedbackText
            );

        if (correct)
        {
            closingPanel = true;

            treatmentButton1.interactable = false;
            treatmentButton2.interactable = false;
            treatmentButton3.interactable = false;

            StartCoroutine(ClosePanelAfterDelay());
        }
    }

    private IEnumerator ClosePanelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(1.2f);

        if (assessmentPanel != null)
            assessmentPanel.SetActive(false);

        if (thirdPersonController != null)
            thirdPersonController.enabled = true;

        if (starterAssetsInputs != null)
            starterAssetsInputs.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentTreatment = null;
        closingPanel = false;
    }
}