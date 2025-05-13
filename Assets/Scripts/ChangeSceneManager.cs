using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    [SerializeField] private Transform _changeToCraftRoom;
    [SerializeField] private Transform _changeToStore;

    [SerializeField] private Transform _changeToTown;

    [SerializeField] private GameObject _player;
    [SerializeField] private float _interactionDistance;

    private bool _wasInCraftRoomRange = false;
    private bool _wasInStoreRange = false;
    private bool _wasInTownRange = false;

    private TeleportZone _lastTeleportedTo = TeleportZone.None;
    private bool _canTeleport = true;

    private void Start()
    {
        _player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    private void Update()
    {
        if (_canTeleport)
        {
            TryTeleport();
        }
        else
        {
            CheckIfPlayerHasLeftLastDestination();
        }

        TryChangeToTown();
    }

    private void TryTeleport()
    {
        if (_changeToCraftRoom && Vector3.Distance(_player.transform.position, _changeToCraftRoom.position) < _interactionDistance)
        {
            _player.transform.position = _changeToStore.position;
            _lastTeleportedTo = TeleportZone.Store;
            _canTeleport = false;
        }
        else if (_changeToStore && Vector3.Distance(_player.transform.position, _changeToStore.position) < _interactionDistance)
        {
            _player.transform.position = _changeToCraftRoom.position;
            _lastTeleportedTo = TeleportZone.CraftRoom;
            _canTeleport = false;
        }
    }
    private void CheckIfPlayerHasLeftLastDestination()
    {
        switch (_lastTeleportedTo)
        {
            case TeleportZone.CraftRoom:
                if (Vector3.Distance(_player.transform.position, _changeToCraftRoom.position) >= _interactionDistance)
                    _canTeleport = true;
                break;

            case TeleportZone.Store:
                if (Vector3.Distance(_player.transform.position, _changeToStore.position) >= _interactionDistance)
                    _canTeleport = true;
                break;
        }

        if (_canTeleport)
            _lastTeleportedTo = TeleportZone.None;
    }

    private void TryChangeToTown()
    {
        if (_changeToTown == null) return;

        if (Vector3.Distance(_player.transform.position, _changeToTown.position) < _interactionDistance)
        {
            SceneManager.LoadScene("TownSceneName"); // Replace with your scene name
        }
    }
    /*private void OnChangeToCraftRoom()
    {
        if (_changeToCraftRoom == null || _changeToStore == null) return;

        float distance = Vector3.Distance(_player.transform.position, _changeToCraftRoom.position);
        bool isInRange = distance < _interactionDistance;

        if (isInRange && !_wasInCraftRoomRange)
        {
            // Just entered the range
            _player.transform.position = _changeToStore.position;
            _wasInCraftRoomRange = true;
        }
        else if (!isInRange && _wasInCraftRoomRange)
        {
            // Just exited the range
            _wasInCraftRoomRange = false;
        }
    }

    private void OnChangeToStore()
    {
        if (_changeToStore == null || _changeToCraftRoom == null) return;

        float distance = Vector3.Distance(_player.transform.position, _changeToStore.position);
        bool isInRange = distance < _interactionDistance;

        if (isInRange && !_wasInStoreRange)
        {
            _player.transform.position = _changeToCraftRoom.position;
            _wasInStoreRange = true;
        }
        else if (!isInRange && _wasInStoreRange)
        {
            _wasInStoreRange = false;
        }
    }

    private void OnChangeToTown()
    {
        if (_changeToTown == null) return;

        float distance = Vector3.Distance(_player.transform.position, _changeToTown.position);
        bool isInRange = distance < _interactionDistance;

        if (isInRange && !_wasInTownRange)
        {
            // Example logic
            // _player.transform.position = someOtherPlace;
            _wasInTownRange = true;
        }
        else if (!isInRange && _wasInTownRange)
        {
            _wasInTownRange = false;
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawSphere(_changeToCraftRoom.position, _interactionDistance);
        Gizmos.DrawSphere(_changeToStore.position, _interactionDistance);
        Gizmos.DrawSphere(_changeToTown.position, _interactionDistance);
    }

    private bool IsPlayerFarFromAllPoints()
    {
        return
            (_changeToCraftRoom == null || Vector3.Distance(_player.transform.position, _changeToCraftRoom.position) >= _interactionDistance) &&
            (_changeToStore == null || Vector3.Distance(_player.transform.position, _changeToStore.position) >= _interactionDistance) &&
            (_changeToTown == null || Vector3.Distance(_player.transform.position, _changeToTown.position) >= _interactionDistance);
    }
}

public enum TeleportZone
{
    None,
    CraftRoom,
    Store
}
