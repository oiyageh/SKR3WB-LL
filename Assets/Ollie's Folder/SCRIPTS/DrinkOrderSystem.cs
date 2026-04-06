using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DrinkOrderSystem : MonoBehaviour
{
    [Header("NPC")]
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 120f; // every 2 minutes

    [Header("UI")]
    public TextMeshProUGUI orderText;

    [Header("Drinks")]
    public List<string> drinks = new List<string> { "Coffee", "Tea", "Juice" };

    private string currentOrder;
    private GameObject currentNPC;

    [Header("Reputation")]
    public int reputation = 0;

    private bool npcActive = false;

    void Start()
    {
        StartCoroutine(SpawnNPCLoop());
    }

    IEnumerator SpawnNPCLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (!npcActive)
            {
                SpawnNPC();
            }
        }
    }

    void SpawnNPC()
    {
        currentNPC = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);

        currentOrder = drinks[Random.Range(0, drinks.Count)];
        npcActive = true;

        orderText.text = "Order: " + currentOrder;
    }

    // Call this from UI buttons
    public void PlayerSelectDrink(string selectedDrink)
    {
        if (!npcActive) return;

        if (selectedDrink == currentOrder)
        {
            Debug.Log("Correct drink!");
            reputation += 1;
            StartCoroutine(NPCLeave("happy"));
        }
        else
        {
            Debug.Log("Wrong drink!");
            reputation -= 1;
            StartCoroutine(NPCLeave("angry"));
        }
    }

    IEnumerator NPCLeave(string mood)
    {
        orderText.text = "Customer leaving...";
        orderText.text = "Waiting for customer...";

        // You can replace this with animations later
        Debug.Log("NPC is " + mood);

        yield return new WaitForSeconds(2f);

        Destroy(currentNPC);
        npcActive = false;
    }
}