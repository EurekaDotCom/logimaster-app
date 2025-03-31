using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QRScreenWindow : UIWindow
{
    [SerializeField]
    private QRScanner _qRScanner;
    [SerializeField]
    private GameObject _errorWindow;
    public GameObject ErrorWindow => _errorWindow;
    [SerializeField]
    private GameObject _cameraWindow;
    public GameObject CameraWindow => _cameraWindow;
    [SerializeField]
    private GameObject _resultWindow;
    public GameObject ResultWindow => _resultWindow;
    [SerializeField]
    private TextMeshProUGUI _scanerStatusText;
    public TextMeshProUGUI ScanerStatusText => _scanerStatusText;

    #region Result screen
    [Header("Result screen"), Space(10), SerializeField]
    private TextMeshProUGUI _slotText;
    public TextMeshProUGUI SlotText => _slotText;
    [SerializeField]
    private TextMeshProUGUI _serialNumberText;
    public TextMeshProUGUI SerialNumberText => _serialNumberText;
    [SerializeField]
    private TextMeshProUGUI _destinationText;
    public TextMeshProUGUI DestinationText => _destinationText;
    [SerializeField]
    private TextMeshProUGUI _materialText;
    public TextMeshProUGUI MaterialText => _materialText;
    [SerializeField]
    private TextMeshProUGUI _sizeText;
    public TextMeshProUGUI SizeText => _sizeText;
    [SerializeField]
    private TextMeshProUGUI _quantityForDestinationText;
    public TextMeshProUGUI QuantityForDestination => _quantityForDestinationText;
    [SerializeField]
    private TextMeshProUGUI _totalQuantity;
    public TextMeshProUGUI TotalQuantity => _totalQuantity;

    [SerializeField]
    private List<Image> _imagesThatChangeColor;
    [SerializeField]
    private List<TextMeshProUGUI> _textThatChangeColor;

    [SerializeField]
    private GameObject _transferButton;
    private GetSlotRespond _slotData;
    #endregion

    public void SetDestination(string destination, Color color, bool isTransfer = false)
    {
        _destinationText.text = destination;
        foreach (var image in _imagesThatChangeColor)   { image.color = color; }
        foreach (var text in _textThatChangeColor)      { text.color = color; }
        _transferButton.SetActive(isTransfer);
    }

    public void ResetVisual()
    {
        foreach (var image in _imagesThatChangeColor) { image.color = new Color(0.01960784f, 0.5450981f, 0.5490196f, 1f); }
        foreach (var text in _textThatChangeColor) { text.color = new Color(0.01960784f, 0.5450981f, 0.5490196f, 1f); }
        _transferButton.SetActive(false);
    }

    [ContextMenu("qwe")]
    public void GetDataFromQRScan(string qrcodeString)
    {
        QRCodeString scanRequest = JsonUtility.FromJson<QRCodeString>(qrcodeString);

        onSuccesRequest = OnSuccesGetProductData;
        onProtocolErrorRequest = OnProtocolError;

        string getRequestString = $"{UrlAddresses.GetProductFromScan}?token={ClientInfo.token}&id={scanRequest.id}&product_token={scanRequest.product_token}";
        StartCoroutine(SendGetRequest(getRequestString));
    }

    private void OnProtocolError()
    {
        _errorWindow.SetActive(true);
    }

    private void OnSuccesGetProductData(string webResponse)
    {
        Debug.Log(webResponse);
        ScanRespond scanRespond = JsonUtility.FromJson<ScanRespond>(webResponse);

        SerialNumberText.text = scanRespond.data.product.serial_number.ToString();
        if (scanRespond.data.product.destination_partner_id == null)
        {
            SetDestination("NONE", new Color(0.01960784f, 0.5450981f, 0.5490196f, 1f), scanRespond.data.product.transfered);
        }
        else
        {
            SetDestination(scanRespond.data.product.destination_partner_id.ToString(), new Color(1f, 0f, 0f, 1f), scanRespond.data.product.transfered);
        }
        
        MaterialText.text = scanRespond.data.product.material_id.ToString();
        SizeText.text = scanRespond.data.product.size.ToString();
        QuantityForDestination.text = scanRespond.data.product.qty.ToString();
        TotalQuantity.text = scanRespond.data.product.qty.ToString();

        int? id = Int32.Parse(scanRespond.data.product.slot_id);
        GetSlot(id);
    }

    public void GetSlot(int? id)
    {
        if (id == null)
        {
            ResultWindow.SetActive(true);
            CameraWindow.SetActive(false);
            SlotText.text = "None";
            return;
        }

        onSuccesRequest = OnSuccesGetSlotsData;
        onProtocolErrorRequest = OnProtocolError;

        string getRequestString = $"{UrlAddresses.GetSlot}?token={ClientInfo.token}&id={id}";
        StartCoroutine(SendGetRequest(getRequestString));
    }

    private void OnSuccesGetSlotsData(string webResponse)
    {
        ResultWindow.SetActive(true);
        CameraWindow.SetActive(false);
        _slotData = JsonUtility.FromJson<GetSlotRespond>(webResponse);
        SlotText.text = _slotData.data.slot.name.ToString();
    }
}

public class QRCodeString
{
    public int id;
    public string product_token;
}