using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.UI;
using UnityEngine.XR;

public class PrintWindow : UIWindow
{
    [SerializeField]
    private TextMeshProUGUI _printQuantityText;
    [SerializeField]
    private Button _printButton;
    [SerializeField]
    private Button _cancelButton;
    [SerializeField]
    private Button _industries;
    [SerializeField]
    private GameObject _sendToPrinterNotification;
    [SerializeField]
    private PrinterPrefab _printerPrefab;
    [SerializeField]
    private Transform _printersParent;

    //[SerializeField]
    //private PrinterPrefab _previousSelectedPrinter;
    //public PrinterPrefab PreviousSelectedPrinter { get { return _previousSelectedPrinter; } set { _previousSelectedPrinter = value; } }

    [SerializeField]
    private PrinterPrefab _selectedPrinter;
    public PrinterPrefab SelectedPrinter { get { return _selectedPrinter; } set { _selectedPrinter = value; } }




    private void OnEnable()
    {
        if (_previousWindow as PrintQR)
            _printQuantityText.text = (_previousWindow as PrintQR).AmountText.text;

        _sendToPrinterNotification.SetActive(false);
        _selectedPrinter = null;
        GetPrinters();
    }

    public void Print(PrintQR printQR)
    {
        if(SelectedPrinter == null)
        {
            return;
        }

        CreateProductRequest productRequest = new CreateProductRequest();
        productRequest.token = ClientInfo.token;
        productRequest.amount = int.Parse(printQR.AmountText.text);
        productRequest.data = new ProductData();

        productRequest.data.serial_number = int.Parse(printQR.StartSerialNumberText.text);
        productRequest.data.size = printQR.SizeText.text;
        productRequest.data.qty = 111;
        productRequest.data.material_id = printQR.QRInfo.material.id;
        productRequest.data.slot_id = printQR.QRInfo.slotID;
        productRequest.data.destination_partner_id = printQR.QRInfo.destinationID;
        

        var jsonDataToSend = JsonConvert.SerializeObject(productRequest);
        DataJsonForRequest = jsonDataToSend;

        onSuccesRequest = OnSuccesPrintRequset;
        onProtocolErrorRequest = OnProtocolPrintError;

        StartCoroutine(SendPostRequest(UrlAddresses.CreateProduct));
    }

    

    private void OnProtocolPrintError()
    {
        
    }

    private void OnSuccesPrintRequset(string webResponse)
    {
        Debug.Log(webResponse);
        CreatedProductResponse createdProductResponse = JsonUtility.FromJson<CreatedProductResponse>(webResponse);

        CreateTaskRequest createTaskRequest = new CreateTaskRequest();

        createTaskRequest.token = ClientInfo.token;
        createTaskRequest.printer_id = SelectedPrinter.PrinterInfo.id;
        createTaskRequest.product_id = createdProductResponse.data[0].id;
        createTaskRequest.product_token = createdProductResponse.data[0].token;

        var jsonDataToSend = JsonConvert.SerializeObject(createTaskRequest);
        DataJsonForRequest = jsonDataToSend;

        onSuccesRequest = OnSuccesTaskRequset;
        onProtocolErrorRequest = OnProtocolTaskError;
        
        Debug.Log(DataJsonForRequest);
        StartCoroutine(SendPostRequest(UrlAddresses.CreatePrinterTask));
    }

    private void OnProtocolTaskError()
    {
        Debug.Log("webResponse error");
    }

    private void OnSuccesTaskRequset(string webResponse)
    {
        _sendToPrinterNotification.SetActive(true);
        Debug.Log(webResponse);
    }

    public void GetPrinters()
    {
        onSuccesRequest = OnSuccesGetPrintersRequset;
        onProtocolErrorRequest = OnProtocolGetPrintersError;

        string getRequestString = $"{UrlAddresses.GetPrinters}?token={ClientInfo.token}";
        StartCoroutine(SendGetRequest(getRequestString));
    }

    private void OnProtocolGetPrintersError()
    {

    }

    private void OnSuccesGetPrintersRequset(string webResponse)
    {
        Debug.Log(webResponse);
        Printers allPrinters = JsonUtility.FromJson<Printers>(webResponse);

        var printersObjects = _printersParent.GetComponentsInChildren<PrinterPrefab>(true);

        if (printersObjects != null)
        {
            foreach (var child in printersObjects)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (var printer in allPrinters.printers)
        {
            PrinterPrefab printerPrefab = Instantiate(_printerPrefab, _printersParent);
            printerPrefab.SetPrinterInfo(printer);
            printerPrefab.PrintWindow = this;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_printersParent as RectTransform);
    }

    public void SelectPrinter(PrinterPrefab printer)
    {
        if(_selectedPrinter == printer) return;

        if (_selectedPrinter != null)
            _selectedPrinter.DeselectPrinter();

        _selectedPrinter = printer;
    }
}

[Serializable]
public class Printers
{
    public PrinterInfo[] printers;
}
[Serializable]
public class PrinterInfo
{
    public int id;
    public string ip;
    public string name;
    public int port;
}

[Serializable]
public class CreateTaskRequest
{
    public string token;
    public int printer_id;
    public int product_id;
    public string product_token;
}

[Serializable]
public class CreatedProductResponse
{
    public ProductResponse[] data;
}

[Serializable]
public class ProductResponse
{
    public int id;
    public string token;
}