using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class TradeUIController : MonoBehaviour
{
    public GameObject tradePanel;
    public Image requestedItemIcon;
    public InventoryHolder playerInventoryHolder;

    [Header("Buttons")]
    public Button confirmButton;
    public Button cancelButton;

    //private NPCBuyer currentNPC;
    //private InventorySlot placedItemSlot;

    [SerializeField] private TradeSystem tradeSystem;

    private void Awake()
    {       
        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmButtonPressed);
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancleButtonPress);
    }




    public void OpenTrade(NPCBuyer npc)
    {
        Debug.Log("Open Trade");

        //currentNPC = npc;
        requestedItemIcon.sprite = npc.requestedItem.icon;
        tradePanel.SetActive(true);

        InventoryHolder holder = FindAnyObjectByType<InventoryHolder>();

        InventoryHolder.OnDynamicInventoryDisplayRequested?.Invoke(holder.InventorySystem);

        //tradeSystem.SetCurrentNPC(npc);
    }

    private void OnConfirmButtonPressed()
    {
        tradeSystem.ConfirmTrade();
    }

    private void OnCancleButtonPress()
    {
        tradeSystem.CancleTrade();
    }


    public void CloseTrade()
    {
        Debug.Log("Close Trade");

        tradePanel.SetActive(false);
        //placedItemSlot = null;
    }
/*
    public void SetPlacedItemSlot(InventorySlot slot)
    {
        placedItemSlot = slot;
        tradeSystem.SetPlacedItemSlot(slot);
    }
*/
}
