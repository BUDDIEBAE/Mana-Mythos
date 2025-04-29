using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[System.Serializable]
public class InventoryHolder : Singleton<InventoryHolder>
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;
    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    [SerializeField] private List<ItemData> itemToAdd = new List<ItemData>();


    protected int money;

    public int Money => money;


    public override void Awake()
    {
        base.Awake();


        inventorySystem = new InventorySystem(inventorySize);
        OnDynamicInventoryDisplayRequested?.Invoke(inventorySystem);
    }

    private void Start() // use for debugging game
    {
        foreach (ItemData item in itemToAdd)
        {
            InventorySystem.AddItemToInventory(item, item.maxStack);
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
    }
}

