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
        InventoryHolder inventoryHolder = InventoryHolder.instance;

        if (moneyToSpend > InventoryHolder.instance.Money)
        {
            // if money to spend is more than current money show something

            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetQuantity() > 0)
            {
                inventoryHolder.InventorySystem.AddItemToInventory(buttons[i].GetItem(), buttons[i].GetQuantity());
            }
        }

        inventoryHolder.SpendMoney(moneyToSpend);
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
