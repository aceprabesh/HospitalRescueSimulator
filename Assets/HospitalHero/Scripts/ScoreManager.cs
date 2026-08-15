using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore = 0;
    public TMP_Text scoreText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
        }

        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
    }

    public void RemoveScore(int amount)
    {
        currentScore -= amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
            scoreText.text = "Score: " + currentScore;
        }
    }
}