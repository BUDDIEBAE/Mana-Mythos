using MaskTransitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WallMaterialPair
{
    public GameObject wall;
    public Material transparentMaterial;
    [HideInInspector] public Material[] originalMaterials;
}

public class StoreSceneManager : MonoBehaviour
{
    [SerializeField] private Transform _changeToCraftRoom;
    [SerializeField] private Transform _changeToStore;

    [SerializeField] private Transform _changeToTown;

    [SerializeField] private GameObject _player;
    [SerializeField] private float _interactionDistance;

    [SerializeField] private float _transitionTime;

    private TeleportZone _lastTeleportedTo = TeleportZone.None;
    private bool _canTeleport = true;

    [Header("Wall")]
    [SerializeField] private AreaTrigger[] _areaTrigger;    
    [SerializeField] private WallMaterialPair[] _wallToDisappear;
    [SerializeField] private bool _wallsAreTransparent = false;
    private bool _wasInside = false;
    private void Start()
    {
        _player = FindAnyObjectByType<PlayerController>().gameObject;

        foreach (var wallPair in _wallToDisappear)
        {
            var renderer = wallPair.wall.GetComponent<Renderer>();
            if (renderer != null)
            {
                wallPair.originalMaterials = renderer.materials;
            }
        }
    }

    private void Update()
    {
        bool inside = _areaTrigger[0].PlayerIsInside || _areaTrigger[1].PlayerIsInside;

        if(inside && !_wasInside)
        {
            MakeWallsTransparent();
            _wasInside = true;
        }
        else if(!inside && _wasInside)
        {
            MakeWallsOpaque();
            _wasInside = false;
        }
        

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

    private void MakeWallsTransparent()
    {
        foreach (var wallPair in _wallToDisappear)
        {
            var renderer = wallPair.wall.GetComponent<Renderer>();
            if (renderer != null && wallPair.transparentMaterial != null)
            {
                // Replace each material with the same transparent one
                Material[] transparentMats = new Material[renderer.materials.Length];
                for (int i = 0; i < transparentMats.Length; i++)
                {
                    transparentMats[i] = wallPair.transparentMaterial;
                }
                renderer.materials = transparentMats;
            }
        }
    }

    private void MakeWallsOpaque()
    {
        foreach (var wallPair in _wallToDisappear)
        {
            var renderer = wallPair.wall.GetComponent<Renderer>();
            if (renderer != null && wallPair.originalMaterials != null)
            {
                renderer.materials = wallPair.originalMaterials;
            }
        }
    }

    private void TryTeleport()
    {
        if (_changeToCraftRoom && Vector3.Distance(_player.transform.position, _changeToCraftRoom.position) < _interactionDistance)
        {
            StartCoroutine(TeleportWithDelay(_changeToStore.position, TeleportZone.Store));
        }
        else if (_changeToStore && Vector3.Distance(_player.transform.position, _changeToStore.position) < _interactionDistance)
        {
            StartCoroutine(TeleportWithDelay(_changeToCraftRoom.position, TeleportZone.CraftRoom));
        }
    }

    private IEnumerator TeleportWithDelay(Vector3 targetPosition, TeleportZone teleportZone)
    {
        _canTeleport = false;

        // 🔽 Play animation here (optional)       

        TransitionManager.Instance.PlayTransition(_transitionTime);

        yield return new WaitForSeconds(_transitionTime - 1.5f); // Adjust delay for animation duration

        _player.transform.position = targetPosition;
        _lastTeleportedTo = teleportZone;
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
 
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawSphere(_changeToCraftRoom.position, _interactionDistance);
        Gizmos.DrawSphere(_changeToStore.position, _interactionDistance);
        Gizmos.DrawSphere(_changeToTown.position, _interactionDistance);
    }

}

public enum TeleportZone
{
    None,
    CraftRoom,
    Store
}


