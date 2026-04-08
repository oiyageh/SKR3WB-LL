using UnityEngine;
using System.Collections.Generic;

//explains what ingridents makes what drink
[CreateAssetMenu(fileName = "DrinkRecipe", menuName = "Drink System/Recipe")]
public class DrinkRecipe : ScriptableObject
{
    public string drinkName;
    public List<IngredientData> ingredients;
}