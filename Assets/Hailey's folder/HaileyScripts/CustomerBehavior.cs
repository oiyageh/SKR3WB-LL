using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using System.Collections;

public class CustomerBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [Header("Player Interacitons")]
    public bool correctDrink;
    public Canvas dialogueCanvas;
    public TextMeshProUGUI dialogueBox;
    public Canvas optionsBox;
    private Drink requestedDrink;

  

    private bool delayOver = false;


    private void OnMouseDown()
    {
        requestedDrink = ScriptableObject.CreateInstance<Drink>();
        requestedDrink.RandomDrink();
        dialogueCanvas.enabled = true;
        dialogueBox.text = "Good evening. I'd like to order a " + requestedDrink.drinkName;
        StartCoroutine(Delay());
        if (delayOver)
        {
            optionsBox.enabled = true;
            if (correctDrink)
            {
                dialogueBox.text = "Just what I ordered. Thank you!";
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().reputation += 5;
                Destroy(gameObject);
            }
            if (!correctDrink)
            {
                dialogueBox.text = "This isn't what I ordered...";
                Destroy(gameObject);
            }

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        delayOver = true;
    }
}
