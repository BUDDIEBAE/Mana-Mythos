using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemData _itemdata;
    public Image _image;
    [SerializeField] public int _slotIndex;
    void Awake()
    {
        _image = GetComponent<Image>();

    }
    void Update()
    {
        Color color = _image.color;

        if (_itemdata != null)
        {
            _image.sprite = _itemdata.icon;
            color.a = 1f;
        }
        else
        {
            _image.sprite = null;
            color.a = 0f;

        }
        _image.color = color;
        _image.preserveAspect = true;
        //_image.SetNativeSize();
    }

    private void OnValidate()
    {
        _image = GetComponent<Image>();
        if (_itemdata != null)
        {
            _image.sprite = _itemdata.icon;
           
        }    
        else
        {
            _image.sprite = null;
        }
        _image.preserveAspect = true;
    }
}