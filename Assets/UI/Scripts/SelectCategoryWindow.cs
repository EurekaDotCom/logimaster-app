using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCategoryWindow : UIWindow
{
    [SerializeField]
    private Button _noneCategoryButton;
    [SerializeField]
    private PrintQR _printQR;
    [SerializeField]
    private Transform _categoryParent;
    [SerializeField]
    private GameObject _categoryPrefab;

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

    private void Start()
    {
        _noneCategoryButton.onClick.AddListener(SetToNone);
        _noneCategoryButton.onClick.AddListener(_backButton.onClick.Invoke);
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
        }
    }

    public void SetToNone()
    {
        if (PreviousWindow as AddCategoryWindow)
        {
            (PreviousWindow as AddCategoryWindow).SetParentCategoryToNone();
        }
    }

    private void SelectCategoryParent(string name, int parentID)
    {
        if(PreviousWindow as AddCategoryWindow)
        {
            (PreviousWindow as AddCategoryWindow).SetParrentCategory(name, parentID);
        }
    }
}
