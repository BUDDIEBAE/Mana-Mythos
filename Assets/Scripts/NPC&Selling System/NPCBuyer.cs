using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;

    [Header("Transform")]
    public Transform destination;

    public void OnGenerateNPC()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
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

            Vector3 target = destination.position;

            while (Vector3.Distance(_rb.position, target) > 0.05f)
            {
                Vector3 direction = (target - _rb.position).normalized;
                _rb.velocity = direction * moveSpeed;

                // Optional: Update animator parameters here using direction.x, direction.y
                // animator.SetFloat("MoveX", direction.x);
                // animator.SetFloat("MoveY", direction.y);
                // animator.SetBool("IsMoving", true);

                yield return null;
            }

            _rb.velocity = Vector2.zero;
            _rb.position = target;
        }

        // Once all destinations have been reached
        isBuying = true;
        OnRequestItem();
        //StartBuying();

        // Optional: animator.SetBool("IsMoving", false);
    }

    public void OnLeftTheStore(Transform[] where)
    {
        Debug.Log("Is Leaving");
        StartCoroutine(OnLeftStore(where));


    }
/*
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
    }*/
    IEnumerator OnLeftStore(Transform[] destinations)
    {
        if (isBuying) yield break;

        foreach (Transform destination in destinations)
        {
            if (destination == null) continue;

            Vector3 target = destination.position;

            while (Vector3.Distance(_rb.position, target) > 0.05f)
            {
                Vector3 direction = (target - _rb.position).normalized;
                _rb.velocity = direction * moveSpeed;

                // Optional: Call animation update method here using direction.x, direction.y

                yield return null;
            }

            _rb.velocity = Vector3.zero;
            _rb.position = target;
        }

        Destroy(gameObject);
    }


    public void StartBuying()
    {
        tradeUI.OpenTrade(this);
    }

    private void Update()
    {
        UpdateAnimationFromVelocity();
    }

    private void UpdateAnimationFromVelocity()
    {
        Vector3 velocity = _rb.velocity;
        Debug.Log(velocity);
        if (velocity.magnitude > 0.01f)
        {
/*            if(_rb.velocity.magnitude > 0.01f)
            {
                _animator.enabled= false;
            }
            else
            {
                _animator.enabled= true;
            }*/
            _animator.SetFloat("MoveX", _rb.velocity.x);
            _animator.SetFloat("MoveZ", _rb.velocity.y);
           // _animator.SetBool("IsMoving", _rb.velocity.magnitude > 0.01f);

        }
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