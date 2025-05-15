using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBuyer : MonoBehaviour
{
    //public string npcName;

    [Header("Movement")]
    public float moveSpeed = 2f;


    [Header("Items")]
    public bool isBuying = false;
    public ItemData requestedItem;
    public List<ItemData> itemTobuy;
    [SerializeField] public NpcNeedsData _needsData;

    [Header("References")]
    public TradeUIController tradeUI; // Reference to the UI

    [Header("Transform")]
    public Transform destination;

    public void OnGenerateNPC()
    {
        itemTobuy.Clear();
        _needsData.GenerateItemsToBuy();
        for(int i = 0; i < _needsData._itemToBuy.Count; i++)
        {
            itemTobuy.Add(_needsData._itemToBuy[i]);
        }
    }

    private void OnRequestItem() // need more logic
    {
        if(isBuying)
        {
            requestedItem = itemTobuy[Random.Range(0, itemTobuy.Count)];
        }
    }

    public void OnMoveToBuyingSpot(Transform[] where)
    {       
        StartCoroutine(OnMove(where));
    }

    IEnumerator OnMove(Transform[] destinations)
    {
        foreach (Transform destination in destinations)
        {
            if (destination == null) continue;

            while (Vector3.Distance(transform.position, destination.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = destination.position;
        }

        // Once all destinations have been reached
        isBuying = true;
        OnRequestItem();
        //StartBuying();
    }

    public void OnLeftTheStore(Transform[] where)
    {
        Debug.Log("Is Leaving");
        StartCoroutine(OnLeftStore(where));


    }

    IEnumerator OnLeft(Transform where)
    {
        if (isBuying == true) yield break;
        Debug.Log("OnLeft is working");
        while (Vector3.Distance(transform.position, where.position) > 0.01f) // Check if arrived to the destination
        {
            transform.position = Vector3.MoveTowards(transform.position, where.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = where.position;

        Destroy(gameObject);
    }
    IEnumerator OnLeftStore(Transform[] destinations)
    {
        if (isBuying == true) yield break;

        foreach (Transform destination in destinations)
        {
            if (destination == null) continue;

            while (Vector3.Distance(transform.position, destination.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = destination.position;
        }

        Destroy(gameObject);
    }


    public void StartBuying()
    {
        tradeUI.OpenTrade(this);
    }


    /// <summary>
    /// //////////////////////////////////////////
    /// </summary>
   /* public bool isUIOpen;
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
    */
}