using UnityEngine;
using System;
using TMPro;

public class ClockUI : MonoBehaviour
{
    [Header("References")]
    public RealWorldTimeSystem timeSystem;
    public TextMeshProUGUI clockText;

    [Header("Format Settings")]
    public bool use24HourFormat = false;

    public Color canPlayColor = Color.green;
    public Color blockedColor = Color.red;

    void Update()
    {
        if (timeSystem == null || clockText == null) return;

        float time;

        //  Use debug time if active
        if (timeSystem.useDebugTime)
        {
            time = timeSystem.debugTime;
        }
        else
        {
            DateTime now = DateTime.Now;
            time = now.Hour + now.Minute / 60f;
        }
        bool canPlay = (time >= 17f || time < 3f);
        clockText.color = canPlay ? canPlayColor : blockedColor;
        UpdateClockDisplay(time);
    }

    void UpdateClockDisplay(float time)
    {
        int hours = Mathf.FloorToInt(time);
        int minutes = Mathf.FloorToInt((time - hours) * 60f);

        if (use24HourFormat)
        {
            clockText.text = string.Format("{0:00}:{1:00}", hours, minutes);
        }
        else
        {
            string period = hours >= 12 ? "PM" : "AM";

            int displayHour = hours % 12;
            if (displayHour == 0) displayHour = 12;

            clockText.text = string.Format("{0:00}:{1:00} {2}", displayHour, minutes, period);
        }
    }
}
