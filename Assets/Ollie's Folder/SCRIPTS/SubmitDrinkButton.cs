using UnityEngine;

public class SubmitDrinkButton : MonoBehaviour
{
    //smth to submit the drink when done
    public DrinkCup cup;

    public void Submit()
    {
        cup.SubmitDrink();
    }
}