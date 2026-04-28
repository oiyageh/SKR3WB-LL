using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [Header("Starting Nodes for Different Days")]
    public NPCData FirstNodeDayOne;
    public NPCData FirstNodeDayTwo;
    public NPCData FirstNodeDayThree;
    public NPCData FirstNodeDayFour;

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
    }
}