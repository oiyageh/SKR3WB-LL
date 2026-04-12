using UnityEngine;

public class SubmitDrinkButton : MonoBehaviour
{
    public DrinkCup cup;

    public void Submit()
    {
        if (cup != null)
            cup.SubmitDrink();
        else
            Debug.LogWarning("No cup assigned!");
    }
}