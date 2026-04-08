using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DrinkRecipe", menuName = "Drink System/Recipe")]
public class DrinkRecipe : ScriptableObject
{
    public string drinkName;
    public List<IngredientData> ingredients;
}