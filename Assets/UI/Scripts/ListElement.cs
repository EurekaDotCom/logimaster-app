using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ListElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textField;
    [SerializeField]
    private Color _defaultColor;
    [SerializeField]
    private Color _redColor;
    [SerializeField]
    private Button _deleteButton;
    public Button DeleteButton => _deleteButton;

    UIWindow _uIWindow;
    private int _id;
    private int? _slot_id;
    private string _categoryName;

    public void SetID(int id)
    {
        _id = id;
    }

    public void SetCategoryName(string categoryName)
    {
        _categoryName = categoryName;
    }

    public void SetNameText(string text)
    {
        _textField.text = text;
    }

    public void SetDefaultTextColor()
    {
        _textField.color = _defaultColor;
    }

    public void SetRedTextColor()
    {
        _textField.color = _redColor;
    }

    public void SetWindowToSendData(UIWindow uIWindow, int? slot_id = null)
    {
        this._uIWindow = uIWindow;
        this._slot_id = slot_id;
        GetComponent<Button>().onClick.AddListener(SendData);
    }
    public void SetWindowToSendMaterialData(UIWindow uIWindow)
    {
        this._uIWindow = uIWindow;
        GetComponent<Button>().onClick.AddListener(SendMaterialData);
    }

    private void SendData()
    {
        if (_uIWindow != null)
        {
            if (_uIWindow as PrintQR)
            {
                CustomUtils.TextAndColorStruct tacStruct = new CustomUtils.TextAndColorStruct();
                tacStruct.text = _textField.text;
                tacStruct.color = _defaultColor;
                (_uIWindow as PrintQR).SetSlot(tacStruct, _slot_id);
            }
        }
    }

    private void SendMaterialData()
    {
        if (_uIWindow != null)
        {
            if (_uIWindow as PrintQR)
            {
                (_uIWindow as PrintQR).SetMaterialInfo(_id, _textField.text, _categoryName);
            }
        }
    }

}

