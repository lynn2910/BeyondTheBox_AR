using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Victory,
        Defeat
    }

    public GameState State { get; private set; }

    [Header("Core References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CountdownTimer countdownTimer;

    [Header("Win Condition")]
    [Tooltip("The total number of sequential markers the player must find to win.")]
    [SerializeField] private int requiredCollectibles = 4;

    // Event raised whenever the game state changes (UI can listen to this)
    public event Action<GameState> GameStateChanged;

    private void OnEnable()
    {
        // Subscribe to events from your existing scripts
        if (playerStats != null)
        {
            playerStats.StatsChanged += CheckWinCondition;
        }
        
        if (countdownTimer != null)
        {
            countdownTimer.TimerFinished += OnTimeOut;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        if (playerStats != null)
        {
            playerStats.StatsChanged -= CheckWinCondition;
        }

        if (countdownTimer != null)
        {
            countdownTimer.TimerFinished -= OnTimeOut;
        }
    }

    private void Start()
    {
        ChangeState(GameState.Playing);
    }

    private void CheckWinCondition()
    {
        if (State != GameState.Playing) return;

        // Check if the collected count meets the requirement
        if (playerStats.CollectedCount >= requiredCollectibles)
        {
            ChangeState(GameState.Victory);
        }
    }

    private void OnTimeOut()
    {
        if (State == GameState.Playing)
        {
            ChangeState(GameState.Defeat);
        }
    }

    private void ChangeState(GameState newState)
    {
        State = newState;
        GameStateChanged?.Invoke(State);

        switch (newState)
        {
            case GameState.Playing:
                Debug.Log("GameManager: Game Started!");
                break;
            case GameState.Victory:
                Debug.Log("GameManager: Player has found all markers! You Win!");
                if (countdownTimer != null) countdownTimer.StopTimer();
                break;
            case GameState.Defeat:
                Debug.Log("GameManager: Time ran out! You Lose!");
                break;
        }
    }
}