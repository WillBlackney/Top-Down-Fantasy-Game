using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Component References")]
    public TextMeshProUGUI currentScoreText;

    [Header("Properties")]
    public int currentScore;
    public void ModifyScore(int scoreGainedOrLost)
    {
        currentScore += scoreGainedOrLost;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        currentScoreText.text = currentScore.ToString();
    }
}
