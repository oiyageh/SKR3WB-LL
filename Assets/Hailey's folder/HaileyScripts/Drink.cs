using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Drink", menuName = "Scriptable Objects/Drink")]
public class Drink : ScriptableObject
{
    public string drinkName;
    public List<string> drinkType = new List<string>();

    public void RandomDrink()
    {
        drinkType.Add("Martini");
        drinkType.Add("Rum and Coke");
        drinkType.Add("Strawberry Daiquiri");
        drinkType.Add("Vodka Soda");
        drinkType.Add("Oil");
        drinkType.Add("Manhattan");
        drinkName = drinkType[Random.Range(0, drinkType.Count)];
    }
}
