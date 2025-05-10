using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftBook : MonoBehaviour
{
    [SerializeField] private GameObject _furnanceCraftBookPanel;
    [SerializeField] private GameObject _potionCraftbookPanel;

    [Header("Display Info in Furnance Craft Book")]
    [SerializeField] private Image _spriteImage;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDescription;
    [SerializeField] private List<Image> _craftMaterials = new List<Image>(4); // use to display Image
    [SerializeField] private List<ItemData> _materialToDisplay = new List<ItemData>(4);
    public void CraftBookButton(int index) // 1 is Furnance, 2 is Potion
    {
        if(index == 1)
        {
            _furnanceCraftBookPanel.SetActive(true);
            _potionCraftbookPanel.SetActive(false);
        }
        else if(index == 2)
        {
            _potionCraftbookPanel.SetActive(true);
            _furnanceCraftBookPanel.SetActive(false);
        }
    }

    public void populateCraftBookMaterial(RecipeData data)
    {
        for(int i = 0; i < _craftMaterials.Count; i++) 
        {
            if(i < data._MaterialsInRecipe.Length && data._MaterialsInRecipe != null)
            {
                _materialToDisplay[i] = data._MaterialsInRecipe[i];
                _craftMaterials[i].sprite = data._MaterialsInRecipe[i].icon;
                _craftMaterials[i].gameObject.SetActive(true);
            }
            else
            {
                _materialToDisplay[i] = null;
                _craftMaterials[i].sprite = null;
                _craftMaterials[i].gameObject.SetActive(true);
            }
        }

        _itemDescription.text = data._ResultItem.description;
        _spriteImage.sprite = data._ResultItem.icon;
        _itemName.text = data._ResultItem.name;
    }

}
