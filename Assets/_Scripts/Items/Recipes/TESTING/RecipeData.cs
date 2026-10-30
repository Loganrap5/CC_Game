using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

[CreateAssetMenu(menuName = "Cooking Recipes")] 
public class RecipeData : ScriptableObject
{
    public string recipeName;

    public List<ItemSO> ingredients;

    public ItemSO result;
}
