using System;
using System.Collections;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI lineText;

    public Button choiceButtonPrefab; // prefab for a single choice button
    public Transform choicesContainer;//parent object where choice buttons will spawn

    private NPCData currentNode; //current node we are reading from the Scriptable Object SO
    private int lineIndex; //which line index we currently on, keeping track of the dialogue
    private bool isActive; //are we currently in dialogue?

    public static event Action<NPCData> OnDialogueStart;
    public static event Action<NPCData> OnDialogueEnd;


    /* [Header("Audio")]
     [SerializeField] private AudioClip[] audioClips;
     [SerializeField] private AudioSource DialogueAudioSource;*/
    private void Awake()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        ClearChoices();
        talkSpeed = defaultTalkSpeed;
    }

    private void Update()
    {
        if (!isActive) return; //if no dialogue is active ignore

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && !Speaking)
        {
            if (ChoicesAreShowing()) return; //block only when buttons exist
            Advance();
        }

        if (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame && Speaking)
        {
            talkSpeed = fastTalkSpeed;
        }
        else if (Keyboard.current != null && !Keyboard.current.leftShiftKey.isPressed && Speaking || currentNode != null)
        {
            talkSpeed = defaultTalkSpeed;
        }
    }

    public void StartDialogue(NPCData npcData)
    {
        if (npcData == null)
        {
            Debug.Log("NPC Data is Null");
            return;
        }
        OnDialogueStart?.Invoke(npcData); ;


        //set state
        currentNode = npcData;
        lineIndex = 0;
        isActive = true;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        ShowLine();
    }

    bool HasChoices(NPCData node)//Check the data
    {
        return node != null && node.allChoices != null && node.allChoices.Length > 0;
    }

    void Advance()
    {
        //if node is finished end dialogue
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        //move to next line
        lineIndex++;

        //if we still have lines to read in this node, show the next one
        if (currentNode.lines != null && lineIndex < currentNode.lines.Length)
        {
            if (lineText != null)
            {
                ShowLine();
                return;
            }
        }

        FinishNode();
    }

    void ShowChoices(DialogueChoice[] choices)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        ClearChoices();
        if (choicesContainer == null || choiceButtonPrefab == null)
        {
            Debug.Log("Choices are not wired");
            return;
        }

        foreach (DialogueChoice choice in choices)
        {
            Button bttn = Instantiate(choiceButtonPrefab, choicesContainer);


            TextMeshProUGUI tmp = bttn.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = choice.choiceText;


            NPCData next = choice.nextNode;

            //lambda
            //this is like onclick on our buttins
            //we are saying add a listener when the button is clicked run this function
            bttn.onClick.AddListener(() =>
            {
                Choose(next);
            });
        }
    }
    //are the choices displaying in the UI do we see them on the screen?
    bool ChoicesAreShowing()
    {
        return choicesContainer != null && choicesContainer.childCount > 0;
    }

    void ClearChoices()
    {
        if (choicesContainer == null) return;

        for (int i = choicesContainer.childCount - 1; i >= 0; i--)
        {
            //For every child of the choice Container (which is a button) subtract unil we clear them all.
            Destroy(choicesContainer.GetChild(i).gameObject);
        }
    }

    void EndDialogue()
    {
        isActive = false;
        OnDialogueEnd?.Invoke(currentNode);

        currentNode = null;
        lineIndex = 0;
        ClearChoices();


        //Turn off dialogue Panel
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }
    void ShowLine()
    {
        ClearChoices();
        //if no node then end dialigue
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        if (displayName != null) displayName.text = currentNode.displayName;

        if (currentNode.lines == null || currentNode.lines.Length == 0)
        {
            FinishNode();
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //clamp index so we never go out of bounds
        lineIndex = Mathf.Clamp(lineIndex, 0, currentNode.lines.Length - 1);

        //Show line text
        if (lineText != null) StartCoroutine(ProcessText(currentNode.lines[lineIndex]));
    }
    void Choose(NPCData nextNode)
    {
        //remove buttons asap so UI feels responsive
        ClearChoices();

        //if no next node this choice ends the convo
        if (nextNode == null)
        {
            EndDialogue();
            return;
        }

        currentNode = nextNode;
        lineIndex = 0;
        ShowLine();
    }
    void FinishNode()
    {
        //1. if node exists show choices
        //2. else if next node exists continue automatically
        //else end dialogue
        if (HasChoices(currentNode))
        {
            ShowChoices(currentNode.allChoices);
            return;
        }
        if (currentNode.nextNode != null)
        {

            currentNode = currentNode.nextNode;
            lineIndex = 0;
            ShowLine();
            return;
        }

        EndDialogue();
    }

    private bool Speaking;
    [SerializeField] private float defaultTalkSpeed;
    [SerializeField] private float fastTalkSpeed;
    private float talkSpeed;
    private string currentText;


    //This section is about iterating over every letter of the current line and modifying the result.
    //This section also directly sends text to the textMeshPro.
    IEnumerator ProcessText(string fullString)
    {
        if (!Speaking)
        {

            currentText = fullString;
            Speaking = true;

            for (int i = 1; i <= currentText.Length; i++)
            {

                lineText.text = currentText.Substring(0, Mathf.Clamp(i, 0, currentText.Length));

                yield return new WaitForSeconds(1f / talkSpeed);

            }
            Speaking = false;
        }

    }
    //Checks the current word to see if it contains any of the commands I created and replaces them with the
    //html commands unity understands
    //It also returns the number of letters that should be skipped when iterating over the text,
    //To make the commands not appear as they're being typed out.

}
