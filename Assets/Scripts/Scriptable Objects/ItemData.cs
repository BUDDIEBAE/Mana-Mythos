using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory & Crafting System/Item Data")]
public class ItemData : ScriptableObject
{
    public string ID; 
    // 3 Digit Start with 0 is Matherials, 1 is Crafted item 
    
    public string itemName;
    public ItemType itemType;
    [TextArea (4,4)]
    public string description;
    public Sprite icon;
    public int maxStack;
}

public enum ItemType
{
    ArtifactParts,
    Artifact,
    Accessory,
    Metal,
    GemStone,
    Herb,
    Potion
}
/*
[System.Serializable]
public class Potion : ItemData
{
    public string _name => itemName;

    public List<ItemData> _requireIngredients = new List<ItemData>();
    public bool CanCraftPotion(ItemData usedIngredients)
    {
        foreach (var ingredient in _requireIngredients)
        {
            if(!_requireIngredients.Contains(usedIngredients))
            {
                return false;
            }
        }
        return true;
    }
}*/