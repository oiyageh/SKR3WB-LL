using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrinkRespawnManager : MonoBehaviour
{
    [Header("Drink Menu (Assign in Inspector)")]
    public List<GameObject> drinkPrefabs;

    [Header("Spawn Points (Optional: match index or reuse)")]
    public List<Transform> spawnPoints;

    [Header("Respawn Settings")]
    public float respawnDelay = 3f;

    // Tracks active drinks by index
    private Dictionary<int, GameObject> activeDrinks = new Dictionary<int, GameObject>();
    private HashSet<int> respawningIndexes = new HashSet<int>();

    void Start()
    {
        // Spawn all drinks at start
        for (int i = 0; i < drinkPrefabs.Count; i++)
        {
            SpawnDrink(i);
        }
    }

    void Update()
    {
        for (int i = 0; i < drinkPrefabs.Count; i++)
        {
            // If drink is gone and not already respawning
            if ((!activeDrinks.ContainsKey(i) || activeDrinks[i] == null)
                && !respawningIndexes.Contains(i))
            {
                StartCoroutine(RespawnRoutine(i));
            }
        }
    }

    IEnumerator RespawnRoutine(int index)
    {
        respawningIndexes.Add(index);

        Debug.Log($"Drink {index} gone... respawning in {respawnDelay} seconds");

        yield return new WaitForSeconds(respawnDelay);

        SpawnDrink(index);

        respawningIndexes.Remove(index);
    }

    void SpawnDrink(int index)
    {
        if (drinkPrefabs[index] == null)
        {
            Debug.LogWarning($"Drink prefab at index {index} is missing!");
            return;
        }

        Transform spawn = GetSpawnPoint(index);

        GameObject newDrink = Instantiate(
            drinkPrefabs[index],
            spawn.position,
            spawn.rotation
        );

        activeDrinks[index] = newDrink;

        Debug.Log($"Spawned drink {index}: {newDrink.name}");
    }

    Transform GetSpawnPoint(int index)
    {
        // If you provided spawn points, use them
        if (spawnPoints != null && spawnPoints.Count > 0)
        {
            return spawnPoints[Mathf.Clamp(index, 0, spawnPoints.Count - 1)];
        }

        // Fallback: spawn at manager position
        return this.transform;
    }
}