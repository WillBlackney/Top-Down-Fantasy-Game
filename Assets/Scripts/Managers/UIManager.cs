using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Wave Compete View Components")]
    public GameObject waveCompleteParent;
    public CanvasGroup waveCompleteCG;
    public TextMeshProUGUI waveCompleteText;
    public float waveCompleteFadeSpeed;

    [Header("Wave Countdown View Components")]
    public GameObject countDownParent;
    public CanvasGroup countDownCG;
    public TextMeshProUGUI countDownText;
    public float countDownFadeSpeed;
    public float scalingSpeed;


    [Header("Game Over Screen Components")]
    public GameObject gameOverScreenVisualParent;
    public CanvasGroup gameOverScreenCG;
    public TextMeshProUGUI gameOverScreenBannerText;
    public float gameOverScreenFadeInSpeed;
    public Button gameOverScreenMainMenuButton;

    public Action ShowNewWaveCompleteMessage()
    {
        Action action = new Action();
        StartCoroutine(ShowNewWaveCompleteMessageCoroutine(action));
        return action;
    }
    private IEnumerator ShowNewWaveCompleteMessageCoroutine(Action action)
    {
        waveCompleteParent.SetActive(true);
        waveCompleteCG.alpha = 0;
        waveCompleteText.text = "Wave " + EnemySpawnManager.Instance.currentStage.ToString() + " Completed!";

        // Fade in screen
        while (waveCompleteCG.alpha < 1)
        {
            waveCompleteCG.alpha += waveCompleteFadeSpeed * Time.deltaTime;
            yield return null;
        }

        // Brief pause to show message
        yield return new WaitForSeconds(1);

        // Fade out again
        while (waveCompleteCG.alpha > 0)
        {
            waveCompleteCG.alpha -= waveCompleteFadeSpeed * Time.deltaTime;
            yield return null;
        }

        action.MarkAsComplete();

    }
    public Action ShowNewWaveCountdownMessage()
    {
        Action action = new Action();
        StartCoroutine(ShowNewWaveCountdownMessageCoroutine(action));
        return action;
    }
    private IEnumerator ShowNewWaveCountdownMessageCoroutine(Action action)
    {
        // Brief pause at start
        yield return new WaitForSeconds(1);

        // Set starting view state
        countDownParent.SetActive(true);
        countDownCG.alpha = 0;

        // Reset scale
        countDownParent.transform.localScale = new Vector3(1, 1, 1);

        // Show wave count message first
        countDownText.text = "Wave " + EnemySpawnManager.Instance.currentStage.ToString();
        yield return new WaitForSeconds(0.2f);

        // Start fade in
        while (countDownCG.alpha < 1)
        {
            countDownCG.alpha += countDownFadeSpeed * Time.deltaTime;
            yield return null;
        }

        // Wait for player to read wave text
        yield return new WaitForSeconds(0.5f);

        // Start fade out
        while (countDownCG.alpha > 0)
        {
            countDownCG.alpha -= countDownFadeSpeed * Time.deltaTime;
            yield return null;
        }

        // Brief wait for countdown to start
        yield return new WaitForSeconds(0.5f);

        for (int i = 3; i > 0; i--)
        {
            // Brief yield so player can see number
            countDownParent.transform.localScale = new Vector3(1, 1, 1);
            countDownText.text = i.ToString();
            countDownCG.alpha = 1;
            yield return new WaitForSeconds(0.2f);

            // Start fade out
            while (countDownCG.alpha > 0)
            {
                float newScaleSize = countDownParent.transform.localScale.x + (scalingSpeed * Time.deltaTime);
                countDownParent.transform.localScale = new Vector3(newScaleSize, newScaleSize, newScaleSize);
                countDownCG.alpha -= countDownFadeSpeed * Time.deltaTime;
                yield return null;
            }

            // Brief wait until next number
            yield return new WaitForSeconds(0.5f);
        }

        action.MarkAsComplete();
    }
    public Action FadeInGameOverScreen(string bannerText)
    {
        Action action = new Action();
        StartCoroutine(FadeInGameOverScreenCoroutine(action, bannerText));
        return action;
    }
    private IEnumerator FadeInGameOverScreenCoroutine(Action action, string bannerText)
    {
        gameOverScreenBannerText.text = bannerText;
        gameOverScreenVisualParent.SetActive(true);
        gameOverScreenCG.alpha = 0;
        gameOverScreenMainMenuButton.interactable = true;

        while(gameOverScreenCG.alpha < 1)
        {
            gameOverScreenCG.alpha += gameOverScreenFadeInSpeed * Time.deltaTime;
            yield return null;
        }

        yield return null;
    }
    public void OnBackToMainMenuButtonClicked()
    {
        // prevent user clicking multiple times
        gameOverScreenMainMenuButton.interactable = false;

        // Start reset game event
        EventManager.Instance.StartResetGameSequence();       

    }
    public void ResetToStartSettings()
    {
        gameOverScreenVisualParent.SetActive(false);
    }
}
