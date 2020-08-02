using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    // Start new game + entry point
    #region
    private void Start()
    {
        // TO DO: this should not be the game entry point
        // change when we have a main menu and other features
        StartNewGameEvent();
    }
    public void StartNewGameEvent()
    {
        StartCoroutine(StartNewGameEventCoroutine());
    }
    private IEnumerator StartNewGameEventCoroutine()
    {
        // Black out screen, wait until fade in complete
        BlackScreenManager.Instance.SetTotalBlackOutState();
        Action fadeInAction = BlackScreenManager.Instance.FadeIn(1000);
        yield return new WaitUntil(() => fadeInAction.actionResolved);

        StartNewWaveSequenceEvent();
    }
    #endregion

    // Game Events
    #region
    public Action StartNewWaveCompletedEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewWaveCompletedEventCoroutine(action));
        return action;
    }
    private IEnumerator StartNewWaveCompletedEventCoroutine(Action action)
    {
        // Did player just defeat the final wave?
        if(EnemySpawnManager.Instance.currentStage > 0)
        {
            // they did, end the game
            StartNewGameOverVictoryEvent();
        }
        else
        {
            // Create celebratory visual sequence
            Action waveCompleteMessageEvent = UIManager.Instance.ShowNewWaveCompleteMessage();
            yield return new WaitUntil(() => waveCompleteMessageEvent.actionResolved);

            // Advance player stage/level
            EnemySpawnManager.Instance.ModifyCurrentStage(1);

            // Start next wave event
            StartNewWaveSequenceEvent();
        }       

        // Resolve
        action.MarkAsComplete();
    }
    public Action StartNewWaveSequenceEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewWaveSequenceEventCoroutine(action));
        return action;
    }
    public IEnumerator StartNewWaveSequenceEventCoroutine(Action action)
    {
        // Play wave start countdown messages
        Action countDownAction = UIManager.Instance.ShowNewWaveCountdownMessage();
        yield return new WaitUntil(() => countDownAction.actionResolved);

        // Start spawn next enemy wave
        EnemySpawnManager.Instance.SpawnEnemyWave(EnemySpawnManager.Instance.GetNextValidEnemyWave());

        // Resolve event
        action.MarkAsComplete();
    }
    #endregion

    // Game Ending Events
    #region
    public Action StartNewGameOverVictoryEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewGameOverVictoryEventCoroutine(action));
        return action;
    }
    private IEnumerator StartNewGameOverVictoryEventCoroutine(Action action)
    {
        // Did we beat high score?
        if (ScoreManager.Instance.DidPlayerAchieveNewHighScore())
        {
            ScoreManager.Instance.SetNewHighScore(ScoreManager.Instance.CurrentScore());
        }

        // Show game over panel
        UIManager.Instance.FadeInGameOverScreen("Victory");
        yield return null;
    }
    public Action StartNewGameOverDefeatEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewGameOverDefeatEventCoroutine(action));
        return action;
    }
    private IEnumerator StartNewGameOverDefeatEventCoroutine(Action action)
    {
        // Did we beat high score?
        if (ScoreManager.Instance.DidPlayerAchieveNewHighScore())
        {
            ScoreManager.Instance.SetNewHighScore(ScoreManager.Instance.CurrentScore());
        }

        // Show game over panel
        UIManager.Instance.FadeInGameOverScreen("Defeat");
        yield return null;
    }
    public Action StartResetGameSequence()
    {
        Action action = new Action();
        StartCoroutine(StartResetGameSequenceCoroutine(action));
        return action;

    }
    private IEnumerator StartResetGameSequenceCoroutine(Action action)
    {
        // Black out screen
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 2, 1, true);
        yield return new WaitUntil(() => fadeOut.actionResolved);

        // Reset
        ResetGameState();

        // Start new game
        StartNewGameEvent();
    }
    #endregion

    // Misc Logic
    #region
    public void ResetGameState()
    {
        // Reset player
        FindObjectOfType<Player>().ResetToStartSettings();

        // Reset Managers
        EnemySpawnManager.Instance.ResetToStartSettings();
        ScoreManager.Instance.ResetToStartSettings();
        UIManager.Instance.ResetToStartSettings();
    }
    #endregion
}
