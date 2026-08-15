using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MissionCompleteUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject missionCompletePanel;

    [Header("Results")]
    public TMP_Text finalScoreText;
    public TMP_Text finalTimeText;

    [Header("Player")]
    public StarterAssets.ThirdPersonController thirdPersonController;
    public StarterAssets.StarterAssetsInputs starterAssetsInputs;

    private void Start()
    {
        if (missionCompletePanel != null)
        {
            missionCompletePanel.SetActive(false);
        }
    }

    public void ShowMissionComplete()
    {
        if (missionCompletePanel != null)
        {
            missionCompletePanel.SetActive(true);
            missionCompletePanel.transform.SetAsLastSibling();
        }

        // Update final score.
        if (finalScoreText != null &&
            ScoreManager.Instance != null)
        {
            finalScoreText.text =
                "Final Score: " +
                ScoreManager.Instance.currentScore;
        }

        // Update remaining time.
        GameTimer timer =
            FindFirstObjectByType<GameTimer>();

        if (finalTimeText != null && timer != null)
        {
            float remaining = timer.timeRemaining;

            int minutes =
                Mathf.FloorToInt(remaining / 60f);

            int seconds =
                Mathf.FloorToInt(remaining % 60f);

            finalTimeText.text =
                $"Time Remaining: {minutes:00}:{seconds:00}";
        }

        // Freeze gameplay.
        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = false;
        }

        if (starterAssetsInputs != null)
        {
            starterAssetsInputs.enabled = false;
        }

        // Force cursor available for result buttons.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("MISSION COMPLETE SCREEN OPENED");
    }

    public void PlayAgain()
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