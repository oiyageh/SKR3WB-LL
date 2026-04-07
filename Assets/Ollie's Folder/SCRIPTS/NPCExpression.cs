using UnityEngine;

public class NPCExpression : MonoBehaviour
{
    private Renderer npcRenderer;

    [Header("Materials (Per NPC)")]
    public Material normalMaterial;
    public Material happyMaterial;
    public Material angryMaterial;

    void Awake()
    {
        npcRenderer = GetComponentInChildren<Renderer>();

        // Set default look
        if (npcRenderer != null && normalMaterial != null)
            npcRenderer.material = normalMaterial;
    }

    public void SetHappy()
    {
        if (npcRenderer != null && happyMaterial != null)
            npcRenderer.material = happyMaterial;
    }

    public void SetAngry()
    {
        if (npcRenderer != null && angryMaterial != null)
            npcRenderer.material = angryMaterial;
    }
}