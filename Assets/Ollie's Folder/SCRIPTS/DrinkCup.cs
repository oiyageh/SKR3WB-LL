using UnityEngine;
using System.Collections.Generic;

public class DrinkCup : MonoBehaviour
{
    //stores what ingridents were put in your current drink
    public List<IngredientData> currentIngredients = new List<IngredientData>();
    public List<DrinkRecipe> recipes;

    public DrinkOrderSystem orderSystem;

    //when drinks touch the cup it gets added and the object is destroyed
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

    //called when player presses the button
    public void SubmitDrink()
    {
        string result = CheckRecipe();

        Debug.Log("Made Drink: " + result);

        if (orderSystem != null)
        {
            //This connects back to the drink order system and sends thre results to the NPCs
            orderSystem.SelectDrink(result);
        }

        currentIngredients.Clear();
    }
    //checks recipe
    string CheckRecipe()
    {
        foreach (var recipe in recipes)
        {
            if (MatchesRecipe(recipe))
                return recipe.drinkName;
        }

        return "Trash"; // fallback
    }

    bool MatchesRecipe(DrinkRecipe recipe)
    {
        //checks to see if all the ingridents match and its the same amount
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