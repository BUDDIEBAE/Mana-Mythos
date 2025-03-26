/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionCraftingUI : MonoBehaviour
{
    public PotionCraftingSystem potionManager;
    public GameObject craftingPanel;
    public GameObject potionResultPanel;

    [Header("Ingredient Slots")]
    public Button[] ingredientButtons;
    private List<Item> selectedIngredients = new List<Item>();

    [Header("Potion Result")]
    public Text potionNameText;
    public Text potionDescriptionText;

    void Start()
    {
        // Initialize the ingredient buttons (this can be dynamic based on available items)
        foreach (Button button in ingredientButtons)
        {
            button.onClick.AddListener(() => AddIngredient(button));
        }
    }

    public void AddIngredient(Button ingredientButton)
    {
        Item ingredient = ingredientButton.GetComponent<ItemButton>().item;  // Assume ItemButton is a script attached to buttons
        if (!selectedIngredients.Contains(ingredient))
        {
            selectedIngredients.Add(ingredient);
            Debug.Log(ingredient.name + " added to the crafting slots.");
        }
    }

    public void CraftPotion()
    {
        // Send selected ingredients to the PotionManager for crafting
        potionManager.StartBrewing(selectedIngredients);

        // Show the result
        ShowPotionResult();
    }

    private void ShowPotionResult()
    {
        if (potionManager != null && potionManager.currentPotion != null)
        {
            potionResultPanel.SetActive(true);
            potionNameText.text = potionManager.currentPotion.name;
            potionDescriptionText.text = "Effect: " + potionManager.currentPotion.name; // You can add more details about the potion
        }
        else
        {
            potionResultPanel.SetActive(false);
            Debug.Log("Failed to craft a potion.");
        }
    }

    public void ApplyPotion()
    {
        potionManager.ApplyPotionEffect();
    }
}
*/