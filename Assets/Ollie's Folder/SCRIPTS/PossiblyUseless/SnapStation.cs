using UnityEngine;

public class SnapStation : MonoBehaviour
{
    public string requiredTag; // e.g. "Cup", "Ingredient"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;
        }
    }
}       