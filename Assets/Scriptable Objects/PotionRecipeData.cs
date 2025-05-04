using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory & Crafting System/Potion Recipe Data")]
public class PotionRecipeData : ScriptableObject
{

    public string _recipeID;
    public ItemData _resultPotion;
    [Header("Ingredients In Recipe")]
    public ItemData[] _requireIngredients = new ItemData[2];


    private void OnValidate()
    {
        _recipeID = string.Empty;

        foreach (ItemData ingredient in _requireIngredients)
        {
            if (ingredient != null)
            {
                _recipeID += ingredient.ID;
            }
            else
            {
                _recipeID += string.Empty;
            }
        }
    }
}
