using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class ScheduledCustomer 
{
    public string characterName;

    [Tooltip("Time in 24h format (e.g. 13.5 = 1:30 PM)")]
    [Range(0f, 24f)]
    public float spawnTime;

    public GameObject npcPrefab;

    public string drinkOrder;

    [HideInInspector] public bool hasSpawnedToday = false;
}

public class ScheduledCustomerSystem : MonoBehaviour
{
    [Header("References")]
    public DrinkOrderSystem drinkSystem;
    public RealWorldTimeSystem timeSystem;
    public Transform spawnPoint;

    [Header("Scheduled Characters")]
    public List<ScheduledCustomer> scheduledCustomers = new List<ScheduledCustomer>();

    private int currentDay = -1;

    void Update()
    {
        float currentTime = GetCurrentTime();
        int today = DateTime.Now.DayOfYear;

        // Reset daily spawns
        if (today != currentDay)
        {
            currentDay = today;

            foreach (var customer in scheduledCustomers)
                customer.hasSpawnedToday = false;
        }

        foreach (var customer in scheduledCustomers)
        {
            if (customer.hasSpawnedToday) continue;

            if (currentTime >= customer.spawnTime)
            {
                TrySpawnCustomer(customer);
            }
        }
    }

    float GetCurrentTime()
    {
        if (timeSystem != null && timeSystem.useDebugTime)
            return timeSystem.debugTime;

        DateTime now = DateTime.Now;
        return now.Hour + now.Minute / 60f;
    }

    void TrySpawnCustomer(ScheduledCustomer customer)
    {
        // Don't interrupt active NPC
        if (drinkSystem.HasActiveNPC()) return;

        // Spawn custom NPC
        GameObject npc = Instantiate(
            customer.npcPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        // Inject into DrinkOrderSystem
        drinkSystem.ClearAllNPCs(); // optional safety
        drinkSystem.ForceOrder(customer.drinkOrder);

        customer.hasSpawnedToday = true;

        Debug.Log($"Special Customer Arrived: {customer.characterName} ordered {customer.drinkOrder}");
    }
}