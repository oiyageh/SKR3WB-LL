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

    void HandleNPCSpawning()
    {
        if (npcActive) return;

        float interval = useDebugSpawnSpeed
            ? spawnInterval / debugSpawnMultiplier
            : spawnInterval;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= interval)
        {
            SpawnNPC();
            spawnTimer = 0f;
        }
    }

    public void SpawnNPC()
    {
        if (npcPrefabs.Count == 0) return;

        GameObject npc = Instantiate(
            npcPrefabs[Random.Range(0, npcPrefabs.Count)],
            spawnPoint.position,
            Quaternion.identity
        );

        activeNPCs.Add(npc);

        currentNPC = npc;
        npcExpression = npc.GetComponent<NPCExpression>();

        currentOrder = drinks[Random.Range(0, drinks.Count)];
        orderText.text = "Order: " + currentOrder;

        npcActive = true;
    }

    public void SelectDrink(string selectedDrink)
    {
        if (!npcActive) return;

        bool correct = selectedDrink == currentOrder;

        // Handle trash or wrong drinks
        if (selectedDrink == "Trash")
            correct = false;

        if (correct)
        {
            reputation++;

            if (statsSystem != null)
                statsSystem.AddCorrectDrink();

            orderText.text = "Correct!";
            npcExpression?.SetHappy();
        }
        else
        {
            reputation--;

            if (statsSystem != null)
                statsSystem.AddWrongDrink();

            orderText.text = "Wrong!";
            npcExpression?.SetAngry();
        }

        UpdateReputationUI();
        StartCoroutine(NPCLeave());
    }

    IEnumerator NPCLeave()
    {
        yield return new WaitForSeconds(2f);

        if (currentNPC != null)
            Destroy(currentNPC);

        npcActive = false;
        currentNPC = null;

        orderText.text = "Waiting for customer...";
    }

    void UpdateReputationUI()
    {
        if (reputationText != null)
            reputationText.text = "Reputation: " + reputation;
    }

    public bool HasActiveNPC()
    {
        return npcActive && currentNPC != null;
    }

    public string GetCurrentOrder()
    {
        if (!npcActive)
            return "";

        return currentOrder;
    }

    public void ClearAllNPCs()
    {
        foreach (GameObject npc in activeNPCs)
        {
            if (npc != null)
                Destroy(npc);
        }

        activeNPCs.Clear();

        currentNPC = null;
        npcActive = false;

        currentOrder = "";

        if (orderText != null)
            orderText.text = "Waiting for customer...";

        Debug.Log("All NPCs cleared!");
    }



    public void ForceOrder(string newOrder)
    {
        // Ensure NPC exists
        if (!npcActive)
        {
            SpawnNPC();
        }

        currentOrder = newOrder;

        if (orderText != null)
            orderText.text = "Order: " + currentOrder;

        Debug.Log("Forced Order: " + currentOrder);
    }


}