using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MouseItemData : MonoBehaviour
{
    [Header("Propoties")]
    [SerializeField] public ItemData _currItem;
    [SerializeField] public int _cursorStack;

    [Header("References")]
    [SerializeField] private Image _customCursor;
    [SerializeField] private TextMeshProUGUI _stackText;
    [SerializeField] private CraftingSystem _craftingSystem;
    [SerializeField] private MiniGameController _controlMiniGame;

    private void Awake()
    {
        _craftingSystem = FindObjectOfType<CraftingSystem>(); // Use Find if not on the same object
        _controlMiniGame = FindObjectOfType<MiniGameController>();
        _customCursor.transform.position = Input.mousePosition;
    }

    private void Update()
    {
        if (_customCursor != null)
        {
            _customCursor.transform.position = Input.mousePosition;
            if(_stackText != null)
            {
                _stackText.text = _cursorStack > 1 ? _cursorStack.ToString() : "";
            }
        }

        // Left Mouse Click to Pick or Drop Items
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            HandleLeftClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }

    private void HandleLeftClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) // Avoid clicking through UI
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                Slot slot = result.gameObject.GetComponent<Slot>(); // Check for Crafting Slot
                InventorySlot_UI inventorySlot = result.gameObject.GetComponent<InventorySlot_UI>(); // Check for Inventory Slot

                if (slot != null)
                {
                    OnPlaceItemCraftSlot(slot);
                    return;
                }
                else if (inventorySlot != null)
                {
                    OnLeftClickInventoryItem(inventorySlot);
                    return;
                }
            }
        }
    }

    private void HandleRightClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) // Avoid clicking through UI
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                Slot slot = result.gameObject.GetComponent<Slot>(); // Check for Crafting Slot
                InventorySlot_UI inventorySlot = result.gameObject.GetComponent<InventorySlot_UI>(); // Check for Inventory Slot

                if (slot != null)
                {
                    OnPlaceItemCraftSlot(slot);
                    return;
                }
                else if (inventorySlot != null)
                {
                    OnRightClickInventoryItem(inventorySlot);
                    return;
                }
            }
        }
    }

    private void OnPlaceItemCraftSlot(Slot slot)
    {
        // Picking up an item from a crafting slot (NOT the result slot)
        if(slot._slotIndex != 5)
        {
            if (_currItem == null && slot._itemdata != null)
            {
                _currItem = slot._itemdata;  // Pick up the item
                _cursorStack++;  // Increase held stack count
                slot._itemdata = null;  // Remove from slot
                slot.GetComponent<Image>().sprite = null;  // Clear UI
                
                if(SceneManager.GetActiveScene().name == "Player's Store")
                {
                    _craftingSystem.OnUpdateRecipeID(); // Update recipe system

                }

                _customCursor.gameObject.SetActive(true);
                _customCursor.sprite = _currItem.icon;
                _customCursor.preserveAspect = true;

                Debug.Log($"Picked up item from Crafting Slot. Stack: {_cursorStack}");
            }
            // Dropping an item into an empty crafting slot
            else if (_currItem != null && slot._itemdata == null)
            {

                slot._itemdata = _currItem;  // Place one item from stack
                slot.GetComponent<Image>().sprite = _currItem.icon;  // Update UI
                _cursorStack--;  // Reduce held stack count

                if(SceneManager.GetActiveScene().name == "Player's Store")
                {
                    _craftingSystem.OnUpdateRecipeID(); // Update recipe system
                }

                if (_cursorStack <= 0)
                {
                    _customCursor.gameObject.SetActive(false);
                    _currItem = null;  // If no items left in stack, clear _currItem
                }

                Debug.Log($"Dropped one item into Crafting Slot. Remaining stack: {_cursorStack}");
            }
            // Picking up more of the SAME item from another crafting slot
            else if (_currItem != null && slot._itemdata != null && _currItem == slot._itemdata)
            {
                _cursorStack++; // Increase stack size
                slot._itemdata = null; // Remove the item from slot
                slot.GetComponent<Image>().sprite = null; // Clear UI


                Debug.Log($"Picked up another {_currItem.name}. Stack: {_cursorStack}");
            }

            if(SceneManager.GetActiveScene().name == "Player's Store")
            {
                _craftingSystem.OnUpdateRecipeID(); // Update recipe system
            }
        }
        // Picking up the crafted item from the result slot (Slot Index 5)
        /*if(slot._slotIndex == 5)
        {
            return;

            if (_currItem == null && slot._itemdata != null)
            {
                if (_controlMiniGame._allHitActive) // Only allow picking up if minigame is completed
                {

                    _currItem = slot._itemdata;  // Pick up crafted item
                    _cursorStack = 1;  // Reset stack count to 1
                    slot._itemdata = null;  // Clear crafting slot
                    slot.GetComponent<Image>().sprite = null;  // Clear UI

                    _craftingSystem.OnClearSlot(); // Reset crafting slots
                    _craftingSystem.OnUpdateRecipeID();

                    _customCursor.gameObject.SetActive(true);
                    _customCursor.sprite = _currItem.icon;
                    _customCursor.preserveAspect = true;

                    Debug.Log("Picked up crafted item after minigame");
                }
            }

        }*/
    }

    private void OnLeftClickInventoryItem(InventorySlot_UI inventorySlotUI)
    {
        // Get the actual inventory slot
        InventorySlot inventorySlot = inventorySlotUI.AssignedSlot;

        if (_currItem == null && inventorySlot.ItemData != null)
        {
            // Pick the whole stack from inventory
            _currItem = inventorySlot.ItemData;
            _cursorStack = inventorySlot.StackSize;
            inventorySlot.ClearSlot();

            inventorySlotUI.SetSlot(inventorySlot); // Update UI
            _customCursor.gameObject.SetActive(true);
            _customCursor.sprite = _currItem.icon;
            _customCursor.preserveAspect = true;

            Debug.Log("Picked up item from Inventory");
        }
        else if (_currItem != null) // If holding an item
        {
            if (inventorySlot.ItemData == null) // Drop all items into empty slot
            {
                _customCursor.gameObject.SetActive(false);

                inventorySlot.SetItem(_currItem, _cursorStack);
                _cursorStack = 0;
                _currItem = null;

                inventorySlotUI.SetSlot(inventorySlot); // Update UI
                Debug.Log("Dropped item into Inventory");
            }
            else if (inventorySlot.ItemData == _currItem) // Stack same item
            {
                _customCursor.gameObject.SetActive(false);

                inventorySlot.AddToStack(_cursorStack);
                _cursorStack = 0;
                _currItem = null;

                inventorySlotUI.SetSlot(inventorySlot); // Update UI
                Debug.Log("Stacked item into Inventory");
            }
            else if (inventorySlot.ItemData != _currItem) // Swap items
            {
                Debug.Log("Swapping items");

                ItemData tempItem = inventorySlot.ItemData;
                int tempStack = inventorySlot.StackSize;

                inventorySlot.SetItem(_currItem, _cursorStack);
                _currItem = tempItem;
                _cursorStack = tempStack;

                inventorySlotUI.SetSlot(inventorySlot); // Update UI
            }
        }
    }

    private void OnRightClickInventoryItem(InventorySlot_UI inventorySlotUI)
    {
        InventorySlot inventorySlot = inventorySlotUI.AssignedSlot;

        if (_currItem == null && inventorySlot.ItemData != null) // Pick up one item
        {
            _currItem = inventorySlot.ItemData;

            if (inventorySlot.StackSize > 1)
            {
                inventorySlot.RemoveFromStack(1);
                _cursorStack = 1;
            }
            else // If only one left, take the whole stack
            {
                _cursorStack = inventorySlot.StackSize;
                inventorySlot.ClearSlot();
            }

            inventorySlotUI.SetSlot(inventorySlot); // Update UI
            _customCursor.gameObject.SetActive(true);
            _customCursor.sprite = _currItem.icon;
            _customCursor.preserveAspect = true;

            Debug.Log("Picked up 1 item from Inventory");
        }
        else if (_currItem != null)
        {
            if (inventorySlot.ItemData == _currItem) // Stack 1 item
            {

                inventorySlot.AddToStack(1);
                _cursorStack--;

                if (_cursorStack == 0) // If nothing left in cursor, clear
                {
                    _customCursor.gameObject.SetActive(false);
                    _currItem = null;
                }

                inventorySlotUI.SetSlot(inventorySlot); // Update UI
                Debug.Log("Added 1 item to Inventory");
            }
            else if (inventorySlot.ItemData == null) // Drop 1 item into empty slot
            {

                inventorySlot.SetItem(_currItem, 1);
                _cursorStack--;

                if (_cursorStack == 0) // If nothing left in cursor, clear
                {
                    _customCursor.gameObject.SetActive(false);
                    _currItem = null;
                }

                inventorySlotUI.SetSlot(inventorySlot); // Update UI
                Debug.Log("Dropped 1 item into empty Inventory slot");
            }
        }
    }
}