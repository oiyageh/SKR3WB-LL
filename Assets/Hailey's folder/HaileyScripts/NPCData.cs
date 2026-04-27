using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/NPC Data")]
public class NPCData : ScriptableObject
{

    [Header("Speaker")]
    public string displayName;

    [Header("Dialogue")]
    [TextArea(3, 10)]
    public string[] lines;

    [Header("If there are no choices, we show buttons after line ends")]
    public DialogueChoice[] allChoices;

    [Header("if no choices, auto continue to this next node")]
    public NPCData nextNode;

    [Header("Node viewed?")]
    public bool nodeViewed;

    [Header("Needs drink to be made first?")]
    public bool requiresDrink;

}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public NPCData nextNode;
}