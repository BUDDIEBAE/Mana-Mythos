using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _inventorySlotPrefab; // Assign in Inspector
    [SerializeField] private Transform _inventoryGrid; // Assign the GridLayout parent

    private List<GameObject> _uiSlots = new List<GameObject>();

    public void PopulateInventory(InventorySystem inventorySystem)
    {
        ClearInventoryUI();
        Debug.Log("Inventory Created");
        foreach (var slot in inventorySystem.InventorySlots)
        {
            GameObject newSlot = Instantiate(_inventorySlotPrefab, _inventoryGrid);
            _uiSlots.Add(newSlot);

            InventorySlot_UI slotUI = newSlot.GetComponent<InventorySlot_UI>();
            if (slotUI != null)
            {
                slotUI.SetSlot(slot);
                //Debug.Log($"Slot No.{slot}");

            }
        }
    }

    private void ClearInventoryUI()
    {
        foreach (GameObject slot in _uiSlots)
        {
            Destroy(slot);
        }
        _uiSlots.Clear();
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += PopulateInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= PopulateInventory;
    }
}
