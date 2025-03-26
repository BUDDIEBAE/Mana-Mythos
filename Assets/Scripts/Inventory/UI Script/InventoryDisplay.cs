using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData _mouseItemData;

    protected InventorySystem _inventorySystem;
    public InventorySystem InventorySystem => _inventorySystem;

    protected Dictionary<InventorySlot_UI, InventorySlot> _slotDictionary;

    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => _slotDictionary;

    public abstract void AssignSlot(InventorySystem inventoryToDisplay);
    
    protected virtual void Start()
    {

    }
    

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary) //Loop through every slot in Dictionary in order to update the ui if the value that we pass in coresponse to the ui slot
        {
            if(slot.Value == updatedSlot)
            {
                slot.Key.UpdateUISlot(updatedSlot); // Slot Key - the UI representation of the value
            }
        }
    }

    public void SlotClick(InventorySlot_UI clickedSlot)
    {
        Debug.Log($"Slot{clickedSlot.gameObject.name} Clicked");
    }




}
