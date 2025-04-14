using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellingSystem : MonoBehaviour
{
    public GameObject tradePanel;
    public Image requestedItemIcon;
    public Button confirmButton;
    public InventoryHolder playerInventoryHolder; // Reference to InventoryHolder, not just InventorySlot
    private NPCBuyer currentNPC;
    [SerializeField] private Slot placedItemSlot; // Change to InventorySlot to reference the slot in inventory

    public void OpenTrade(NPCBuyer npc)
    {
        Debug.Log("Open Trade");
        //placedItemSlot = GetComponent<Slot>();

        currentNPC = npc;
        requestedItemIcon.sprite = npc.requestedItem.icon; // Show the requested item
        tradePanel.SetActive(true);
    }


    /*public void PlaceItem(ItemData item)
    {
        // Find the first Slot containing the item
        placedItemSlot = FindSlot(item);

        if (placedItemSlot != null)
        {
            Debug.Log($"Placed {item.itemName} for trade.");
        }
        else
        {
            Debug.Log("Item not found in inventory.");
        }
    }*/

    public void ConfirmTrade()
    {
        if (placedItemSlot == null || placedItemSlot._itemdata == null)
        {
            Debug.Log("No item placed.");
            return;
        }

        if (!currentNPC.CheckItemMatch(placedItemSlot._itemdata))
        {
            Debug.Log("Incorrect item placed.");
            return;
        }

        Debug.Log($"Sold {placedItemSlot._itemdata.itemName} to {currentNPC.gameObject.name}!");

        // Clear the slot (remove item)
        placedItemSlot._itemdata = null;
        placedItemSlot._image.sprite = null;
        placedItemSlot._image.color = new Color(1f, 1f, 1f, 0f); // Make transparent

        CloseTrade();
    }

    public void CloseTrade()
    {
        Debug.Log("Close Trade");

        tradePanel.SetActive(false);
        placedItemSlot._itemdata = null;
        //placedItemSlot = null;
    }
}
