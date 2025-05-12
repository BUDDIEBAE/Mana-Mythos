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
    private void Start()
    {
        _player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    private void Update()
    {
        OnChangeToCraftRoom();
       // OnChangedToStore();
        OnChangeToTown();
    }

    private void OnChangeToCraftRoom()
    {
        if (_changeToCraftRoom == null) return;

        float distance = Vector3.Distance(_player.transform.position, _changeToCraftRoom.position);

        if(distance < _interactionDistance)
        {
            _player.gameObject.transform.position = _changeToStore.transform.position;
        }
    }

    private void OnChangedToStore()
    {
        if(_changeToStore == null) return;

        float distance = Vector3.Distance(_player.transform.position, _changeToStore.position);
        if(distance < _interactionDistance)
        {
            _player.gameObject.transform.position = _changeToCraftRoom.transform.position;
        }
    }

    private void OnChangeToTown()
    {
        if (_changeToTown == null) return;

        float distance = Vector3.Distance(_player.transform.position, _changeToTown.position);
        if (distance < _interactionDistance)
        {
            //_player.gameObject.transform.position = _changeToTown.transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawSphere(_changeToCraftRoom.position, _interactionDistance);
        Gizmos.DrawSphere(_changeToStore.position, _interactionDistance);
        Gizmos.DrawSphere(_changeToTown.position, _interactionDistance);
    }

    
}
