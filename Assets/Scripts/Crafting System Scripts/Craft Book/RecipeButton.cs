using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RecipeButton : MonoBehaviour
{
    [SerializeField] private RecipeData _recipeData;

    [SerializeField] private Image _itemSprite;
    [SerializeField] private TMP_Text _itemName;

    [SerializeField] private Button _button;
    private void OnValidate()
    {
        if (_recipeData == null)
        {
            Debug.LogWarning("You has not assign the Recipe Data Yet");
            return;
        }

        if (_button == null)
        {            
            _button = gameObject.GetComponent<Button>();
            if(_button == null)
            {
                Debug.LogWarning("Cannot find <Button> Component on this Game Object");
                return;
            }
        }

        _itemSprite.sprite = _recipeData._ResultItem.icon;
        _itemName.text = _recipeData._ResultItem.name;
    }

}
