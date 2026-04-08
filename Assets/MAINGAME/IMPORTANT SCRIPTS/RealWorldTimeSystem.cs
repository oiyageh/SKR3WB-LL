using UnityEngine;
using System;

public class RealWorldSunCycle : MonoBehaviour
{
    [Header("Location Settings")]
    //this defines where you are, latitude effects day length while longitude effects time offset of the sun
   
    [Tooltip("Latitude ")]
    public float latitude = 40.7f;

    [Tooltip("Longitude")]
    public float longitude = -74.0f;

    [Header("Sun")]
    //sun is the directional light while sunOffset rotates staring point so sunrise isnt straight overhead
    public Light sun;
    public float sunOffset = -90f;

    [Header("Lighting")]
    //gradient controls the color over time and animariont curve is the brightness,so the brightness will dim the lower the sun is
    public Gradient lightColor;
    public AnimationCurve lightIntensity;

    [Header("Ambient")]
    //global scene lighting colors
    public Gradient ambientColor;

    //stores calculated sunrise and sunset variables
    private float sunriseTime;
    private float sunsetTime;

    void Start()
    {
       //this will run once the scen starts and calculates todays sunrise and sunset
        CalculateSunTimes();
    }

    void Update()
    {
        //in every frame it will get realworld time and convert it into a normal day cyle and update the lighting and sun
        float currentTime = GetCurrentTime();
        float timePercent = GetDayPercent(currentTime);

        UpdateSun(timePercent);
        UpdateLighting(timePercent);
    }

    //  Get current local time (0–24)
    float GetCurrentTime()
    {
        //converts the time into decimal hours and relates math easier
        DateTime now = DateTime.Now;
        return now.Hour + now.Minute / 60f + now.Second / 3600f;
    }

    // Convert time into a normalized curve based on sunrise/sunset
    float GetDayPercent(float currentTime)
    {
        //if the time is before sunrise then it maps midnignht then sunrise into 0-0.25
        //keeps night shorter in the lighting curve
        if (currentTime < sunriseTime)
        {
            // Night before sunrise
            return Mathf.InverseLerp(0, sunriseTime, currentTime) * 0.25f;
        }
        //maps sunrise and sunset into .25 - .75, main daylight period
        else if (currentTime < sunsetTime)
        {
            // Daytime
            return 0.25f + Mathf.InverseLerp(sunriseTime, sunsetTime, currentTime) * 0.5f;
        }
        //maps sunset into midnight which is .75 - 1 and then restets at 0 and repeats cycle
        else
        {
            // Night after sunset
            return 0.75f + Mathf.InverseLerp(sunsetTime, 24f, currentTime) * 0.25f;
        }
    }

    //coverts the time into rotation 360 = a full day cycle
    void UpdateSun(float timePercent)
    {
        if (sun == null) return;

        float sunAngle = timePercent * 360f;
        sun.transform.rotation = Quaternion.Euler(sunAngle + sunOffset, 170f, 0);
        // pulls values from graident/curves
        sun.color = lightColor.Evaluate(timePercent);
        sun.intensity = lightIntensity.Evaluate(timePercent);
    }

    void UpdateLighting(float timePercent)
    {
        //changes global light based o time
        RenderSettings.ambientLight = ambientColor.Evaluate(timePercent);
    }

    //  REAL SUNRISE/SUNSET CALCULATION
    void CalculateSunTimes()
    {
        DateTime now = DateTime.Now;
        int dayOfYear = now.DayOfYear;
        //needed because sunrise changes through the yearrrrr woaaaaahhhhhhhhhh

        //earth has a rotation of 15 degrees per hour so you convert that to longitude into time
        float lngHour = longitude / 15f;

        sunriseTime = CalculateSunTime(dayOfYear, true, lngHour);
        sunsetTime = CalculateSunTime(dayOfYear, false, lngHour);

        Debug.Log("Sunrise: " + sunriseTime + " | Sunset: " + sunsetTime);
    }

    float CalculateSunTime(int dayOfYear, bool isSunrise, float lngHour)
    {
        float t = isSunrise
            ? dayOfYear + ((6f - lngHour) / 24f)
            : dayOfYear + ((18f - lngHour) / 24f);
        //6 is sunrise guess and 18 sunest guess

        float M = (0.9856f * t) - 3.289f;
        //position of earth in orbit

        float L = M + (1.916f * Mathf.Sin(Deg2Rad(M))) + (0.020f * Mathf.Sin(2 * Deg2Rad(M))) + 282.634f;
        L = Normalize(L, 360f);

        float RA = Rad2Deg(Mathf.Atan(0.91764f * Mathf.Tan(Deg2Rad(L))));
        RA = Normalize(RA, 360f);

        float Lquadrant = Mathf.Floor(L / 90f) * 90f;
        float RAquadrant = Mathf.Floor(RA / 90f) * 90f;
        RA = RA + (Lquadrant - RAquadrant);
        RA /= 15f;

        float sinDec = 0.39782f * Mathf.Sin(Deg2Rad(L));
        float cosDec = Mathf.Cos(Mathf.Asin(sinDec));

        float cosH = (Mathf.Cos(Deg2Rad(90.833f)) - (sinDec * Mathf.Sin(Deg2Rad(latitude))))
                   / (cosDec * Mathf.Cos(Deg2Rad(latitude)));

        if (cosH > 1) return 0;   // no sunrise
        if (cosH < -1) return 12; // no sunset

        float H = isSunrise
            ? 360f - Rad2Deg(Mathf.Acos(cosH))
            : Rad2Deg(Mathf.Acos(cosH));

        H /= 15f;

        float T = H + RA - (0.06571f * t) - 6.622f;

        float UT = T - lngHour;
        UT = Normalize(UT, 24f);

        // Convert to local time
        float localTime = UT + TimeZoneOffset();
        return Normalize(localTime, 24f);
    }

    float TimeZoneOffset()
    {
        return (float)TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalHours;
    }

    float Deg2Rad(float deg) => deg * Mathf.Deg2Rad;
    float Rad2Deg(float rad) => rad * Mathf.Rad2Deg;

    float Normalize(float value, float max)
    {
        value %= max;
        if (value < 0) value += max;
        return value;
    }
}