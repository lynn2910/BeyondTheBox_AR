using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private const string CollectedCountKey = "player_collected_count";

    public int CollectedCount { get; private set; }

    public event Action StatsChanged;

    private void Awake()
    {
        Load();
    }

    public void Load()
    {
        CollectedCount = PlayerPrefs.GetInt(CollectedCountKey, 0);
        StatsChanged?.Invoke();
    }

    public bool IsCollected(string collectibleId)
    {
        if (string.IsNullOrWhiteSpace(collectibleId)) return false;
        return PlayerPrefs.GetInt(GetCollectedKey(collectibleId), 0) == 1;
    }

    // Removed points parameter
    public bool Collect(string collectibleId)
    {
        if (string.IsNullOrWhiteSpace(collectibleId) || IsCollected(collectibleId))
        {
            return false;
        }

        PlayerPrefs.SetInt(GetCollectedKey(collectibleId), 1);
        CollectedCount++;
        PlayerPrefs.SetInt(CollectedCountKey, CollectedCount);
        PlayerPrefs.Save();

        StatsChanged?.Invoke();
        Debug.Log($"PlayerStats: collected '{collectibleId}'. Total collected: {CollectedCount}.");
        return true;
    }

    public void ResetAllProgressInThisApp()
    {
        PlayerPrefs.DeleteAll();
        CollectedCount = 0;
        PlayerPrefs.Save();
        StatsChanged?.Invoke();
        Debug.Log("PlayerStats: progress reset.");
    }

    private static string GetCollectedKey(string collectibleId)
    {
        return $"collected_{collectibleId}";
    }
}