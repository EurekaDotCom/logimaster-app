using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PrintQR : UIWindow
{
    [SerializeField]
    private TMP_InputField _skuText;
    public TMP_InputField SKUText => _skuText;
    [SerializeField]
    private TMP_InputField _startSerialNumberText;
    public TMP_InputField StartSerialNumberText => _startSerialNumberText;
    [SerializeField]
    private TMP_InputField _amountText;
    public TMP_InputField AmountText => _amountText;
    [SerializeField]
    private TextMeshProUGUI _warehouseText;
    public TextMeshProUGUI WarehouseText => _warehouseText;
    [SerializeField]
    private TextMeshProUGUI _slotText;
    [SerializeField]
    private TextMeshProUGUI _destinationText;
    [SerializeField]
    private TextMeshProUGUI _materialText;
    [SerializeField]
    private TextMeshProUGUI _sizeText;
    public TextMeshProUGUI SizeText => _sizeText;
    [SerializeField]
    private Image _productImage;

    #region Visuals
    [SerializeField]
    private List<Image> _imagesThatChangeColor;
    [SerializeField]
    private List<TextMeshProUGUI> _textThatChangeColor;
    [SerializeField]
    private Image _printButton;
    [SerializeField]
    private Sprite _partnerSprite;
    [SerializeField]
    private Sprite _warehouseSprite;
    #endregion

    [SerializeField]
    private CustomUtils.WindowFieldCategory _selectedCategoty;

    private QRInfo _qrInfo = new QRInfo();
    public QRInfo QRInfo { get { return _qrInfo; } }

    [SerializeField]
    private List<LocalCategory> _categories = new List<LocalCategory>();
    public List<LocalCategory> Categories => _categories;

    private MaterialsData _materialsData;
    [SerializeField]
    private GetPartnersRespond _partnersData = new GetPartnersRespond();
    public GetPartnersRespond PartnersData => _partnersData;

    [SerializeField]
    private GetWarehousesRespond _warehousesData = new GetWarehousesRespond();
    public GetWarehousesRespond WarehousesData => _warehousesData;

    [SerializeField]
    private GetLanesRespond _lanesData = new GetLanesRespond();
    public GetLanesRespond LanesData => _lanesData;
    //public MaterialsData MaterialsData => _materialsData;


    private void OnEnable()
    {
        //UpdateData();
    }

    private void Start()
    {
#if PLATFORM_ANDROID || UNITY_ANDROID
         
        _productImage.rectTransform.rotation = new Quaternion(0f, 0f, -0.707106829f, 0.707106829f);
#endif
    }

    public void UpdateData()
    {
        onSuccesRequest = OnSuccesGetData;
        onProtocolErrorRequest = OnProtocolError;
        //onTryAgain = Login;

        string getRequestString = $"{UrlAddresses.MaterialsData}?token={ClientInfo.token}";
        StartCoroutine(SendGetRequest(getRequestString));
    }
    
    public void UpdatePartnersData()
    {
        onSuccesRequest = OnSuccesGetPartnersData;
        onProtocolErrorRequest = OnProtocolError;

        string getRequestString = $"{UrlAddresses.GetPartners}?token={ClientInfo.token}";
        StartCoroutine(SendGetRequest(getRequestString));
    }

    public void UpdateWarehousesData()
    {
        onSuccesRequest = OnSuccesGetWarehousesData;
        onProtocolErrorRequest = OnProtocolError;

        string getRequestString = $"{UrlAddresses.GetWarehouses}?token={ClientInfo.token}";
        StartCoroutine(SendGetRequest(getRequestString));
    }

    public void UpdateSlotsData()
    {
        if (_qrInfo.warehouse_id == null)
            return;

        onSuccesRequest = OnSuccesGetSlotsData;
        onProtocolErrorRequest = OnProtocolError;

        string getRequestString = $"{UrlAddresses.GetLanes}?token={ClientInfo.token}&warehouse_id={_qrInfo.warehouse_id}";
        StartCoroutine(SendGetRequest(getRequestString));
    }

    private void OnProtocolError()
    {

    }

    private void OnSuccesGetData(string webResponse)
    {
        UpdatePartnersData();
        _materialsData = JsonUtility.FromJson<MaterialsData>(webResponse);
        _categories.Clear();
        Debug.Log(webResponse);
        foreach (var category in _materialsData.data.categories)
        {
            var localCategory = new LocalCategory();

            localCategory.name = category.name;
            localCategory.company_id = category.company_id;
            localCategory.id = category.id;
            localCategory.is_custom = category.custom;
            localCategory.SubCategories = new List<SubCategory>();
            localCategory.materials = new List<MaterialWithCategory>();

            foreach (var material in category.materials)
            {
                var catMaterial = new MaterialWithCategory();
                catMaterial.id = material.id;
                catMaterial.name = material.name;
                catMaterial.is_custom = material.custom;

                localCategory.materials.Add(catMaterial);
            }

            foreach (var subCategory in category.categories)
            {
                var localSubcategory = new SubCategory();

                localSubcategory.name = subCategory.name;
                localSubcategory.company_id = subCategory.company_id;
                localSubcategory.id = subCategory.id;
                localSubcategory.is_custom = subCategory.custom;
                localSubcategory.materials = new List<MaterialWithCategory>();

                foreach (var material in subCategory.materials)
                {
                    var subCatMaterial = new MaterialWithCategory();
                    subCatMaterial.id = material.id;
                    subCatMaterial.name = material.name;
                    subCatMaterial.is_custom = material.custom;

                    localSubcategory.materials.Add(subCatMaterial);
                }

                localCategory.SubCategories.Add(localSubcategory);
            }

            _categories.Add(localCategory);
        }
    }

    private void OnSuccesGetPartnersData(string webResponse)
    {
        _partnersData = JsonUtility.FromJson<GetPartnersRespond>(webResponse);
        UpdateWarehousesData();
    }

    private void OnSuccesGetWarehousesData(string webResponse)
    {
        _warehousesData = JsonUtility.FromJson<GetWarehousesRespond>(webResponse);
    }

    private void OnSuccesGetSlotsData(string webResponse)
    {
        _lanesData = JsonUtility.FromJson<GetLanesRespond>(webResponse);
    }

    public MaterialOption[] FindOptionsByMaterial(int materialID)
    {
        foreach (var category in _materialsData.data.categories)
        {
            foreach (var material in category.materials.Where(material => material.id == materialID && material.options != null))
                return material.options;

            foreach (var material in category.categories.SelectMany(subCategory => subCategory.materials.Where(material => material.id == materialID && material.options != null)))
                return material.options;
        }

        return null;
    }

    public void SetSelectedCategory(int fieldCategoty)
    {
        _selectedCategoty = (CustomUtils.WindowFieldCategory)fieldCategoty;
    }

    public void SetSlot(CustomUtils.TextAndColorStruct tacStruct, int? slot_id)
    {
        switch (_selectedCategoty)
        {
            case CustomUtils.WindowFieldCategory.Slot:
                _slotText.text = tacStruct.text;
                _slotText.color = tacStruct.color;
                _qrInfo.slotID = slot_id;
                break;
            case CustomUtils.WindowFieldCategory.Destination:
                break;
            case CustomUtils.WindowFieldCategory.Material:
                break;
            case CustomUtils.WindowFieldCategory.Size:
                _sizeText.text = tacStruct.text;
                _sizeText.color = tacStruct.color;
                break;
            default:
                break;
        }
    }

    public void SetDestination(string destination, Color color, int? id)
    {
        bool isPartner = id != null;
        _qrInfo.destinationID = id;
        _destinationText.text = destination;
        foreach (var image in _imagesThatChangeColor) { image.color = color; }
        foreach (var text in _textThatChangeColor) { text.color = color; }
        if (isPartner)
            _printButton.sprite = _partnerSprite;
        else
            _printButton.sprite = _warehouseSprite;
    }

    public void SetSize(CustomUtils.TextAndColorStruct size)
    {
        _sizeText.text = size.text;
        _sizeText.color = size.color;
    }

    public void SetImage(Sprite sprite)
    {
        _productImage.gameObject.SetActive(true);
        _productImage.sprite = sprite;
    }

    public void DisableProductImage()
    {
        _productImage.gameObject.SetActive(false);
    }

    public void SetMaterialInfo(int id, string name, string categoryName)
    {
        MaterialWithCategory material = new MaterialWithCategory();
        _materialText.text = name;
        material.name = name;
        material.id = id;
        material.MaterialCategory = categoryName;
        _qrInfo.material = material;
    }
}

public class QRInfo
{
    public string warehouse_name = null;
    public int? warehouse_id = null;
    public int? slotID = null;
    public int? destinationID = null;
    public MaterialWithCategory material;
}

#region MaterialAndCategories

[Serializable]
public class LocalCategory
{
    public string name;
    public int company_id;
    public int id;
    public bool is_custom;
    public List<SubCategory> SubCategories;
    public List<MaterialWithCategory> materials;
}

[Serializable]
public class SubCategory
{
    public string name;
    public int company_id;
    public int id;
    public bool is_custom;
    public List<MaterialWithCategory> materials;
}

[Serializable]
public class MaterialWithCategory
{
    public int id;
    public string name;
    public bool is_custom;
    public string MaterialDescription;
    public string MaterialCategory;
    public bool isSubcategoryMaterial;
}
#endregion