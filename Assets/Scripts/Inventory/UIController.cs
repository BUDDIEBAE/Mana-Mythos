using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPanel; // Assign in the Inspector
    [SerializeField] public GameObject _furnancePanel;
    [SerializeField] public GameObject _potionPanel;

    public bool isOpen = false;

    private void Start()
    {
        //_inventoryPanel.SetActive(false); // Ensure inventory is hidden at start
        _furnancePanel.SetActive(false);
        _potionPanel.SetActive(false);
    }

    public void ToggleInventory(InventorySystem inventorySystem)
    {
        isOpen = !isOpen;
        _inventoryPanel.SetActive(isOpen);
        
        if (isOpen)
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested?.Invoke(inventorySystem);
        }
    }

    public void ShowInventory(InventorySystem invSystem)
    {
        if (!_inventoryPanel.activeSelf)
        {
            ToggleInventory(invSystem); // opens it
        }
        else
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested?.Invoke(invSystem); // just refresh
        }
    }

    public void ToggleCrafting()
    {
        isOpen = !isOpen;
        _furnancePanel.SetActive(isOpen);
        {

        }

    }
}
