using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;


    public List<InventorySlot> InventorySlots => inventorySlots;

    public int InventorySize => inventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChange;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }

    }

    public bool AddItemToInventory(ItemData itemToAdd, int amountToAdd)
    {
        if(ContainsItem(itemToAdd, out List<InventorySlot> Slots)) // Check whether item exists in inventory
        {
            foreach (var slot in Slots)
            {
                if(slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChange?.Invoke(slot);


                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot))   // Get the first available slot
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChange?.Invoke(freeSlot);

            return true;
        }

        return false;

    }

    public bool ContainsItem(ItemData itemToAdd, out List<InventorySlot> Slots)
    {
        Slots = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();

        return Slots == null ? false : true;

    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }

}
