using UnityEngine;
using TMPro;
using System;

public class GameDebugPanel : MonoBehaviour
{
    [Header("References")]
    public RealWorldTimeSystem timeSystem;
    public AutoSaveSystem statsSystem;
    public DrinkOrderSystem npcSpawner;
    public DrinkOrderSystem drinkSystem;

    [Header("UI State")]
    bool showUI = false;
    float timeSpeed = 1f;

    void Update()
    {
        // Toggle debug panel
        if (Input.GetKeyDown(KeyCode.F1))
            showUI = !showUI;

        if (timeSystem != null)
        {
            // Manual debug time progression
            if (timeSystem.useDebugTime)
            {
                timeSystem.debugTime += Time.deltaTime * timeSpeed;

                if (timeSystem.debugTime >= 24f)
                    timeSystem.debugTime = 0f;
            }
        }
    }

    void OnGUI()
    {
        if (!showUI) return;

        // Start outer area
        GUILayout.BeginArea(new Rect(20, 20, 350, 500), GUI.skin.box);
        GUILayout.Label("🛠 GAME DEBUG PANEL");

        // Start scroll view
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(330), GUILayout.Height(470));

        // ================= TIME SYSTEM =================
        if (timeSystem != null)
        {
            GUILayout.Label("⏱ TIME SYSTEM");

            timeSystem.useDebugTime = GUILayout.Toggle(timeSystem.useDebugTime, "Use Debug Time");

            GUILayout.Label("Time: " + timeSystem.debugTime.ToString("F2"));
            timeSystem.debugTime = GUILayout.HorizontalSlider(timeSystem.debugTime, 0f, 24f);

            GUILayout.Label("Speed: " + timeSpeed.ToString("F1"));
            timeSpeed = GUILayout.HorizontalSlider(timeSpeed, 0f, 10f);

            GUILayout.Space(5);

            GUILayout.Label("Quick Time Presets:");
            if (GUILayout.Button("🌅 Sunrise")) timeSystem.debugTime = 6f;
            if (GUILayout.Button("☀️ Noon")) timeSystem.debugTime = 12f;
            if (GUILayout.Button("🌇 Sunset")) timeSystem.debugTime = 18f;
            if (GUILayout.Button("🌙 Midnight")) timeSystem.debugTime = 0f;
        }

        GUILayout.Space(10);

        // ================= STATS SYSTEM =================
        if (statsSystem != null)
        {
            GUILayout.Label("📊 STATS SYSTEM");

            GUILayout.Label("Reputation: " + statsSystem.GetReputation());

            if (GUILayout.Button("🔄 RESET ALL STATS"))
                statsSystem.ResetAllStats();

            if (GUILayout.Button("💾 FORCE SAVE"))
                statsSystem.SaveManually();
        }

        GUILayout.Space(10);

        // ================= NPC SPAWNER =================
        if (drinkSystem != null)
        {
            GUILayout.Label("👥 NPC SPAWNER");

            drinkSystem.useDebugSpawnSpeed =
                GUILayout.Toggle(drinkSystem.useDebugSpawnSpeed, "Use Fast NPC Spawns");

            GUILayout.Label("Spawn Interval: " + drinkSystem.spawnInterval.ToString("F1"));
            drinkSystem.spawnInterval =
                GUILayout.HorizontalSlider(drinkSystem.spawnInterval, 1f, 30f);

            GUILayout.Label("Speed Multiplier: " + drinkSystem.debugSpawnMultiplier.ToString("F1"));
            drinkSystem.debugSpawnMultiplier =
                GUILayout.HorizontalSlider(drinkSystem.debugSpawnMultiplier, 1f, 20f);

            if (GUILayout.Button("⚡ Instant Spawn"))
                drinkSystem.SpawnNPC();

            if (GUILayout.Button("🧹 Clear NPCs"))
                drinkSystem.ClearAllNPCs();


        }

        // End scroll view
        GUILayout.EndScrollView();

        // End outer area
        GUILayout.EndArea();
    }

    // Add this at the top of your class
    private Vector2 scrollPosition = Vector2.zero;
}