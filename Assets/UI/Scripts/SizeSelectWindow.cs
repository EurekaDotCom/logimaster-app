using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SizeSelectWindow : UIWindow
{
    [SerializeField]
    private TMP_InputField _lenghtInputField;
    [SerializeField]
    private TMP_InputField _widthInputField;
    [SerializeField]
    private TMP_InputField _thicknessInputField;
    [SerializeField]
    private TextMeshProUGUI _materialAndCategoryText;

    [SerializeField]
    private Button _doneButton;

    private string _size;

    private void OnEnable()
    {
        string s = $"{(_previousWindow.PreviousWindow as PrintQR).QRInfo.material.name} - {(_previousWindow.PreviousWindow as PrintQR).QRInfo.material.MaterialCategory}";
        _materialAndCategoryText.text = s;
    }

    private void Start()
    {
        _doneButton.onClick.AddListener(SetCustomSize);
    }

    public void SetCustomSize()
    {
        if(_previousWindow.PreviousWindow as PrintQR)
        {
            _size = $"{_lenghtInputField.text} x {_widthInputField.text} / {_thicknessInputField.text}";

            CreateOptionRequest _createOptionRequest = new CreateOptionRequest();
            _createOptionRequest.token = ClientInfo.token;
            _createOptionRequest.name = _size;
            _createOptionRequest.material_id = (_previousWindow.PreviousWindow as PrintQR).QRInfo.material.id;

            var jsonDataToSend = JsonConvert.SerializeObject(_createOptionRequest);
            DataJsonForRequest = jsonDataToSend;

            onSuccesRequest = OnSuccesRequset;
            onProtocolErrorRequest = OnProtocolError;

            StartCoroutine(SendPostRequest(UrlAddresses.CreateOption));
        }
    }

    private void OnProtocolError()
    {

    }

    private void OnSuccesRequset(string webResponse)
    {
        if (_previousWindow.PreviousWindow as PrintQR)
        {
            CustomUtils.TextAndColorStruct textAndColor = new CustomUtils.TextAndColorStruct();
            textAndColor.text = _size;
            textAndColor.color = Color.black;
            (_previousWindow.PreviousWindow as PrintQR).SetSize(textAndColor);

            (_previousWindow.PreviousWindow as PrintQR).UpdateData();
            _backButton.onClick.Invoke();
            _previousWindow.BackButton.onClick.Invoke();
        }
    }
}
