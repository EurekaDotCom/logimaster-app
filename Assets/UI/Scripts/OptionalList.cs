using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionalList : UIWindow
{
    [SerializeField]
    private Transform _listParent;
    [SerializeField]
    private GameObject _listElementPrefab;
    [SerializeField]
    private GameObject _resizeButton;
    [SerializeField]
    private TextMeshProUGUI _headerText;

    [SerializeField]
    private GameObject _deleteConfirmPanel;
    [SerializeField]
    private Button _confirmDelete;
    [SerializeField]
    private Button _cancelDelete;
    [SerializeField]
    private List<GameObject> _deleteButtons = new List<GameObject>();

    private PrintQR _printQR = null;

    private List<GameObject> _currentList = new List<GameObject>();
    private MaterialOption[] _materialOption;


    private void OnEnable()
    {
        if (_printQR == null)
            _printQR = _previousWindow as PrintQR;
        _deleteButtons.Clear();
    }

    public void CreateListOfSizes()
    {
        ClearCurrentList();

        if (_printQR == null)
            _printQR = _previousWindow as PrintQR;

        if (_printQR.QRInfo.material == null)
        {
            _backButton.onClick.Invoke();
            return;
        }

        _materialOption = _printQR.FindOptionsByMaterial(_printQR.QRInfo.material.id);
        _headerText.text = _printQR.QRInfo.material.name;
        foreach (var option in _materialOption)
        {
            ListElement destinationVariant = Instantiate(_listElementPrefab, _listParent, false).GetComponent<ListElement>();
            _currentList.Add(destinationVariant.gameObject);
            destinationVariant.SetNameText(option.name);
            destinationVariant.SetDefaultTextColor();

            destinationVariant.GetComponent<Button>().onClick.AddListener(_backButton.onClick.Invoke);
            destinationVariant.SetWindowToSendData(_previousWindow);

            if (option.custom)
            {
                _deleteButtons.Add(destinationVariant.DeleteButton.gameObject);
                destinationVariant.DeleteButton.onClick.AddListener(() => DeleteOption(option.id));
            }
            else Destroy(destinationVariant.DeleteButton.gameObject);
        }
        SetCustomSizeButtonAtAndOfTheList();
    }

    public void CreateListOfSlots()
    {
        _resizeButton.SetActive(false);
        ClearCurrentList();

        if (_printQR == null)
            _printQR = _previousWindow as PrintQR;

        if (_printQR.QRInfo.warehouse_id == null)
        {
            _backButton.onClick.Invoke();
            return;
        }

        _headerText.text = _printQR.QRInfo.warehouse_name;
        foreach (var slot in _printQR.LanesData.data.lanes)
        {
            ListElement destinationVariant = Instantiate(_listElementPrefab, _listParent, false).GetComponent<ListElement>();
            _currentList.Add(destinationVariant.gameObject);
            destinationVariant.SetNameText(slot.name);
            destinationVariant.SetDefaultTextColor();

            destinationVariant.GetComponent<Button>().onClick.AddListener(_backButton.onClick.Invoke);
            destinationVariant.SetWindowToSendData(_previousWindow, slot.id);
            destinationVariant.DeleteButton.gameObject.SetActive(false);
        }
    }

    private void ClearCurrentList()
    {
        if (_currentList.Count <= 0) return; 
        foreach (var item in _currentList)
        {
            Destroy(item.gameObject);
        }
        _currentList.Clear();
    }

    private void SetCustomSizeButtonAtAndOfTheList()
    {
        _resizeButton.SetActive(true);
        _resizeButton.transform.SetParent(null);
        _resizeButton.transform.SetParent(_listParent);
    }

    #region Deletion
    public void DeleteOption(int optionID)
    {
        _deleteConfirmPanel.gameObject.SetActive(true);
        _confirmDelete.onClick.AddListener(() => ConfirmOptionDeletion(optionID));
    }

    private void ConfirmOptionDeletion(int id)
    {
        DeleteOptionRequest request = new DeleteOptionRequest();
        request.token = ClientInfo.token;
        request.id = id;

        Debug.Log(id.ToString());

        var jsonDataToSend = JsonConvert.SerializeObject(request);
        DataJsonForRequest = jsonDataToSend;

        onSuccesRequest = OnSuccesRequest;
        onProtocolErrorRequest = OnProtocolError;

        StartCoroutine(SendPostRequest(UrlAddresses.DeleteOption));
    }

    private void OnProtocolError()
    {
        
    }

    private void OnSuccesRequest(string webResponse)
    {
        CustomUtils.TextAndColorStruct clearSize = new CustomUtils.TextAndColorStruct();
        clearSize.text = "";
        clearSize.color = Color.black;
        _printQR.SetSize(clearSize);

        _printQR.UpdateData();
        CloseDeleteConfirmationPanel();
        _backButton.onClick.Invoke();
    }

    public void CloseDeleteConfirmationPanel()
    {
        _deleteConfirmPanel.gameObject.SetActive(false);
        _confirmDelete.onClick.RemoveAllListeners();
    }

    #endregion
}
