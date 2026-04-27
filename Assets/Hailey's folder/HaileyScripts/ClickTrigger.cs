using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [Header("Starting Nodes for Different Days")]
    public NPCData FirstNodeDayOne;
    public NPCData FirstNodeDayTwo;
    public NPCData FirstNodeDayThree;
    public NPCData FirstNodeDayFour;
    public NPCData FirstNodeDayFive;
    public NPCData FirstNodeDaySix;
    public NPCData FirstNodeDaySeven;

    void OnMouseDown()
    {
        Debug.Log(gameObject.name + " was clicked.");

        //Day One
        if (FirstNodeDayOne.nodeViewed == false && FirstNodeDayTwo.nodeViewed == false)
        {
            GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().StartDialogue(FirstNodeDayOne);
        }
        //Day Two
        if (FirstNodeDayOne.nodeViewed == true && FirstNodeDayTwo.nodeViewed == false)
        {
            GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().StartDialogue(FirstNodeDayTwo);
        }
        //Day Three
        if (FirstNodeDayTwo.nodeViewed == true && FirstNodeDayThree.nodeViewed == false)
        {
            GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().StartDialogue(FirstNodeDayThree);
        }
        //Day Four
        if (FirstNodeDayThree.nodeViewed == true && FirstNodeDayFour.nodeViewed == false)
        {
            GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().StartDialogue(FirstNodeDayFour);
        }
        //Day Five
        if (FirstNodeDayFour.nodeViewed == true && FirstNodeDayFive.nodeViewed == false)
        {
            GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().StartDialogue(FirstNodeDayFive);
        }
        //Day Six
        if (FirstNodeDayFive.nodeViewed == true && FirstNodeDaySix.nodeViewed == false)
        {
            GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().StartDialogue(FirstNodeDaySix);
        }
        //Day Seven
        if (FirstNodeDaySix.nodeViewed == true && FirstNodeDaySeven.nodeViewed == false)
        {
            GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().StartDialogue(FirstNodeDaySeven);
        }
    }
}