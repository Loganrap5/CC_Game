using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class CookingStation : MonoBehaviour
{
    private HoldInteraction interaction;
    public Player player;
    public List<RecipeData> recipes = new List<RecipeData>();
    private List<ItemSO> addedIngredients = new List<ItemSO>();


    //Locations for where the ingredients populate on the grill
    public Transform[] ingredientSpots = new Transform[4];

    private GameObject[] spawnedIngredients = new GameObject[4];

    private ItemSO[] ingredientSlots = new ItemSO[4];

    public Transform finishedFoodSpot;
    private GameObject finishedFood;

    private void Start()
    {
        interaction = GetComponent<HoldInteraction>();              //Tons of debugging here so far but it's about 95% functional at this point
        interaction.OnComplete += AddCarriedItem;
    }

    public bool AddIngredient(ItemSO ingredient)
    {
        if (ingredient == null || ingredient.obj == null)
            return false;

        int slot = GetNextAvailableSpot();

        if (slot == -1)
        {
            Debug.Log("The grill is full!");
            return false;
        }

        if (ingredientSpots[slot] == null)
        {
            Debug.LogError($"Ingredient spot {slot} is not assigned!");
            return false;
        }

        GameObject spawned = Instantiate(ingredient.obj, ingredientSpots[slot].position, ingredientSpots[slot].rotation);

        spawned.transform.SetParent(ingredientSpots[slot], true);

        spawnedIngredients[slot] = spawned;
        ingredientSlots[slot] = ingredient;
        addedIngredients.Add(ingredient);

        //Connect the spawned ingredient to this station
        PickupInteraction pickup = spawned.GetComponent<PickupInteraction>();

        if (pickup != null)
        {
            pickup.SetCookingStation(this, slot);
        }
        else
        {
            Debug.LogWarning(
                $"The prefab {ingredient.name} needs a PickupInteraction component."
            );
        }

        UpdateInteractionTime();

        Debug.Log($"Added {ingredient.name} to grill slot {slot}.");

        return true;
    }

    private void AddCarriedItem()
    {
        if (interaction.player == null)
            return;

        if(interaction.player.carriedItem != null)
        {
            Item item = interaction.player.carriedItem.GetComponent<Item>();

            if (item == null)
                return;

            if (AddIngredient(item.itemData))
            {
                //Remove ingredient from player inventory
                interaction.player.carriedItem = null;
            }

            return;
        }

        if (HasCompleteRecipe())
        {
            TryCook(interaction.player);
        }
        else
        {
            Debug.Log("You need a complete recipe before cooking.");
        }

        //TryCook(interaction.player);
        
    }

    private void TryCook(Player currentPlayer)
    {
        if (addedIngredients.Count == 0)
        {
            Debug.Log("No ingredients to cook.");
            return;
        }

        foreach (RecipeData recipe in recipes)
        {
            if (!MatchesRecipe(recipe))
                continue;

            ItemSO result = recipe.result;

            if (result == null || result.obj == null)
            {
                Debug.LogError("Recipe result or prefab is missing: " + recipe.name);
                return;
            }

            if (finishedFoodSpot == null)
            {
                Debug.LogError("Finished Food Spot is not assigned!");
                return;
            }

            //Spawn the finished food on the grill
            finishedFood = Instantiate(
                result.obj,
                finishedFoodSpot.position,
                finishedFoodSpot.rotation
            );

            //Parent it to the finished food spot without changing world scale
            finishedFood.transform.SetParent(finishedFoodSpot, true);

            //Clear all ingredients from the grill
            for (int i = 0; i < spawnedIngredients.Length; i++)
            {
                if (spawnedIngredients[i] != null)
                    Destroy(spawnedIngredients[i]);

                spawnedIngredients[i] = null;
                ingredientSlots[i] = null;
            }

            addedIngredients.Clear();

            // Reset interaction time.
            interaction.requiredTime = 0f;

            Debug.Log("Successfully cooked: " + result.name);
            Debug.Log("Finished food spawned on the grill.");

            return;
        }

        Debug.Log("No matching recipe. Ingredients remain on the grill.");
    }


    private bool MatchesRecipe(RecipeData recipe)
    {
        if (recipe.ingredients.Count != addedIngredients.Count)
            return false;

        List<ItemSO> remaining =
            new List<ItemSO>(addedIngredients);

        foreach (ItemSO required in recipe.ingredients)
        {
            int index = remaining.IndexOf(required);

            if (index == -1)
                return false;

            remaining.RemoveAt(index);
        }

        return remaining.Count == 0;
    }

    private bool HasCompleteRecipe()
    {
        foreach (RecipeData recipe in recipes)
        {
            if (MatchesRecipe(recipe))
                return true;
        }

        return false;
    }

    private int GetNextAvailableSpot()
    {
        for (int i = 0; i < ingredientSpots.Length; i++)
        {
            if (spawnedIngredients[i] == null)
                return i;
        }

        return -1;
    }

    public void RemoveIngredientAt(int slot)
    {
        if (slot < 0 || slot >= spawnedIngredients.Length)
            return;

        ItemSO ingredient = ingredientSlots[slot];

        if (ingredient != null)
        {
            //Remove only one occurrence, allowing duplicate ingredients.
            addedIngredients.Remove(ingredient);
        }

        //The pickup script destroys the actual GameObject.
        spawnedIngredients[slot] = null;
        ingredientSlots[slot] = null;

        UpdateInteractionTime();

        Debug.Log($"Removed ingredient from grill slot {slot}.");
    }

    private void UpdateInteractionTime()
    {
        if (interaction == null)
            return;

        interaction.requiredTime = HasCompleteRecipe() ? 4f : 0f;
    }

}
