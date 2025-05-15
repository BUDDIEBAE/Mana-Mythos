using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TradeUIController : MonoBehaviour
{
    public GameObject tradePanel;
    public Image requestedItemIcon;
    public InventoryHolder playerInventoryHolder;

    [Header("Buttons")]
    public Button confirmButton;

    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private TMP_Text _earningText;

    //public Button cancelButton;

    //private NPCBuyer currentNPC;
    //private InventorySlot placedItemSlot;

    [SerializeField] private TradeSystem tradeSystem;
    [SerializeField] private UIController uiController;

    private void Awake()
    {       
        //_buttonText = confirmButton.GetComponentInChildren<TextMeshPro>();

       /* if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancleButtonPress);*/
    }
    private void Update()
    {
        UpdateConfirmButton();
    }

    public void OpenTrade(NPCBuyer npc)
    {
        Debug.Log("Open Trade");

        if (npc.requestedItem != null)
        {
            requestedItemIcon.sprite = npc.requestedItem.icon;
        }
        else
        {
            Debug.LogWarning($"NPC {npc.name} has no requested item!");
            requestedItemIcon.sprite = null; // Or default placeholder
        }

        tradePanel.SetActive(true);
        UpdateConfirmButton();

        RefreshInventoryUI();
    }
    public void CloseTrade()
    {
        Debug.Log("Close Trade");

        tradePanel.SetActive(false);
        RefreshInventoryUI();
    }

    private void UpdateConfirmButton()
    {
        confirmButton.onClick.RemoveAllListeners();

        if (tradeSystem._hasPlaceItem)
        {
            _buttonText.text = "Sold!!";
            //confirmButton.onClick.AddListener(OnConfirmButtonPressed);

            if (tradeSystem.TryGetItemPrice(tradeSystem.GetPlacedItem(), out int price))
            {
                _earningText.text = price.ToString();
            }
            else
            {
                _earningText.text = "0";
            }
        }
        else
        {
            _buttonText.text = "Don't have that!!";
            _earningText.text = "";
            //confirmButton.onClick.AddListener(OnCancelButtonPress);
        }
    }

    public void ButtonFunctionChange()
    {
        if(tradeSystem._hasPlaceItem)
        {
            OnConfirmButtonPressed();
        }
        else
        {
            OnCancelButtonPress();
        }
    }

    private void OnConfirmButtonPressed()
    {
        tradePanel.SetActive(false);

        tradeSystem.ConfirmTrade();
        RefreshInventoryUI();
    }

    private void OnCancelButtonPress()
    {
        tradePanel.SetActive(false);

        tradeSystem.CancleTrade();
        RefreshInventoryUI();
    }


    private void RefreshInventoryUI()
    {
        InventorySystem holder = playerInventoryHolder.InventorySystem;
        InventoryHolder.OnDynamicInventoryDisplayRequested?.Invoke(holder);
        uiController.ToggleInventory(holder); // Add this line back
    }
}
