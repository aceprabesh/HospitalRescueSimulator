using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 300f; // 5 minutes
    public TMP_Text timerText;

    private bool timerRunning = true;
    private bool missionEnded = false;

    void Start()
    {
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!timerRunning || missionEnded)
            return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining < 0)
            {
                timeRemaining = 0;
            }

            UpdateTimerDisplay();
        }
        else
        {
            TimeUp();
        }
    }

    private void TimeUp()
    {
        if (missionEnded)
            return;

        missionEnded = true;
        timerRunning = false;
        timeRemaining = 0;

        UpdateTimerDisplay();

        Debug.Log("TIME'S UP - MISSION FAILED");

        MissionFailedUI failedUI =
            FindFirstObjectByType<MissionFailedUI>();

        if (failedUI != null)
        {
            failedUI.ShowMissionFailed();
        }
        else
        {
            Debug.LogWarning(
                "MissionFailedUI could not be found."
            );
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null)
            return;

        int minutes =
            Mathf.FloorToInt(timeRemaining / 60f);

        int seconds =
            Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text =
            string.Format(
                "{0:00}:{1:00}",
                minutes,
                seconds
            );

        if (timeRemaining <= 60f)
        {
            timerText.color = Color.red;
        }
        else if (timeRemaining <= 120f)
        {
            timerText.color = Color.yellow;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    public void StopTimer()
    {
        timerRunning = false;
        missionEnded = true;
    }
}