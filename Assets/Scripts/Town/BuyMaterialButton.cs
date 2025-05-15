using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyMaterialButton : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public Text quantityText;

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
            quantity= Mathf.Max(10, quantity ++);
            manager.moneyToSpend += sellPrice;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            quantity = Mathf.Max(0, quantity - 1);
            manager.moneyToSpend -= sellPrice;

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
}
