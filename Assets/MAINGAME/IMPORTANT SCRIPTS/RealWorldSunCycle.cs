using UnityEngine;
using System;

public class RealWorldTimeSystem : MonoBehaviour
{
    //debug stuff
    [Header("Debug Time Controls")]
    public bool useDebugTime = false;
    [Range(0f, 24f)] public float debugTime = 12f;
    public float debugSpeed = 1f;
    public bool autoPlayTime = false;


    [Header("Location")]
    //this defines where you are, latitude effects day length while longitude effects time offset of the sun
    public float latitude = 40.7f;
    public float longitude = -74.0f;

    [Header("Sun")]
    //sun is the directional light while sunOffset rotates staring point so sunrise isnt straight overhead
    public Light sun;
    public float sunOffset = -90f;

    [Header("Lighting")]
    //gradient controls the color over time and animariont curve is the brightness,so the brightness will dim the lower the sun is
    public Gradient lightColor;
    public AnimationCurve lightIntensity;
    public Gradient ambientColor;


    //Hdri to make it more pretty
    [Header("HDRI Blending")]
    public Material skyboxBlendMat;

    public Cubemap nightHDRI;
    public Cubemap sunriseHDRI;
    public Cubemap dayHDRI;
    public Cubemap sunsetHDRI;

    //stores calculated sunrise and sunset variables
    float sunriseTime;
    float sunsetTime;

    void Start()
    {
        //this will run once the scen starts and calculates todays sunrise and sunset
        CalculateSunTimes();
    }

    void Update()
    {

        //in every frame it will get realworld time and convert it into a normal day cyle and update the lighting and sun
        float currentTime = GetCurrentTime();
        float timePercent = GetTimePercent(currentTime);

        UpdateSun(timePercent);
        UpdateLighting(timePercent);
        UpdateSkybox(timePercent);

        HandleDebugInput();
    }

    //  Get current local time (0–24)
    float GetCurrentTime()
    {
        if (useDebugTime)
        {
            if (autoPlayTime)
            {
                debugTime += Time.deltaTime * debugSpeed;
                if (debugTime >= 24f) debugTime = 0f;
            }

            return debugTime;
        }

        DateTime now = DateTime.Now;
        return now.Hour + now.Minute / 60f;
    }

    // Convert time into a normalized curve based on sunrise/sunset
    float GetTimePercent(float currentTime)
    {
        //if the time is before sunrise then it maps midnignht then sunrise into 0-0.25
        //keeps night shorter in the lighting curve
        if (currentTime < sunriseTime)
            // Night before sunrise
            return Mathf.InverseLerp(0, sunriseTime, currentTime) * 0.25f;

        if (currentTime < sunsetTime)
            return 0.25f + Mathf.InverseLerp(sunriseTime, sunsetTime, currentTime) * 0.5f;

        return 0.75f + Mathf.InverseLerp(sunsetTime, 24f, currentTime) * 0.25f;
    }

    //coverts the time into rotation 360 = a full day cycle
    void UpdateSun(float t)
    {
        if (sun == null) return;

        float angle = t * 360f;
        sun.transform.rotation = Quaternion.Euler(angle + sunOffset, 170f, 0);

        // pulls values from graident/curves
        sun.color = lightColor.Evaluate(t);
        sun.intensity = lightIntensity.Evaluate(t);
    }

    void UpdateLighting(float t)
    {
        //changes global light based o time
        RenderSettings.ambientLight = ambientColor.Evaluate(t);
    }

    void UpdateSkybox(float t)
    {
        if (t < 0.25f)
        {
            skyboxBlendMat.SetTexture("_TexA", nightHDRI);
            skyboxBlendMat.SetTexture("_TexB", sunriseHDRI);
            skyboxBlendMat.SetFloat("_Blend", t / 0.25f);
        }
        else if (t < 0.5f)
        {
            skyboxBlendMat.SetTexture("_TexA", sunriseHDRI);
            skyboxBlendMat.SetTexture("_TexB", dayHDRI);
            skyboxBlendMat.SetFloat("_Blend", (t - 0.25f) / 0.25f);
        }
        else if (t < 0.75f)
        {
            skyboxBlendMat.SetTexture("_TexA", dayHDRI);
            skyboxBlendMat.SetTexture("_TexB", sunsetHDRI);
            skyboxBlendMat.SetFloat("_Blend", (t - 0.5f) / 0.25f);
        }
        else
        {
            skyboxBlendMat.SetTexture("_TexA", sunsetHDRI);
            skyboxBlendMat.SetTexture("_TexB", nightHDRI);
            skyboxBlendMat.SetFloat("_Blend", (t - 0.75f) / 0.25f);
        }
    }

    //  REAL SUNRISE/SUNSET CALCULATION???? MATHHHHH AUGHHHHHH
    void CalculateSunTimes()
    {
        DateTime now = DateTime.Now;
        //needed because sunrise changes through the yearrrrr woaaaaahhhhhhhhhh
        int day = now.DayOfYear;
        //earth has a rotation of 15 degrees per hour so you convert that to longitude into time
        float lngHour = longitude / 15f;

        sunriseTime = CalculateSunTime(day, true, lngHour);
        sunsetTime = CalculateSunTime(day, false, lngHour);
    }

    float CalculateSunTime(int day, bool sunrise, float lngHour)
    {
        //6 is sunrise guess and 18 sunest guess

        float t = sunrise ? day + ((6 - lngHour) / 24f) : day + ((18 - lngHour) / 24f);
        float M = (0.9856f * t) - 3.289f;

        //position of earth in orbit
        float L = M + 1.916f * Mathf.Sin(M * Mathf.Deg2Rad) + 0.020f * Mathf.Sin(2 * M * Mathf.Deg2Rad) + 282.634f;
        L = Mathf.Repeat(L, 360f);

        float RA = Mathf.Atan(0.91764f * Mathf.Tan(L * Mathf.Deg2Rad)) * Mathf.Rad2Deg;
        RA = Mathf.Repeat(RA, 360f);

        float Lq = Mathf.Floor(L / 90f) * 90f;
        float RAq = Mathf.Floor(RA / 90f) * 90f;
        RA += (Lq - RAq);
        RA /= 15f;

        float sinDec = 0.39782f * Mathf.Sin(L * Mathf.Deg2Rad);
        float cosDec = Mathf.Cos(Mathf.Asin(sinDec));

        float cosH = (Mathf.Cos(90.833f * Mathf.Deg2Rad) - sinDec * Mathf.Sin(latitude * Mathf.Deg2Rad)) /
                     (cosDec * Mathf.Cos(latitude * Mathf.Deg2Rad));

        if (cosH > 1) return 0;
        if (cosH < -1) return 12;

        float H = sunrise ? 360f - Mathf.Acos(cosH) * Mathf.Rad2Deg : Mathf.Acos(cosH) * Mathf.Rad2Deg;
        H /= 15f;

        float T = H + RA - (0.06571f * t) - 6.622f;
        float UT = T - lngHour;

        float localTime = UT + (float)TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalHours;
        return Mathf.Repeat(localTime, 24f);
    }

    void HandleDebugInput()
    {
        if (!useDebugTime) return;

        if (Input.GetKey(KeyCode.RightArrow))
            debugTime += Time.deltaTime * debugSpeed;

        if (Input.GetKey(KeyCode.LeftArrow))
            debugTime -= Time.deltaTime * debugSpeed;

        debugTime = Mathf.Repeat(debugTime, 24f);
    }
}