using UnityEngine;
using System.Collections.Generic;

public class DrinkCup : MonoBehaviour
{
    public List<IngredientData> currentIngredients = new List<IngredientData>();
    public List<DrinkRecipe> recipes;

    public DrinkOrderSystem orderSystem;

    void OnTriggerEnter(Collider other)
    {
        DraggableIngredient ingredient = other.GetComponent<DraggableIngredient>();

        if (ingredient != null)
        {
            AddIngredient(ingredient.data);
            Destroy(other.gameObject);
        }
    }

    void AddIngredient(IngredientData data)
    {
        currentIngredients.Add(data);
        Debug.Log("Added: " + data.ingredientName);
    }

    public void SubmitDrink()
    {
        if (orderSystem == null)
        {
            Debug.LogWarning("No Order System connected!");
            return;
        }

        if (!orderSystem.HasActiveNPC())
        {
            Debug.Log("No NPC to serve.");
            return;
        }

        string result = CheckRecipe();

        Debug.Log("Made Drink: " + result);

        orderSystem.SelectDrink(result);

        currentIngredients.Clear();
    }

    string CheckRecipe()
    {
        foreach (var recipe in recipes)
        {
            if (MatchesRecipe(recipe))
                return recipe.drinkName;
        }

        return "Trash";
    }

    bool MatchesRecipe(DrinkRecipe recipe)
    {
        if (recipe.ingredients.Count != currentIngredients.Count)
            return false;

        foreach (var ingredient in recipe.ingredients)
        {
            if (!currentIngredients.Contains(ingredient))
                return false;
        }

        return true;
    }
}