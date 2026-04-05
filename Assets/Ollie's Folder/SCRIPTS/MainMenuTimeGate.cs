using UnityEngine;
using System;

public class MainMenuTimeGate : MonoBehaviour
{
    [Header("References")]
    public RealWorldTimeSystem timeSystem;

    [Header("UI")]
    public GameObject mainMenuUI;
    public GameObject blockedUI;

    [Header("Allowed Time Window")]
    public float startHour = 17f; // 5 PM
    public float endHour = 3f;    // 3 AM

    void Update()
    {
        CheckTime();
    }

    void CheckTime()
    {
        float currentTime;

        // ✅ Use debug time if enabled
        if (timeSystem != null && timeSystem.useDebugTime)
        {
            currentTime = timeSystem.debugTime;
        }
        else
        {
            DateTime now = DateTime.Now;
            currentTime = now.Hour + now.Minute / 60f;
        }

        // Handles time range that crosses midnight
        bool canPlay = (currentTime >= startHour || currentTime < endHour);

        // Toggle UI
        mainMenuUI.SetActive(canPlay);
        blockedUI.SetActive(!canPlay);
    }
}