using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MerchantManager : MonoBehaviour
{
    [Header("References")]

    [Header("Money")]
    [SerializeField] public int moneyToSpend;
    [SerializeField] private TMP_Text _moneyText;

    [Header("Interaction")]
    [SerializeField] private PlayerController _player;
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
        if(_player == null)
        {
            _player = FindAnyObjectByType<PlayerController>();
        }
        float distance = Vector3.Distance(_player.gameObject.transform.position, _interactTransform.position);
        if(distance > _interactiondistance)
        {
            _catalogPanel.SetActive(false);
            OnResetPanel();
        }
        else if(distance < _interactiondistance)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {                
                _catalogPanel.SetActive(!_catalogPanel.activeSelf);
                OnResetPanel();
            }
        }
    }

    public void UpdateMoneyText(int amount = 0)
    {
        moneyToSpend += (amount);
        if(moneyToSpend <= 0)
        {
            moneyToSpend = 0;
        }
        _moneyText.text = moneyToSpend.ToString();
    }


    public void OnConfirmBuy()
    {
        Debug.Log("OnConfirmBuy called");


        if (moneyToSpend > InventoryHolder.instance.Money)
        {
            Debug.LogWarning("Not enough money to buy.");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            var item = buttons[i].GetItem();
            int quantity = buttons[i].GetQuantity();

            Debug.Log($"Button {i}: Item = {(item != null ? item.name : "null")}, Quantity = {quantity}");

            if (quantity > 0)
            {
                bool added = InventoryHolder.instance.InventorySystem.AddItemToInventory(item, quantity);
                if (!added)
                {
                    Debug.LogWarning($"❌ Failed to add {quantity}x {item.name} to inventory.");
                }
                else
                {
                    Debug.Log($"✅ Added {quantity}x {item.name} to inventory.");
                }
            }
        }

        InventoryHolder.instance.SpendMoney(moneyToSpend);
        moneyToSpend = 0;
        _catalogPanel.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_interactTransform.position, _interactiondistance);
    }

    private void OnResetPanel()
    {
        moneyToSpend = 0;
        _moneyText.text = moneyToSpend.ToString();
    }
    
}
