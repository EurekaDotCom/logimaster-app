using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AddCategoryWindow : UIWindow
{
    [SerializeField]
    private SelectMaterial _selectMaterial = null;
    [SerializeField]
    private bool _isHasParentCategory = false;
    public bool IsHasParentCategory { get { return _isHasParentCategory; } set { _isHasParentCategory = value; } }

    private int? _parentID = null;

    [SerializeField]
    private TextMeshProUGUI _parentCategoryText;
    [SerializeField]
    private TMP_InputField _categoryName;

    public void SetParentCategoryToNone()
    {
        _isHasParentCategory = false;
        _parentCategoryText.text = "none";
        _parentID = null;
    }

    public void SetParrentCategory(string parentCategoryName, int parentID)
    {
        _isHasParentCategory = true;
        _parentCategoryText.text = parentCategoryName;
        _parentID = parentID;
    }


    public void AddNewCategory() 
    {
        if (_categoryName.text != "")
        {
            CreateCategoryRequest _createCategoryRequest = new CreateCategoryRequest();
            _createCategoryRequest.token = ClientInfo.token;
            _createCategoryRequest.name = _categoryName.text;
            _createCategoryRequest.parent_id = _parentID;

            var jsonDataToSend = JsonConvert.SerializeObject(_createCategoryRequest);
            DataJsonForRequest = jsonDataToSend;
            
            onSuccesRequest = OnSuccesRequset;
            onProtocolErrorRequest = OnProtocolError;

            StartCoroutine(SendPostRequest(UrlAddresses.CreateMaterialCategory));
        }
    }

    private void OnProtocolError()
    {

    }

    private void OnSuccesRequset(string webResponse)
    {
        if (_selectMaterial.PreviousWindow as PrintQR)
        {
            (_selectMaterial.PreviousWindow as PrintQR).UpdateData();
            _backButton.onClick.Invoke();
        }
    }
}
