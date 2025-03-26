using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCraftingSystem : MonoBehaviour
{

    [Header("Slots")]
    public Slot _ingredientSlot;
    public List<Slot> _resultSlots = new List<Slot>();

    [Header("Result Potion")]
    [SerializeField] private ItemData _currPotion;


    [Header("Potions Recipes")]
    public List<PotionRecipeData> _potionsRecipe = new List<PotionRecipeData>();

    [Header("Current Recipe ID")]
    [SerializeField] private string _recipeID = ""; // string part a + string part b
    private PotionMinigameController _minigameController;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            OnClearSlot();
        }
    }



    public void StartBrewing() // When hit a start crafting button
    {
        if(_ingredientSlot._itemdata != null)
        {
            AddIngredientToRecipe(_ingredientSlot._itemdata);
            UpdateResultSlot();

            _ingredientSlot._itemdata = null;
        }
    }

    private void AddIngredientToRecipe(ItemData ingredient)
    {
        if(ingredient != null)
        {
            if(_recipeID == "") // add the first ingredient
            {
                _recipeID = ingredient.ID;
            }
            else // add a second ingredient to 
            {
                _recipeID += ingredient.ID;
            }

            _ingredientSlot._itemdata = ingredient;
        }
        
    }

    private void UpdateResultSlot()
    {
        foreach (Slot resultSlot in _resultSlots)
        {
            // Find matching recipe based on the current recipe ID
            PotionRecipeData matchingRecipe = _potionsRecipe.Find(recipe => recipe._recipeID == _recipeID);

            if (matchingRecipe != null)
            {
                _currPotion = matchingRecipe._resultPotion;
                resultSlot._itemdata = matchingRecipe._resultPotion;  // Update the result slot with the resulting potion
            }
            else // Handle a condition where crafting unsuccessfull here
            {
                Debug.LogWarning("No matching recipe found for this combination of ingredients.");
            }
        }
    }    

    public void OnClearSlot()
    {
        foreach(Slot slot in _resultSlots)
        {
            if(slot._itemdata != null)
            {
                slot._itemdata = null;
            }
        }

        _ingredientSlot._itemdata = null;
        _recipeID = string.Empty;
    }
}