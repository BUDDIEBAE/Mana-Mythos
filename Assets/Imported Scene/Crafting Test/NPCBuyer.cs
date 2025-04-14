using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBuyer : MonoBehaviour
{
    //public string npcName;

    [Header("Movement")]
    public float moveSpeed = 2f;


    [Header("Items")]
    private bool isBuying = false;
    public ItemData requestedItem;
    public List<ItemData> itemTobuy;
    [SerializeField] private NpcNeedsData _needsData;

    [Header("References")]
    public SellingSystem tradeUI; // Reference to the UI

    //public GameObject _NpcPrefab;

    [Header("Transform")]
    public Transform destination; // Assign a transform in the Inspector
    public Transform spawnPoint;



    private void Start()
    {
        OnGenerateNPC();

    }

    void Update()
    {
    }

    private void OnGenerateNPC()
    {
        _needsData.GenerateItemsToBuy();
        itemTobuy.Clear();
        for(int i = 0; i < _needsData._itemToBuy.Count; i++)
        {
            itemTobuy.Add(_needsData._itemToBuy[i]);
        }
    }

    private void OnRequestItem()
    {
        if(isBuying)
        {
            requestedItem = itemTobuy[Random.Range(0, itemTobuy.Count)];
        }
    }

    public void StartBuying()
    {
        isBuying = true;
    }

    void MoveToBuyingSpot()
    {
        if (destination != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);
        }
    }
    public bool isUIOpen;

    public void OpenTradeUI()
    {
        if (tradeUI != null)
        {
            tradeUI.OpenTrade(this);
            isUIOpen = true;
        }
    }

    public void CloseTradeUI()
    {
        if (tradeUI != null)
        {
            tradeUI.CloseTrade();
            isUIOpen = false;
        }
    }
    public bool CheckItemMatch(ItemData item)
    {
        if(requestedItem == item)
        {
            Instantiate(this, spawnPoint.transform.position,Quaternion.identity);
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
        //return allPosibleItems.Contains(item);
    }
}