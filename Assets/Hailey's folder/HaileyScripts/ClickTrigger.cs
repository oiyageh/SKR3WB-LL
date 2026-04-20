using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    public NPCData NPCData;

    void OnMouseDown()
    {
        Debug.Log(gameObject.name + " was clicked");
        //GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().StartDialogue(gameObject.GetComponent<NPCData>());

        GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().StartDialogue(NPCData);
    }
}