using UnityEngine;

public class TimeDebugUI : MonoBehaviour
{
    public RealWorldTimeSystem timeSystem;

    bool showUI = false;

    float timeSpeed = 1f;

    void Update()
    {
        // Toggle UI
        if (Input.GetKeyDown(KeyCode.F1))
            showUI = !showUI;

        // Speed up debug time
        if (timeSystem.useDebugTime)
        {
            timeSystem.debugTime += Time.deltaTime * timeSpeed;

            if (timeSystem.debugTime >= 24f)
                timeSystem.debugTime = 0f;
        }
    }

    void OnGUI()
    {
        if (!showUI || timeSystem == null) return;

        GUILayout.BeginArea(new Rect(20, 20, 300, 300), GUI.skin.box);

        GUILayout.Label("⏱ TIME DEBUG PANEL");

        // Toggle real vs debug
        timeSystem.useDebugTime = GUILayout.Toggle(timeSystem.useDebugTime, "Use Debug Time");

        // Time slider
        GUILayout.Label("Time: " + timeSystem.debugTime.ToString("F2"));
        timeSystem.debugTime = GUILayout.HorizontalSlider(timeSystem.debugTime, 0f, 24f);

        // Speed control
        GUILayout.Label("Speed: " + timeSpeed.ToString("F1"));
        timeSpeed = GUILayout.HorizontalSlider(timeSpeed, 0f, 10f);

        GUILayout.Space(10);

        // Quick presets to switch between the times
        GUILayout.Label("Quick Set:");

        if (GUILayout.Button("🌅 Sunrise")) timeSystem.debugTime = 6f;
        if (GUILayout.Button("☀️ Noon")) timeSystem.debugTime = 12f;
        if (GUILayout.Button("🌇 Sunset")) timeSystem.debugTime = 18f;
        if (GUILayout.Button("🌙 Midnight")) timeSystem.debugTime = 0f;

        GUILayout.EndArea();
    }
}