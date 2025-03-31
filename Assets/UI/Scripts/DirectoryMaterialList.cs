using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DirectoryMaterialList : UIWindow
{
    [SerializeField]
    private Transform _listParent;
    [SerializeField]
    private GameObject _categoryElementPrefab;
    [SerializeField]
    private GameObject _subCategoryElementPrefab;
    [SerializeField]
    private GameObject _materialElementPrefab;
    [SerializeField]
    private List<GameObject> _gameObjects = new List<GameObject>();
    [SerializeField]
    private GameObject _deleteConfirmPanel;
    [SerializeField]
    private Button _confirmDelete;
    [SerializeField]
    private Button _cancelDelete;
    [SerializeField]
    private List<GameObject> _deleteButtons = new List<GameObject>();

    //private PrintQR _printQR;
    private MaterialsData _materialsData;
    [SerializeField]
    private List<LocalCategory> _categories = new List<LocalCategory>();

    [SerializeField]
    private DirectoryMaterialSizeList _directoryMaterialSizeList;


    private void OnEnable()
    {
        //UpdateData();
    }

    public void UpdateData()
    {
        onSuccesRequest = OnSuccesGetData;
        onProtocolErrorRequest = OnProtocolError;
        //onTryAgain = Login;

        string getRequestString = $"{UrlAddresses.MaterialsData}?token={ClientInfo.token}";
        StartCoroutine(SendGetRequest(getRequestString));
    }

    private void OnProtocolError()
    {
        
    }

    private void OnSuccesGetData(string webResponse)
    {
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


        if (_gameObjects.Count > 0)
        {
            foreach (var go in _gameObjects)
            {
                Destroy(go);
            }
        }
        _gameObjects.Clear();
        _deleteButtons.Clear();
        CreateList();
    }

    public void CreateList()
    {
        foreach (var category in _categories)
        {
            GameObject categoryGO = Instantiate(_categoryElementPrefab, _listParent);
            categoryGO.GetComponentInChildren<TextMeshProUGUI>().text = category.name;
            _gameObjects.Add(categoryGO);
            
            Destroy(categoryGO.GetComponentInChildren<Button>().gameObject);

            foreach (var subCategory in category.SubCategories)
            {
                GameObject subCategoryGO = Instantiate(_subCategoryElementPrefab, categoryGO.transform);
                subCategoryGO.GetComponentInChildren<TextMeshProUGUI>().text = subCategory.name;
                _gameObjects.Add(subCategoryGO);

                Destroy(subCategoryGO.GetComponentInChildren<Button>().gameObject);

                if (subCategory.materials.Count > 0)
                    foreach (var material in subCategory.materials)
                        AddMaterialElement(subCategoryGO.transform, material.name, material.id, subCategory.name, material.is_custom, material);
            }

            if (category.materials.Count > 0)
                foreach (var material in category.materials)
                    AddMaterialElement(categoryGO.transform, material.name, material.id, category.name, material.is_custom, material);
        }
    }

    private void AddMaterialElement(Transform parent, string materialName, int id, string categoryName, bool is_custom, MaterialWithCategory materialWithCategory)
    {
        var material = Instantiate(_materialElementPrefab, parent).GetComponent<ListElement>();

        Destroy(material.DeleteButton.gameObject);

        material.SetNameText(materialName);
        material.SetID(id);
        material.SetCategoryName(categoryName);
        material.SetDefaultTextColor();

        //material.GetComponent<Button>().onClick.AddListener(_backButton.onClick.Invoke);
        material.SetWindowToSendMaterialData(_previousWindow);
        //material.GetComponent<Button>().onClick = _action;
        material.GetComponent<Button>().onClick.AddListener(() =>  
        {
            _directoryMaterialSizeList.gameObject.SetActive(true);
            _directoryMaterialSizeList.SetPreviousWindow(this);
            _directoryMaterialSizeList.Material = materialWithCategory;
            _directoryMaterialSizeList.MaterialsData = _materialsData;
            _directoryMaterialSizeList.CreateListOfSizes();
        });
    }


}