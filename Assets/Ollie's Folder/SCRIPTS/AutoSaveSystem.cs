using UnityEngine;
using TMPro;
using System;

public class AutoSaveSystem : MonoBehaviour
{
    [Header("References")]
    public DrinkOrderSystem drinkSystem;

    [Header("UI - HUD")]
    public TextMeshProUGUI drinksMadeText;
    public TextMeshProUGUI lastPlayedText;

    [Header("UI - Stats Menu")]
    public GameObject statsPanel;
    public TextMeshProUGUI statsText;

    private int correctDrinks = 0;
    private int wrongDrinks = 0;

    void Start()
    {
        LoadGame();
        ShowTimeSinceLastPlayed();
        UpdateHUD();
        UpdateStatsMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleStatsMenu();
        }
    }

    // =========================
    // CALLED FROM GAME SYSTEM
    // =========================

    public void AddCorrectDrink()
    {
        correctDrinks++;
        UpdateHUD();
        UpdateStatsMenu();
    }

    public void AddWrongDrink()
    {
        wrongDrinks++;
        UpdateStatsMenu();
    }

    // =========================
    // UI
    // =========================

    void UpdateHUD()
    {
        if (drinksMadeText != null)
            drinksMadeText.text = "Drinks Made: " + correctDrinks;
    }

    void ToggleStatsMenu()
    {
        if (statsPanel == null) return;

        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateStatsMenu();
    }

    void UpdateStatsMenu()
    {
        if (statsText == null || drinkSystem == null) return;

        int total = correctDrinks + wrongDrinks;
        float accuracy = total > 0 ? (float)correctDrinks / total * 100f : 0f;

        statsText.text =
            "=== STATS ===\n" +
            "Reputation: " + drinkSystem.reputation + "\n" +
            "Correct Drinks: " + correctDrinks + "\n" +
            "Wrong Drinks: " + wrongDrinks + "\n" +
            "Accuracy: " + accuracy.ToString("F1") + "%\n" +
            "Total Orders: " + total;
    }

    // =========================
    // SAVE / LOAD
    // =========================

    void OnApplicationQuit()
    {
        SaveGame();
    }

    void SaveGame()
    {
        if (drinkSystem != null)
            PlayerPrefs.SetInt("Reputation", drinkSystem.reputation);

        PlayerPrefs.SetInt("CorrectDrinks", correctDrinks);
        PlayerPrefs.SetInt("WrongDrinks", wrongDrinks);

        string timeNow = DateTime.Now.ToBinary().ToString();
        PlayerPrefs.SetString("LastExitTime", timeNow);

        PlayerPrefs.Save();
    }

    void LoadGame()
    {
        if (drinkSystem != null && PlayerPrefs.HasKey("Reputation"))
            drinkSystem.reputation = PlayerPrefs.GetInt("Reputation");

        correctDrinks = PlayerPrefs.GetInt("CorrectDrinks", 0);
        wrongDrinks = PlayerPrefs.GetInt("WrongDrinks", 0);
    }

    // =========================
    // TIME DISPLAY
    // =========================

    void ShowTimeSinceLastPlayed()
    {
        if (!PlayerPrefs.HasKey("LastExitTime")) return;

        long temp = Convert.ToInt64(PlayerPrefs.GetString("LastExitTime"));
        DateTime lastTime = DateTime.FromBinary(temp);
        TimeSpan diff = DateTime.Now - lastTime;

        string timeText;

        if (diff.TotalMinutes < 60)
            timeText = Mathf.FloorToInt((float)diff.TotalMinutes) + " minutes ago";
        else if (diff.TotalHours < 24)
            timeText = Mathf.FloorToInt((float)diff.TotalHours) + " hours ago";
        else
            timeText = Mathf.FloorToInt((float)diff.TotalDays) + " days ago";

        if (lastPlayedText != null)
            lastPlayedText.text = "Last Played: " + timeText;
    }

    // =========================
    //  DEBUG SYSTEM SUPPORT
    // =========================

    public int GetReputation()
    {
        return drinkSystem != null ? drinkSystem.reputation : 0;
    }

    public void SaveManually()
    {
        SaveGame();
        Debug.Log("Manual Save Complete");
    }

    public void ResetAllStats()
    {
        correctDrinks = 0;
        wrongDrinks = 0;

        if (drinkSystem != null)
            drinkSystem.reputation = 0;

        PlayerPrefs.DeleteKey("Reputation");
        PlayerPrefs.DeleteKey("CorrectDrinks");
        PlayerPrefs.DeleteKey("WrongDrinks");
        PlayerPrefs.DeleteKey("LastExitTime");

        PlayerPrefs.Save();

        UpdateHUD();
        UpdateStatsMenu();

        Debug.Log("All stats reset!");
    }
}