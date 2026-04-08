using UnityEngine;

public class SubmitDrinkButton : MonoBehaviour
{
    public DrinkCup cup;

    public void Submit()
    {
        cup.SubmitDrink();
    }
}