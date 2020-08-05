using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Component References")]
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Properties")]
    private int currentScore;
    // TO DO: if there is time, save high score to player prefs for persistency between sessions
    private int highScore = 0;

    public int CurrentScore()
    {
        return currentScore;
    }
    public void ModifyScore(int scoreGainedOrLost)
    {
        currentScore += scoreGainedOrLost;
        UpdateScoreText();
    }
    public void SetNewHighScore(int newHighScore)
    {
        highScore = newHighScore;
        UpdateHighScoreText(newHighScore);
    }
    private void UpdateScoreText()
    {
        currentScoreText.text = currentScore.ToString();
    }
    private void UpdateHighScoreText(int newHighScore)
    {
        highScoreText.text = newHighScore.ToString();
    }
    public void ResetToStartSettings()
    {
        ModifyScore(-currentScore);
    }
    public bool DidPlayerAchieveNewHighScore()
    {
        return currentScore > highScore;
    }
}
