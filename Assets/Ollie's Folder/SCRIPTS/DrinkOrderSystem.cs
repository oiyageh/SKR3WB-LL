using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DrinkOrderSystem : MonoBehaviour
{
    [Header("NPC Variants")]
    public List<GameObject> npcPrefabs;
    public Transform spawnPoint;

    [Header("Spawn Settings")]
    public float spawnInterval = 10f;
    public bool useDebugSpawnSpeed = false;
    public float debugSpawnMultiplier = 5f;

    private float spawnTimer;

    [Header("Systems")]
    private AutoSaveSystem statsSystem;

    private GameObject currentNPC;
    private NPCExpression npcExpression;
    private bool npcActive = false;

    [Header("Drinks")]
    public List<string> drinks = new List<string> { "Coffee", "Latte", "Tea" };
    private string currentOrder;

    [Header("UI")]
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI reputationText;

    [Header("Reputation")]
    public int reputation = 0;

    //npc list for debug
    private List<GameObject> activeNPCs = new List<GameObject>();

    void Awake()
    {
        statsSystem = GetComponent<AutoSaveSystem>();
    }

    void Start()
    {
        orderText.text = "Waiting for customer...";
        UpdateReputationUI();
    }

    void Update()
    {
        HandleNPCSpawning();
    }

    // =========================
    // NEW TIMER SYSTEM (DEBUG SAFE) HOPEFULLLLYYY
    // =========================

    void HandleNPCSpawning()
    {
        if (npcActive) return;

        float interval = spawnInterval;

        if (useDebugSpawnSpeed)
            interval = spawnInterval / debugSpawnMultiplier;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= interval)
        {
            SpawnNPC();
            spawnTimer = 0f;
        }
    }

    public void ClearAllNPCs()
    {
        foreach (GameObject npc in activeNPCs)
        {
            if (npc != null)
                Destroy(npc);
        }

        activeNPCs.Clear(); // important

        currentNPC = null;
        npcActive = false;

        Debug.Log("All NPCs cleared!");
    }

    public void SpawnNPC()
    {
        if (npcPrefabs.Count == 0) return;

        GameObject randomNPC = npcPrefabs[Random.Range(0, npcPrefabs.Count)];
        GameObject npc = Instantiate(randomNPC, spawnPoint.position, Quaternion.identity);

        activeNPCs.Add(npc); // TRACK IT

        currentNPC = npc;
        npcExpression = npc.GetComponent<NPCExpression>();

        currentOrder = drinks[Random.Range(0, drinks.Count)];
        orderText.text = "Order: " + currentOrder;

        npcActive = true;
    }

    public void SelectDrink(string selectedDrink)
    {
        if (!npcActive) return;

        if (selectedDrink == currentOrder)
        {
            reputation++;

            if (statsSystem != null)
                statsSystem.AddCorrectDrink();

            orderText.text = "Correct!";

            if (npcExpression != null)
                npcExpression.SetHappy();
        }
        else
        {
            reputation--;

            if (statsSystem != null)
                statsSystem.AddWrongDrink();

            orderText.text = "Wrong!";

            if (npcExpression != null)
                npcExpression.SetAngry();
        }

        UpdateReputationUI();
        StartCoroutine(NPCLeave());
    }

    IEnumerator NPCLeave()
    {
        yield return new WaitForSeconds(2f);

        Destroy(currentNPC);
        npcActive = false;

        orderText.text = "Waiting for customer...";
    }

    void UpdateReputationUI()
    {
        if (reputationText != null)
            reputationText.text = "Reputation: " + reputation;
    }
}