using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ImageTrackedCollectibles collectibles;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text collectedText;
    [SerializeField] private TMP_Text pointsText;

    [Header("Standard Message Panel")]
    [SerializeField] private GameObject standardMessagePanel; 
    [SerializeField] private TMP_Text standardMessageText;
    [SerializeField] private TMP_Text standardMessageButtonText;

    [Header("Custom Panels")]
    [SerializeField] private KeypadPanel keypadPanelScript;

    private void OnEnable()
    {
        if (gameManager != null) gameManager.GameStateChanged += HandleGameStateChanged;

        if (collectibles != null)
        {
            collectibles.StatusChanged += UpdateStatus;
            collectibles.CollectibleCollected += HandleItemCollected;
        }

        if (playerStats != null)
        {
            playerStats.StatsChanged += UpdateStats;
        }

        if (keypadPanelScript != null)
        {
            keypadPanelScript.OnKeypadSolved += HandleKeypadSolved; 
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
            collectibles.CollectibleCollected -= HandleItemCollected;
        }

        if (playerStats != null)
        {
            playerStats.StatsChanged -= UpdateStats;
        }

        if (keypadPanelScript != null) keypadPanelScript.OnKeypadSolved -= HandleKeypadSolved;
    }

    public void ResetStats()
    {
        playerStats.ResetAllProgressInThisApp();
        countdownTimer.ResetTimer();
        
        HideAllPanels(); 
        
        if (statusText != null)
        {
            statusText.text = "Collectibles: Ready";
        }
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

    private void HandleItemCollected(string collectibleId)
    {
        HideAllPanels();

        switch (collectibleId)
        {
            case "first_marker":
                keypadPanelScript.gameObject.SetActive(true); 
                break;
                
            case "politechnika_logo":
                ShowStandardMessage("You found the Politechnika Logo! Keep going.", "Continue");
                break;

            case "iot_wall":
                ShowStandardMessage("Great job! Now locate the final marker.", "Let's Go!");
                break;
        }
    }

    // --- PANEL CONTROLS ---

    private void HandleKeypadSolved()
    {
        HideAllPanels();
        ShowStandardMessage("ACCESS GRANTED.\n\nProceed to the IoT Wall.", "Acknowledge");
    }

    // UPDATED: Takes an optional second parameter for the button text
    public void ShowStandardMessage(string message, string buttonText = "Close")
    {
        if (standardMessageText != null && standardMessagePanel != null)
        {
            standardMessageText.text = message;
            
            if (standardMessageButtonText != null)
            {
                standardMessageButtonText.text = buttonText;
            }

            standardMessagePanel.SetActive(true);
        }
    }

    public void HideAllPanels()
    {
        if (standardMessagePanel != null) standardMessagePanel.SetActive(false);
        if (keypadPanelScript != null) keypadPanelScript.gameObject.SetActive(false);
    }
}