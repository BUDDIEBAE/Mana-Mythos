using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MerchantManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController _player;
    [SerializeField] public int moneyToSpend;

    [Header("Interaction")]
    [SerializeField] private Transform _interactTransform;
    [SerializeField] private float _interactiondistance;

    [Header("UI")]
    [SerializeField] private GameObject _catalogPanel;

    [Header("Buttons")]
    [SerializeField] private BuyMaterialButton[] buttons;
    [SerializeField] private Button confirmButton;


    // Update is called once per frame
    void Update()
    {
        CheckDistanceInteraction();
    }

    private void CheckDistanceInteraction()
    {
        float distance = Vector3.Distance(_player.gameObject.transform.position, _interactTransform.position);
        if(distance < _interactiondistance)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {                
                _catalogPanel.SetActive(!_catalogPanel.activeSelf);
            }
        }
    }

    public void OnConfirmBuy()
    {
        

        InventoryHolder inventoryHolder = FindAnyObjectByType<InventoryHolder>();

        for (int i = 0; i < buttons.Length; i++)
        {
            inventoryHolder.InventorySystem.AddItemToInventory(buttons[i].GetItem(), buttons[i].GetQuantity());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_interactTransform.position, _interactiondistance);
    }
}
