using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Drink System/Ingredient")]
public class IngredientData : ScriptableObject
{
    //instead of hardcoading smth like "milk" i can make reusuable assets in unity, 
    //not a gameobject but a data definition
    public string ingredientName;
}