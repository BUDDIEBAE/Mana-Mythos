using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CheckHitBox : MonoBehaviour
{
    [SerializeField] private bool _isInCheckFurnancebox;
    [SerializeField] private bool _isInCheckNPC;
    [SerializeField] private bool _isInCheckPotion;


    /*public void OnTriggerTradeUI(TradeSystem tradeSystem)
    {
        TradeUIController T = FindObjectOfType<TradeUIController>();
        UIController uiController = FindObjectOfType<UIController>();
        InventoryHolder holder = FindObjectOfType<InventoryHolder>();

        uiController.ToggleInventory(holder.InventorySystem);
        if (!npc.isUIOpen)
        {
            //npc.OpenTradeUI();
        }
        else
        {
            //npc.CloseTradeUI();
        }
    }*/


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
                UIController uiController = FindObjectOfType<UIController>();
                InventoryHolder holder = FindObjectOfType<InventoryHolder>();
            if(_isInCheckFurnancebox)
            {                
                if(holder != null && uiController != null)
                {
                    uiController._furnancePanel.SetActive(!uiController.isOpen);
                    uiController.ToggleInventory(holder.InventorySystem);
                }
            }
/*            if(_isInCheckNPC)
            {

            }*/
            if(_isInCheckPotion)
            {
                if (holder != null && uiController != null)
                {
                    uiController._potionPanel.SetActive(!uiController.isOpen);
                    uiController.ToggleInventory(holder.InventorySystem);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FurnanceCheck")
        {
            _isInCheckFurnancebox = true;
        }
        if(other.tag == "NPC")
        {
            _isInCheckNPC = true;
        }
        if(other.tag == "PotionCheck")
        {
            _isInCheckPotion= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIController uiController = FindObjectOfType<UIController>();
        InventoryHolder holder = FindObjectOfType<InventoryHolder>();
        if (other.tag == "FurnanceCheck")
        {
            _isInCheckFurnancebox = false;


            if(uiController.isOpen)
            {
                uiController._furnancePanel.SetActive(!uiController.isOpen);
                uiController.ToggleInventory(holder.InventorySystem);

            }
        }
/*        if (other.tag == "NPC")
        {
            _isInCheckNPC = false;

            NPCBuyer npc = other.GetComponent<NPCBuyer>();
            npc.CloseTradeUI();
            if (uiController.isOpen)
            {
                uiController.ToggleInventory(holder.InventorySystem);
            }
        }*/
        if(other.tag == "PotionCheck")
        {
            _isInCheckPotion = false;

            if (uiController.isOpen)
            {
                uiController._potionPanel.SetActive(!uiController.isOpen);
                uiController.ToggleInventory(holder.InventorySystem);
            }
        }
    }
}
