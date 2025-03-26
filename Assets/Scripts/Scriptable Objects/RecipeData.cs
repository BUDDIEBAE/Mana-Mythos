using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory & Crafting System/Recipe Data")]

public class RecipeData : ScriptableObject
{
    public string _RecipeID;

    public ItemData _ResultItem;

    public ItemData[] _MaterialsInRecipe = new ItemData[4];

    private void OnValidate()
    {
        _RecipeID = string.Empty;

        foreach (ItemData material in _MaterialsInRecipe)
        {
            if(material != null)
            {
                _RecipeID += material.ID;
            }
            else
            {
                _RecipeID += "000";
            }
        }
    }
}
