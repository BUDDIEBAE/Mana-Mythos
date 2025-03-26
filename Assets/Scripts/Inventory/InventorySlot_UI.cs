using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _stackText;
    [SerializeField] private InventorySlot _assignedSlot; // Reference to the inventory slot

    public InventorySlot AssignedSlot => _assignedSlot;

    public InventoryDisplay ParentDisplay { get; private set; }

    // the dis play start with blank inventory slot
    private void Awake()
    {
        ClearSlot();

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();

    }

    public void Init(InventorySlot slot) //
    {
        _assignedSlot = slot;
        UpdateUISlot(slot);
    }

    // Assign the InventorySlot when setting the UI
    public void SetSlot(InventorySlot slot)
    {
        _assignedSlot = slot; // Store the reference

        if (slot.ItemData != null)
        {
            _itemIcon.sprite = slot.ItemData.icon;
            _stackText.text = slot.StackSize > 1 ? slot.StackSize.ToString() : "";
            _itemIcon.enabled = true;
        }
        else
        {
            _itemIcon.enabled = false;
            _stackText.text = "";
        }
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            _itemIcon.sprite = slot.ItemData.icon;
            _itemIcon.color = Color.white;

            if (slot.StackSize > 1) _stackText.text = slot.StackSize.ToString();
            else _stackText.text = "";
        }
        else
        {
            ClearSlot();
        }
    }

    public void UpdateUISlot() // refresh a slot without passing in InventorySlot
    {
        if (_assignedSlot != null) UpdateUISlot( _assignedSlot);
    }


    public void OnUISlotClick()
    {
        //Access display class function
        ParentDisplay?.SlotClick(this);
    }

    public void ClearSlot()
    {
        _assignedSlot?.ClearSlot();
        _itemIcon.sprite = null;
        _itemIcon.color = Color.clear;
        _stackText.text = "";
    }
}
