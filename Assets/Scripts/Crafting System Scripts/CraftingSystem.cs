using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    [Header("Slots")]
    public Slot[] _slots;
    public Slot _resultSlot;

    [Header("Scriptable Object Data")]
    public RecipeData[] _recipes;
    [SerializeField] private ItemData _resultItemData;

    [SerializeField] private string _recipeID;
    [SerializeField] private bool isMatch = false;

    private MiniGameController miniGameController;

    private void Start()
    {
        miniGameController = GetComponent<MiniGameController>();
    }

    // Starts crafting when the player presses a button
    public void StartCrafting()
    {
        OnCheckRecipeID();
        if (isMatch)
        {
            miniGameController.StartMiniGame();
        }
    }

    // Completes crafting when all green bars are hit
    public void CompleteCrafting()
    {
        Debug.Log("Crafting Completed!");
        InventoryHolder inventoryHolder = FindAnyObjectByType<InventoryHolder>();
        inventoryHolder.InventorySystem.AddItemToInventory(_resultItemData, 1);


        OnClearSlot();
    }
        /*_resultSlot.gameObject.SetActive(true);
        _resultSlot._itemdata = _resultItemData;
        _resultSlot._image.sprite = _resultItemData.icon;
        */

    // Clears the slots and stops the mini-game
    public void OnClearSlot()
    {
        miniGameController.StopMiniGame();
        foreach (var slot in _slots)
        {
            slot._itemdata = null;
        }
        _resultItemData = null;
        _resultSlot._itemdata = null;
        _resultSlot._image.sprite = null;
    }
    public void OnUpdateRecipeID()
    {
        _recipeID = "";

        foreach (Slot slot in _slots)
        {
            if (slot._itemdata != null)
            {
                _recipeID += slot._itemdata.ID;
            }
            else
            {
                _recipeID += "000";
            }
        }
        FindObjectOfType<MiniGameController>()?.SetUpGreenBars();
        OnCheckRecipeID();
    }
    private void OnCheckRecipeID() // Check if the recipe is correct in order to start to mini game
    {
        for (int i = 0; i < _recipes.Length; i++)
        {
            if (_recipeID == _recipes[i]._RecipeID)
            {
                isMatch = true;
                _resultSlot.gameObject.SetActive(true);
                _resultItemData = _recipes[i]._ResultItem;
                _resultSlot._itemdata = _resultItemData;

                if (_resultItemData != null)
                {
                    _resultSlot._image.sprite = _resultItemData.icon;
                }
                return;
            }
        }

        // If no match is found
        isMatch = false;
        _resultItemData = null;
        _resultSlot.gameObject.SetActive(false);
        //miniGameController.StopMiniGame(); // Stop mini-game if no recipe matches
    }
}