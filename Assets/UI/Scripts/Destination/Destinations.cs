using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Destinations : UIWindow
{
    [SerializeField]
    private GameObject _listElementPrefab;
    [SerializeField]
    private Transform _listComponentParent;
    [SerializeField]
    private QRScreenWindow _qrScreenWindow;
    [SerializeField]
    private PrintQR _printQR;

    [SerializeField]
    private List<GameObject> _destinationList = new List<GameObject>();
    [SerializeField]
    private Button _noneDestinationButton;


    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_previousWindow as PrintQR)
        {
            _noneDestinationButton.GetComponent<DestinationVariant>().SetPrintQR(_previousWindow as PrintQR);
            _noneDestinationButton.onClick.AddListener(_backButton.onClick.Invoke);
        }
    }

    public void CreatePartnersList(bool hasNoneButton)
    {
        if (_destinationList.Count > 0)
        {
            foreach (GameObject item in _destinationList)
                Destroy(item);

            _destinationList.Clear();
        }

        foreach (var partner in _printQR.PartnersData.data.partners)
        {
            DestinationVariant destinationVariant = Instantiate(_listElementPrefab, _listComponentParent, false).GetComponent<DestinationVariant>();
            destinationVariant.SetNameText(partner.company_name);
            destinationVariant.SetTextColor(DestinationColor.Company);

            if (_previousWindow as QRScreenWindow)
                destinationVariant.SetQRScreenWindow(_previousWindow as QRScreenWindow);
            else if (_previousWindow as PrintQR)
                destinationVariant.SetPrintQR(_previousWindow as PrintQR);

            destinationVariant.GetComponent<Button>().onClick.AddListener(destinationVariant.SetDestination);
            destinationVariant.GetComponent<Button>().onClick.AddListener(_backButton.onClick.Invoke);

            _destinationList.Add(destinationVariant.gameObject);
        }

        _noneDestinationButton.gameObject.SetActive(hasNoneButton);
    }

    public void CreateWarehousesList(bool hasNoneButton)
    {
        if (_destinationList.Count > 0)
        {
            foreach (GameObject item in _destinationList)
                Destroy(item);

            _destinationList.Clear();
        }

        foreach (var warehouse in _printQR.WarehousesData.data.warehouses)
        {
            DestinationVariant destinationVariant = Instantiate(_listElementPrefab, _listComponentParent, false).GetComponent<DestinationVariant>();
            destinationVariant.SetNameText(warehouse.name);
            destinationVariant.SetTextColor(DestinationColor.Warehouse);

            if (_previousWindow as QRScreenWindow)
                destinationVariant.SetQRScreenWindow(_previousWindow as QRScreenWindow);
            else if (_previousWindow as PrintQR)
            {
                destinationVariant.SetPrintQR(_previousWindow as PrintQR);

                destinationVariant.GetComponent<Button>().onClick.AddListener(() =>
                {
                    _printQR.QRInfo.warehouse_id = warehouse.id;
                    _printQR.QRInfo.warehouse_name = warehouse.name;
                    _printQR.UpdateSlotsData();
                    _printQR.WarehouseText.text = warehouse.name;
                });
            }
                
            destinationVariant.GetComponent<Button>().onClick.AddListener(_backButton.onClick.Invoke);

            _destinationList.Add(destinationVariant.gameObject);
        }

        _noneDestinationButton.gameObject.SetActive(hasNoneButton);
    }
}
