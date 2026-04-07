using UnityEngine;
using System.Collections.Generic;

public class Cup : MonoBehaviour
{
    public List<string> ingredients = new List<string>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            ingredients.Add(other.name);
            Destroy(other.gameObject);

            Debug.Log("Added: " + other.name);
        }
    }
}