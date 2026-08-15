using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MissionFailedUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject missionFailedPanel;

    [Header("Results")]
    public TMP_Text failedScoreText;
    public TMP_Text failedPatientsText;

    [Header("Player")]
    public StarterAssets.ThirdPersonController thirdPersonController;
    public StarterAssets.StarterAssetsInputs starterAssetsInputs;

    private void Start()
    {
        if (missionFailedPanel != null)
        {
            missionFailedPanel.SetActive(false);
        }
    }

    public void ShowMissionFailed()
    {
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.SetMissionFailed();
        }

        if (missionFailedPanel != null)
        {
            missionFailedPanel.SetActive(true);
            missionFailedPanel.transform.SetAsLastSibling();
        }

        if (failedScoreText != null && ScoreManager.Instance != null)
        {
            failedScoreText.text =
                "Final Score: " + ScoreManager.Instance.currentScore;
        }

        if (failedPatientsText != null && MissionManager.Instance != null)
        {
            failedPatientsText.text =
                "Patients Stabilized: " +
                MissionManager.Instance.stabilizedPatients +
                " / " +
                MissionManager.Instance.totalPatients;
        }

        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = false;
        }

        if (starterAssetsInputs != null)
        {
            starterAssetsInputs.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("MISSION FAILED SCREEN OPENED");
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}