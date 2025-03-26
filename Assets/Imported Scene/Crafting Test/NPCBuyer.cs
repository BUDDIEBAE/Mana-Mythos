using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBuyer : MonoBehaviour
{
    public string npcName;
    public ItemData requestedItem;
    public List<ItemData> allPosibleItems;
    public float moveSpeed = 2f;
    public Transform destination; // Assign a transform in the Inspector
    private bool isBuying = false;

    public SellingSystem tradeUI; // Reference to the UI

    public GameObject _NpcPrefab;
    public Transform spawnPoint;
    private void Start()
    {
        StartBuying();
        requestedItem = allPosibleItems[Random.Range(0, allPosibleItems.Count)];
    }

    void Update()
    {
        if (isBuying)
        {
            MoveToPlayer();
        }
    }

    public void StartBuying()
    {
        isBuying = true;
    }

    void MoveToPlayer()
    {
        if (destination != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);

            /*if (Vector3.Distance(transform.position, destination.position) < 1f)
            {
                isBuying = false;
                OpenTradeUI();
            }*/
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
