using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameDebugPanel : MonoBehaviour
{
    [Header("References")]
    public RealWorldTimeSystem timeSystem;
    public AutoSaveSystem statsSystem;
    public DrinkOrderSystem drinkSystem;

    [Header("Drink System Debug")]
    public DrinkCup cup;
    public List<GameObject> ingredientPrefabs;
    public Transform spawnPoint;
    public List<DrinkRecipe> recipes;

    [Header("UI State")]
    bool showUI = false;
    float timeSpeed = 1f;

    private Vector2 scrollPosition = Vector2.zero;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            showUI = !showUI;

        // Debug time progression
        if (timeSystem != null && timeSystem.useDebugTime)
        {
            timeSystem.debugTime += Time.deltaTime * timeSpeed;

            if (timeSystem.debugTime >= 24f)
                timeSystem.debugTime = 0f;
        }
    }

    void OnGUI()
    {
        if (!showUI) return;

        GUILayout.BeginArea(new Rect(20, 20, 350, 550), GUI.skin.box);
        GUILayout.Label("🛠 GAME DEBUG PANEL");

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(330), GUILayout.Height(520));

        // ================= TIME =================
        if (timeSystem != null)
        {
            GUILayout.Label("⏱ TIME SYSTEM");

            timeSystem.useDebugTime = GUILayout.Toggle(timeSystem.useDebugTime, "Use Debug Time");

            GUILayout.Label("Time: " + timeSystem.debugTime.ToString("F2"));
            timeSystem.debugTime = GUILayout.HorizontalSlider(timeSystem.debugTime, 0f, 24f);

            GUILayout.Label("Speed: " + timeSpeed.ToString("F1"));
            timeSpeed = GUILayout.HorizontalSlider(timeSpeed, 0f, 10f);

            if (GUILayout.Button("🌅 Sunrise")) timeSystem.debugTime = 6f;
            if (GUILayout.Button("☀️ Noon")) timeSystem.debugTime = 12f;
            if (GUILayout.Button("🌇 Sunset")) timeSystem.debugTime = 18f;
            if (GUILayout.Button("🌙 Midnight")) timeSystem.debugTime = 0f;
        }

        GUILayout.Space(10);

        // ================= STATS =================
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

        // ================= NPC =================
        if (drinkSystem != null)
        {
            GUILayout.Label("👥 NPC SYSTEM");

            drinkSystem.useDebugSpawnSpeed =
                GUILayout.Toggle(drinkSystem.useDebugSpawnSpeed, "Fast NPC Spawns");

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

        GUILayout.Space(10);

        // ================= DRINK DEBUG =================
        if (cup != null && drinkSystem != null)
        {
            GUILayout.Label("🍹 DRINK DEBUG");

            // --- Spawn Ingredients ---
            GUILayout.Label("Spawn Ingredients:");
            for (int i = 0; i < ingredientPrefabs.Count; i++)
            {
                if (GUILayout.Button("🧪 " + ingredientPrefabs[i].name))
                {
                    SpawnIngredient(i);
                }
            }

            GUILayout.Space(5);

            // --- Force Orders ---
            GUILayout.Label("Force Orders:");
            for (int i = 0; i < recipes.Count; i++)
            {
                if (GUILayout.Button("🎯 " + recipes[i].drinkName))
                {
                    ForceOrder(i);
                }
            }

            GUILayout.Space(5);

            if (GUILayout.Button("🧹 Clear Cup"))
                ClearCup();

            if (GUILayout.Button("⚡ Auto Complete Order"))
                AutoComplete();
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    // ================= FUNCTIONS =================

    void SpawnIngredient(int index)
    {
        if (index < 0 || index >= ingredientPrefabs.Count) return;

        Vector3 pos = spawnPoint != null
            ? spawnPoint.position
            : Camera.main.transform.position + Camera.main.transform.forward * 3f;

        Instantiate(ingredientPrefabs[index], pos, Quaternion.identity);
    }

    void ForceOrder(int index)
    {
        if (index < 0 || index >= recipes.Count) return;

        if (drinkSystem == null) return;

        string forcedDrink = recipes[index].drinkName;

        // Ensure NPC exists before forcing
        if (!drinkSystem.HasActiveNPC())
            drinkSystem.SpawnNPC();

        drinkSystem.ForceOrder(forcedDrink);
    }

    void ClearCup()
    {
        cup.currentIngredients.Clear();
        Debug.Log("Cup cleared!");
    }

    void AutoComplete()
    {
        if (drinkSystem == null || cup == null) return;

        if (!drinkSystem.HasActiveNPC())
        {
            Debug.Log("No NPC to complete order for!");
            return;
        }

        string order = drinkSystem.GetCurrentOrder();

        foreach (var recipe in recipes)
        {
            if (recipe.drinkName == order)
            {
                cup.currentIngredients.Clear();

                foreach (var ing in recipe.ingredients)
                {
                    cup.currentIngredients.Add(ing);
                }

                cup.SubmitDrink();
                return;
            }
        }

        Debug.Log("No matching recipe found!");
    }

}