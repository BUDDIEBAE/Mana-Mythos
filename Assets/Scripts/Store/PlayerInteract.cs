using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform _furnanceInteraction;
    [SerializeField] private Transform _potionInteraction;
    public Image furnanceInterAction;
    public Image potionInterAction;


    [SerializeField] private GameObject _player;

    [SerializeField] private float _interactionDistance = 1.0f;


    private bool _furnanceUIOpen = false;
    private bool _potionUIOpen = false;

    [SerializeField] private UIController _uiController;
    [SerializeField] private InventoryHolder _holder;
    void Start()
    {
        _player = FindAnyObjectByType<PlayerController>().gameObject;
        _uiController = FindObjectOfType<UIController>();
        _holder = FindObjectOfType<InventoryHolder>();
    }

    void Update()
    {
        if (_player == null || _uiController == null || _holder == null) return;

        HandlePlayerInteraction();
    }

    private void HandlePlayerInteraction()
    {
        float distToFurnance = Vector3.Distance(_player.transform.position, _furnanceInteraction.position);
        float distToPotion = Vector3.Distance(_player.transform.position, _potionInteraction.position);

        // Toggle interaction with E

            // Toggle Furnance
            if (distToFurnance <= _interactionDistance)
            {
            furnanceInterAction.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    bool isActive = _uiController._furnancePanel.activeSelf;
                    _uiController._furnancePanel.SetActive(!isActive);
                    _uiController.ToggleInventory(_holder.InventorySystem);
                    _furnanceUIOpen = !isActive;

                }
            }
            else
        {
            furnanceInterAction.gameObject.SetActive(false);
        }

            // Toggle Potion
            if (distToPotion <= _interactionDistance)
            {
            potionInterAction.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
                {
                    bool isActive = _uiController._potionPanel.activeSelf;
                    _uiController._potionPanel.SetActive(!isActive);
                    _uiController.ToggleInventory(_holder.InventorySystem);
                    _potionUIOpen = !isActive;

                }
            }      
            else
        {
            potionInterAction.gameObject.SetActive(false);

        }

        // Auto-close if player moves away
        if (_furnanceUIOpen && distToFurnance > _interactionDistance)
        {
            _uiController._furnancePanel.SetActive(false);
            _uiController.ToggleInventory(_holder.InventorySystem);
            _furnanceUIOpen = false;
        }

        if (_potionUIOpen && distToPotion > _interactionDistance)
        {
            _uiController._potionPanel.SetActive(false);
            _uiController.ToggleInventory(_holder.InventorySystem);
            _potionUIOpen = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(_furnanceInteraction.position, _interactionDistance);
        Gizmos.DrawSphere(_potionInteraction.position, _interactionDistance);
    }

}
