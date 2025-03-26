using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int stackSize;

    public ItemData ItemData => itemData;
    public int StackSize => stackSize;
    
    public InventorySlot(ItemData source, int amount)
    {
        itemData= source;
        stackSize= amount;
    }
    public InventorySlot()
    {
        ClearSlot();
    }
    public void ClearSlot()
    {
        itemData= null;
        stackSize= -1;   
    }
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }
    
    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

    public void SetItem(ItemData itemData, int amount)
    {
        this.itemData = itemData;
        stackSize = amount;
    }

    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining) //use when spliting or combinding a stack
    {
        amountRemaining = itemData.maxStack - stackSize;
        return RoomLeftInStack(amountToAdd);
    }
    public bool RoomLeftInStack(int amountToAdd) //true when there are enough room in stack
    {
        if(stackSize + amountToAdd <= itemData.maxStack) return true;
        else return false;
    }

    public void UpdateInventorySlot(ItemData data, int amount)
    {
        itemData = data;
        stackSize = amount;
    }
}
