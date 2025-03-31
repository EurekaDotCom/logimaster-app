using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DirectoryMaterialSizeList : UIWindow
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

    private List<GameObject> _currentList = new List<GameObject>();
    private MaterialOption[] _materialOption;

    private MaterialWithCategory _material;
    public MaterialWithCategory Material {  get { return _material; } set { _material = value; } }

    private MaterialsData _materialsData;
    public MaterialsData MaterialsData {  get { return _materialsData; } set { _materialsData = value; } }

    private void OnEnable()
    {
        _deleteButtons.Clear();
    }

    public void CreateListOfSizes()
    {
        ClearCurrentList();

        if (_material == null)
        {
            _backButton.onClick.Invoke();
            return;
        }

        _materialOption = FindOptionsByMaterial(_material.id);
        _headerText.text = _material.name;
        foreach (var option in _materialOption)
        {
            ListElement destinationVariant = Instantiate(_listElementPrefab, _listParent, false).GetComponent<ListElement>();
            _currentList.Add(destinationVariant.gameObject);
            destinationVariant.SetNameText(option.name);
            destinationVariant.SetDefaultTextColor();


            Destroy(destinationVariant.DeleteButton.gameObject);
        }
        SetCustomSizeButtonAtAndOfTheList();
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
        _resizeButton.SetActive(false);
        //_resizeButton.transform.SetParent(null);
        //_resizeButton.transform.SetParent(_listParent);
    }
}
