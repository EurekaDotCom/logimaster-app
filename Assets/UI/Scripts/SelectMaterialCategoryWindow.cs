using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectMaterialCategoryWindow : UIWindow
{
    [SerializeField]
    private PrintQR _printQR;
    [SerializeField]
    private Transform _categoryParent;
    [SerializeField]
    private GameObject _categoryPrefab;
    [SerializeField]
    private GameObject _subCategoryPrefab;

    [SerializeField]
    private List<GameObject> _gameObjects = new List<GameObject>();


    private void OnEnable()
    {
        if (_gameObjects.Count > 0)
        {
            foreach (var go in _gameObjects)
            {
                Destroy(go);
            }
        }
        _gameObjects.Clear();
        CreateList();
    }

    public void CreateList()
    {
        foreach (var category in _printQR.Categories)
        {
            GameObject categoryGO = Instantiate(_categoryPrefab, _categoryParent);
            _gameObjects.Add(categoryGO);
            categoryGO.GetComponentInChildren<TextMeshProUGUI>().text = category.name;
            categoryGO.GetComponent<Button>().onClick.AddListener(() => { SelectCategoryParent(category.name, category.id); });
            categoryGO.GetComponent<Button>().onClick.AddListener(_backButton.onClick.Invoke);
            foreach (var subCategory in category.SubCategories)
            {
                GameObject subCategoryGO = Instantiate(_subCategoryPrefab, categoryGO.transform);
                _gameObjects.Add(subCategoryGO);
                subCategoryGO.GetComponentInChildren<TextMeshProUGUI>().text = subCategory.name;
                subCategoryGO.GetComponent<Button>().onClick.AddListener(() => { SelectCategoryParent(subCategory.name, subCategory.id); });
                subCategoryGO.GetComponent<Button>().onClick.AddListener(_backButton.onClick.Invoke);
            }
        }
    }

    private void SelectCategoryParent(string name, int? categoryID = null)
    {
        if (PreviousWindow as AddMaterialWindow)
        {
            (PreviousWindow as AddMaterialWindow).SetParrentCategory(name, categoryID);
        }
    }
}
