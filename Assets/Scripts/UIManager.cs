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
    [SerializeField] private BinaryKeypadPanel binaryKeypadPanelScript;

    private void OnEnable()
    {
        if (gameManager != null) gameManager.GameStateChanged += HandleGameStateChanged;

        if (collectibles != null)
        {
            collectibles.StatusChanged += UpdateStatus;
            collectibles.CollectibleCollected += HandleItemCollected;
        }

        if (playerStats != null) playerStats.StatsChanged += UpdateStats;
        if (keypadPanelScript != null) keypadPanelScript.OnKeypadSolved += HandleKeypadSolved; 
        if (binaryKeypadPanelScript != null) binaryKeypadPanelScript.OnBinarySolved += HandleBinarySolved;
    }

    private void Start()
    {
        UpdateStats();
        if (statusText != null) statusText.text = "Collectibles: Ready";
    }

    private void OnDisable()
    {
        if (gameManager != null) gameManager.GameStateChanged -= HandleGameStateChanged;

        if (collectibles != null)
        {
            collectibles.StatusChanged -= UpdateStatus;
            collectibles.CollectibleCollected -= HandleItemCollected;
        }

        if (playerStats != null) playerStats.StatsChanged -= UpdateStats;
        if (keypadPanelScript != null) keypadPanelScript.OnKeypadSolved -= HandleKeypadSolved;
        if (binaryKeypadPanelScript != null) binaryKeypadPanelScript.OnBinarySolved -= HandleBinarySolved;
    }

    public void ResetStats()
    {
        playerStats.ResetAllProgressInThisApp();
        countdownTimer.ResetTimer();
        HideAllPanels(); 
        if (statusText != null) statusText.text = "Collectibles: Ready";
    }

    private void UpdateStatus(string status)
    {
        if (statusText != null) statusText.text = $"Collectibles: {status}";
    }

    private void UpdateStats()
    {
        if (playerStats == null) return;
        if (collectedText != null) collectedText.text = $"Found: {playerStats.CollectedCount} / 4";
        if (pointsText != null) pointsText.gameObject.SetActive(false);
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (statusText == null) return;
        if (state == GameManager.GameState.Victory) statusText.text = "<color=green>MISSION ACCOMPLISHED!</color>";
        else if (state == GameManager.GameState.Defeat) statusText.text = "<color=red>TIME'S UP! MISSION FAILED.</color>";
    }

    private void HandleItemCollected(string collectibleId)
    {
        HideAllPanels();

        switch (collectibleId)
        {
            case "first_marker":
                // PHASE 1 START
                keypadPanelScript.gameObject.SetActive(true); 
                break;
                
            case "politechnika_logo":
                // PHASE 2
                if (countdownTimer != null) countdownTimer.OverrideTimer(300f);
                ShowStandardMessage("You actually tapped it?? Now, you triggered my defence system. You have now 5 MINUTES to disable the timer before i dont know what happens.\n\nIf you want to stop the timer find the Internet of things. Good luck.", "Find the IoT Wall");
                break;
                
            case "iot_wall":
                // PHASE 3 START
                binaryKeypadPanelScript.gameObject.SetActive(true);
                break;
                
            case "final_marker":
                // PHASE 4 FINALE
                if (countdownTimer != null) countdownTimer.StopTimer();
                ShowStandardMessage("Impressisve; You escaped the chamber and contained the Cloud. Few subjects have made it this far: Unfortunately for you, this changes nothing. While you fought for survival, Skynet escqped and the next phase has already begun. The  world remains vulnerable. Await further instructions:", "End Simulation");
                break;
        }
    }

    // --- PANEL CONTROLS ---

    private void HandleKeypadSolved()
    {
        // PHASE 1 SUCCESS
        HideAllPanels();
        ShowStandardMessage("Wow, you can read numbers. Outstanding.\n\nDon’t go standing infront of the Politechnika Krakowska logo. Seriously. There is absolutley nothing for you to see there. Do not look at it!", "Continue");
    }

    private void HandleBinarySolved()
    {
        // PHASE 3 SUCCESS
        HideAllPanels();
        
        if (countdownTimer != null) 
        {
            countdownTimer.StopTimer();
            countdownTimer.ResetTimerColor();
        }

        ShowStandardMessage("Timer is disabled.", "Proceed");
    }

    public void ShowStandardMessage(string message, string buttonText = "Close")
    {
        if (standardMessageText != null && standardMessagePanel != null)
        {
            standardMessageText.text = message;
            if (standardMessageButtonText != null) standardMessageButtonText.text = buttonText;
            standardMessagePanel.SetActive(true);
        }
    }
    
    public void HideAllPanels()
    {
        if (standardMessagePanel != null) standardMessagePanel.SetActive(false);
        if (keypadPanelScript != null) keypadPanelScript.gameObject.SetActive(false);
        if (binaryKeypadPanelScript != null) binaryKeypadPanelScript.gameObject.SetActive(false);
    }
}