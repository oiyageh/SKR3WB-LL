using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DrinkOrderSystem : MonoBehaviour
{
    [Header("NPC Variants")]
    public List<GameObject> npcPrefabs;
    public Transform spawnPoint;
    public float spawnInterval = 10f;

    private GameObject currentNPC;
    private NPCExpression npcExpression;
    private bool npcActive = false;

    [Header("Drinks")]
    public List<string> drinks = new List<string> { "Coffee", "Tea", "Juice" };
    private string currentOrder;

    [Header("UI")]
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI reputationText;

    [Header("Reputation")]
    public int reputation = 0;

    void Start()
    {
        orderText.text = "Waiting for customer...";
        UpdateReputationUI();
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (!npcActive)
                SpawnNPC();
        }
    }

    void SpawnNPC()
    {
        GameObject randomNPC = npcPrefabs[Random.Range(0, npcPrefabs.Count)];
        currentNPC = Instantiate(randomNPC, spawnPoint.position, Quaternion.identity);

        npcExpression = currentNPC.GetComponent<NPCExpression>();

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
            orderText.text = "Correct! ";

            if (npcExpression != null)
                npcExpression.SetHappy();
        }
        else
        {
            reputation--;
            orderText.text = "Wrong! ";

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