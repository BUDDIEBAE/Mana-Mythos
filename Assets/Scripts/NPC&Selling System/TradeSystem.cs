using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TradeSystem : MonoBehaviour
{
    [SerializeField] private NPCBuyer currentNPC;
    [SerializeField] private Slot placedItemSlot; // might change later

    [SerializeField] private TradeUIController uiManager;

    [SerializeField] private List<GameObject> npcPrefabs = new List<GameObject>();
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private Transform[] destination;

    [SerializeField] private Transform[] leavingDestination;

    [Header("Player Interaction")]
    [SerializeField] private Transform playerStand;
    [SerializeField] private float interactionDistance = 2f;
    public Image interactionImage;

    public bool _hasPlaceItem;
    private bool _areTrading => TimeManager.instance.IsStoreOpen;


    private void SetCurrentNPC(NPCBuyer npc)
    {
        currentNPC = npc;
    }
    private void Update()
    {
        
        if(placedItemSlot._itemdata == null)
        {
            _hasPlaceItem= false;
        }
        else
        {
            _hasPlaceItem= true;
        }

        OnCheckPlayerDistance();        

    }

    private void OnCheckPlayerDistance()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        float distance = Vector3.Distance(playerTransform.position, playerStand.position);
        FindPlayerStandAndInteractionImage();

        if (distance <= interactionDistance)
        {
            interactionImage.gameObject.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E))
            {
                if(currentNPC != null && _areTrading)
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
                else if(currentNPC != null) // clear last NPC
                {
                    if (!uiManager.tradePanel.activeSelf)
                    {
                        uiManager.OpenTrade(currentNPC);
                    }
                    else
                    {
                        uiManager.CloseTrade();
                    }
                }

                if(!_areTrading && TimeManager.instance.CanOpenShopBeforeTime()) // If haven't open store :open store
                {
                    TimeManager.instance.IsStoreOpen = true;
                    TimeManager.instance.TryOpenStore();
                    OnSpawnNPC();
                }

            }
        }
        else
        {
            if (interactionImage == null) return;
            interactionImage.gameObject.SetActive(false);
        }
    }

    private void FindPlayerStandAndInteractionImage()
    {
        if (playerStand == null) playerStand = GameObject.FindWithTag("PlayerStand").GetComponent<Transform>();
        if (interactionImage == null) interactionImage = GameObject.FindWithTag("SellButton").GetComponent<Image>();
    }

    public void OnSpawnNPC()
    {
        if (!_areTrading) return;

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

        if(!CheckItemMatch(placedItemSlot._itemdata, out int itemPrice))
        {
            Debug.Log("Incorrect item placed."); 
            return;
        }

        //Debug.Log($"Sold {placedItemSlot._itemdata.itemName} to {currentNPC.gameObject.name}!");

        placedItemSlot._itemdata = null;
        placedItemSlot._image.sprite = null;
        placedItemSlot._image.color = new Color(1f, 1f, 1f, 0f);

        currentNPC.isBuying = false;
        currentNPC.OnLeftTheStore(leavingDestination);
        currentNPC = null;


        //uiManager.CloseTrade();

        OnSpawnNPC();
    }

    public void CancleTrade()
    {
        currentNPC.isBuying = false;
        currentNPC.OnLeftTheStore(leavingDestination);
        currentNPC = null;

        //uiManager.CloseTrade();

        OnSpawnNPC();
    }

    public bool CheckItemMatch(ItemData item, out int itemPrice)
    {
        bool match = TryGetItemPrice(item, out itemPrice);

        if (match)
        {
            InventoryHolder.instance.AddMoney(itemPrice);
        }

        return match;
    }

    public bool TryGetItemPrice(ItemData item, out int itemPrice)
    {
        itemPrice = 0;

        if (currentNPC == null || currentNPC.requestedItem == null || item == null)
        {
            return false;
        }

        if (currentNPC.requestedItem == item)
        {
            itemPrice = item.sellPrice;
            return true;
        }
        else if (currentNPC.requestedItem.itemType == item.itemType)
        {
            if (item.sellPrice > currentNPC.requestedItem.sellPrice)
            {
                itemPrice = item.sellPrice;
                return true;
            }
            else
            {
                float priceDifference = (float)item.sellPrice / (float)currentNPC.requestedItem.sellPrice;
                float multiplier = Mathf.Lerp(0.6f, 0.8f, priceDifference);
                itemPrice = Mathf.RoundToInt(item.sellPrice * multiplier);
                return true;
            }
        }
        else
        {
            itemPrice = 0;
            return false;
        }
    }

    public ItemData GetPlacedItem()
    {
        return placedItemSlot?._itemdata;
    }

    private void OnDrawGizmos()
    {
        if (playerStand == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerStand.position, interactionDistance);
        
        for(int i = 0; i < destination.Length; i++)
        {
            Gizmos.DrawWireSphere(destination[i].position, 0.1f);
        }

    }

}
