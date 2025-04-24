using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TradeSystem : Singleton<TradeSystem>
{
    
    [SerializeField] private NPCBuyer currentNPC;
    [SerializeField] private Slot placedItemSlot; // might change later

    [SerializeField] private TradeUIController uiManager;

    [SerializeField] private List<GameObject> npcPrefabs = new List<GameObject>();
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private Transform destination;

    [Header("Player Interaction")]
    [SerializeField] private Transform playerStand;
    [SerializeField] private float interactionDistance = 2f;
    private void SetCurrentNPC(NPCBuyer npc)
    {
        currentNPC = npc;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            OnSpawnNPC();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            OnCheckPlayerDistance();
        }

    }

    private void OnCheckPlayerDistance()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        float distance = Vector3.Distance(playerTransform.position, playerStand.position);

        if(distance <= interactionDistance)
        {
            if(currentNPC != null)
            {
                if(!uiManager.tradePanel.activeSelf)
                {
                    uiManager.OpenTrade(currentNPC);
                }
                else
                {
                    uiManager.CloseTrade();
                }
            }
        }
    }

    public void OnSpawnNPC()
    {
        List<GameObject> weightedList = new List<GameObject>();

        foreach (GameObject npcPrefab in npcPrefabs)
        {
            NPCBuyer NPC = npcPrefab.GetComponent<NPCBuyer>();
            NpcNeedsData npcData = NPC._needsData;
            if (npcData == null)
            {
                Debug.LogWarning($"NpcNeedsData is missing! : Object<{npcPrefab.name}>");
                continue;
            }

            int weight = 4 - npcData.rarity; // 1 = Common, 2 = Uncommon, 3 = Rare
            for (int i = 0; i < weight; i++)
            {
                weightedList.Add(npcPrefab);
            }
        }

        if (weightedList.Count == 0)
        {
            Debug.LogWarning("No NPCs to spawn.");
            return;
        }

        GameObject selectedPrefab = weightedList[Random.Range(0, weightedList.Count)];

        GameObject newNPC = Instantiate(selectedPrefab, spawnTransform.position, Quaternion.identity);
        NPCBuyer buyer = newNPC.GetComponent<NPCBuyer>();
        buyer.tradeUI = uiManager;
        buyer.OnGenerateNPC();
        buyer.OnMoveToBuyingSpot(destination);
        SetCurrentNPC(buyer);
    }

    public void ConfirmTrade()
    {
        if (placedItemSlot == null)
        {
            Debug.Log($"{gameObject.name} Placed Item Slot is null");
            return;
        }
        if (placedItemSlot._itemdata == null)
        {
            Debug.Log("No item placed.");
            return;
        }

        if(!CheckItemMatch(placedItemSlot._itemdata))
        {
            Debug.Log("Incorrect item placed."); 
            return;
        }

        //Debug.Log($"Sold {placedItemSlot._itemdata.itemName} to {currentNPC.gameObject.name}!");

        placedItemSlot._itemdata = null;
        placedItemSlot._image.sprite = null;
        placedItemSlot._image.color = new Color(1f, 1f, 1f, 0f);

        currentNPC.isBuying = false;
        currentNPC.OnLeftTheStore(spawnTransform);
        currentNPC = null;


        uiManager.CloseTrade();

        OnSpawnNPC();
    }

    public void CancleTrade()
    {
        currentNPC.isBuying = false;
        currentNPC.OnLeftTheStore(spawnTransform);
        currentNPC = null;

        uiManager.CloseTrade();

        OnSpawnNPC();
    }

    public bool CheckItemMatch(ItemData item)
    {
        if (currentNPC.requestedItem == item) // if the item match
        {
            // Increase Player's money here

            return true;
        }
        else if(currentNPC.requestedItem.itemType == item.itemType)
        {
            if(item.sellPrice > currentNPC.requestedItem.sellPrice)
            {
                // If the price of item that player offer is higher than requested item price == the price that player will get is equal to item match amount
                return true;
            }
            else
            {
                // If the price of item that player offer is lower than requested item price == lower the amount of money that player will get (Around 60-80%)

                return true;
            }
        }
        else // item not match at all
        {
            return false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (playerStand == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerStand.position, interactionDistance);
    }

}
