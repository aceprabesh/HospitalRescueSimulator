using System.Collections;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Patient Progress")]
    public int totalPatients = 5;
    public int stabilizedPatients = 0;

    [Header("UI")]
    public TMP_Text stabilizedText;

    [Header("Mission Complete")]
    public MissionCompleteUI missionCompleteUI;

    // Other scripts can check whether the game has ended.
    public bool gameEnded = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void PatientStabilized()
    {
        if (gameEnded)
            return;

        stabilizedPatients++;

        if (stabilizedPatients > totalPatients)
        {
            stabilizedPatients = totalPatients;
        }

        UpdateUI();

        Debug.Log(
            "Patient stabilized: " +
            stabilizedPatients +
            "/" +
            totalPatients
        );

        if (stabilizedPatients >= totalPatients)
        {
            StartCoroutine(CompleteMissionAfterDelay());
        }
    }

    private IEnumerator CompleteMissionAfterDelay()
    {
        // Mark game as ended IMMEDIATELY.
        // This stops Pause from opening during the 2-second delay.
        gameEnded = true;

        Debug.Log("ALL PATIENTS STABILIZED!");

        // Stop timer immediately when final patient is treated.
        GameTimer timer =
            FindFirstObjectByType<GameTimer>();

        if (timer != null)
        {
            timer.StopTimer();
        }

        // Disable pause immediately.
        PauseMenu pauseMenu =
            FindFirstObjectByType<PauseMenu>();

        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }

        // Allow final patient's assessment/green feedback to finish.
        yield return new WaitForSecondsRealtime(2f);

        if (missionCompleteUI != null)
        {
            missionCompleteUI.ShowMissionComplete();
        }
        else
        {
            Debug.LogWarning(
                "MissionCompleteUI is not assigned."
            );
        }
    }

    public void SetMissionFailed()
    {
        gameEnded = true;

        PauseMenu pauseMenu =
            FindFirstObjectByType<PauseMenu>();

        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }
    }

    private void UpdateUI()
    {
        if (stabilizedText != null)
        {
            stabilizedText.text =
                "Stabilized: " +
                stabilizedPatients +
                "/" +
                totalPatients;
        }
    }
}