using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BuyMaterialButton : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public TMP_Text quantityText;

    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity;
    [SerializeField] private int sellPrice;

    [SerializeField] private MerchantManager manager;
    private void OnValidate()
    {
        if (itemData != null)
        {
            iconImage.sprite = itemData.icon;
            sellPrice = itemData.sellPrice;
        }        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left-Click");

            int previousQuantity = quantity;

            quantity = Mathf.Min(quantity + 1 , 10);
            
            if(quantity > previousQuantity)
            {
                manager.UpdateMoneyText(+sellPrice);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            quantity = Mathf.Max(0, quantity - 1);
            manager.UpdateMoneyText(-sellPrice);

        }

        UpdateQuantityUI();
    }

    void UpdateQuantityUI()
    {
        quantityText.text = quantity.ToString();
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public ItemData GetItem()
    {
        return itemData;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        quantity = 0;
        UpdateQuantityUI();
    }
}
