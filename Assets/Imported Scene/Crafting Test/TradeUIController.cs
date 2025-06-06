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

    [Header("Small UI Request Item")]
    [SerializeField] private Image smallRequestIcon;
    [SerializeField] private Image smallRequestFrame;

    private void Awake()
    {       
        //_buttonText = confirmButton.GetComponentInChildren<TextMeshPro>();

       /* if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancleButtonPress);*/
    }
    private void Update()
    {
        SmallRequestFrame();
        UpdateConfirmButton();
        SmallRequestItemUI();
    }

    private void SmallRequestFrame()
    {
        if(smallRequestFrame != null)
        {
            smallRequestFrame.gameObject.SetActive(!tradePanel.activeSelf);

        }
        else
        {
            smallRequestFrame = GameObject.FindGameObjectWithTag("SmallRequestFrame").GetComponent<Image>();
        }
    }

    private void SmallRequestItemUI() // Bug To Fix
    {
        // Ensure smallRequestIcon is assigned
        if (smallRequestIcon == null)
        {
            GameObject iconObject = GameObject.FindWithTag("SmallRequestItem");
            if (iconObject != null)
                smallRequestIcon = iconObject.GetComponent<Image>();
        }

        if (smallRequestIcon == null)
            return; // Exit early if still null


        // Try to find and assign requestedItemIcon
        if (requestedItemIcon == null)
        {
            GameObject requestedObject = GameObject.FindWithTag("RequestItem");
            if (requestedObject != null)
                requestedItemIcon = requestedObject.GetComponent<Image>();
        }

        bool iconSet = false;

        // Priority 1: Use requestedItemIcon if available
        if (requestedItemIcon != null && requestedItemIcon.sprite != null)
        {
            smallRequestIcon.sprite = requestedItemIcon.sprite;
            SetImageAlpha(smallRequestIcon, 1f);
            iconSet = true;
        }
        else
        {
            // Priority 2: Find the NPC who is buying
            NPCBuyer[] npcs = GameObject.FindObjectsOfType<NPCBuyer>();
            foreach (var npc in npcs)
            {
                if (npc.isBuying && npc.requestedItem != null && npc.requestedItem.icon != null)
                {
                    smallRequestIcon.sprite = npc.requestedItem.icon;
                    SetImageAlpha(smallRequestIcon, 1f);
                    iconSet = true;
                    break;
                }
            }
        }

        // If no icon could be set, hide the image
        if (!iconSet)
        {
            smallRequestIcon.sprite = null;
            SetImageAlpha(smallRequestIcon, 0f);
        }
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
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
        smallRequestIcon.sprite = null;
        SetImageAlpha(smallRequestIcon, 0f);

    }

    private void OnCancelButtonPress()
    {
        tradePanel.SetActive(false);

        tradeSystem.CancleTrade();
        RefreshInventoryUI();
        smallRequestIcon.sprite= null;
        SetImageAlpha(smallRequestIcon, 0f);

    }


    private void RefreshInventoryUI()
    {
        InventorySystem holder = playerInventoryHolder.InventorySystem;
        InventoryHolder.OnDynamicInventoryDisplayRequested?.Invoke(holder);
        uiController.ToggleInventory(holder); // Add this line back
    }
}
