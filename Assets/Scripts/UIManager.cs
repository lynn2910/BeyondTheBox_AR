using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private ImageTrackedCollectibles collectibles;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CountdownTimer countdownTimer;

    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text collectedText;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private GameManager gameManager;

    private void OnEnable()
    {
        if (gameManager != null) gameManager.GameStateChanged += HandleGameStateChanged;

        if (collectibles != null)
        {
            collectibles.StatusChanged += UpdateStatus;
        }

        if (playerStats != null)
        {
            playerStats.StatsChanged += UpdateStats;
        }
    }

    private void Start()
    {
        UpdateStats();

        if (statusText != null)
        {
            statusText.text = "Collectibles: Ready";
        }
    }

    private void OnDisable()
    {
        if (gameManager != null) gameManager.GameStateChanged -= HandleGameStateChanged;

        if (collectibles != null)
        {
            collectibles.StatusChanged -= UpdateStatus;
        }

        if (playerStats != null)
        {
            playerStats.StatsChanged -= UpdateStats;
        }
    }

    public void ResetStats()
    {
        playerStats.ResetAllProgressInThisApp();
        countdownTimer.ResetTimer();
    }

    private void UpdateStatus(string status)
    {
        if (statusText != null)
        {
            statusText.text = $"Collectibles: {status}";
        }
    }

    private void UpdateStats()
    {
        if (playerStats == null) return;

        if (collectedText != null)
        {
            collectedText.text = $"Found: {playerStats.CollectedCount} / 4";
        }

        if (pointsText != null)
        {
            pointsText.gameObject.SetActive(false);
        }
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (statusText == null) return;

        if (state == GameManager.GameState.Victory)
        {
            statusText.text = "<color=green>MISSION ACCOMPLISHED!</color>";
        }
        else if (state == GameManager.GameState.Defeat)
        {
            statusText.text = "<color=red>TIME'S UP! MISSION FAILED.</color>";
        }
    }
}