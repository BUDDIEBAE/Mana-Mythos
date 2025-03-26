using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuActions : MonoBehaviour
{
    [SerializeField] private List<Image> _highlightImages = new List<Image>();



    GraphicRaycaster _rayCaster;
    PointerEventData _pointerEventData;
    EventSystem _eventSystem;

    private void Start()
    {
        _rayCaster = GetComponent<GraphicRaycaster>();
        _eventSystem = GetComponent<EventSystem>();

        foreach (var image in _highlightImages)
        {
            if(image.IsActive())
            {
                image.color = Color.clear;
            }
        }
    }

    private void Update()
    {

    }

    public void OnStartClick()
    {
        SceneManager.LoadSceneAsync("TownScene");
    }
}
