using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectMaterial : UIWindow
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

    private PrintQR _printQR;


    private void OnEnable()
    {
        if (_printQR == null && PreviousWindow as PrintQR)
        {
            _printQR = PreviousWindow as PrintQR;
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
        foreach (var category in _printQR.Categories)
        {
            GameObject categoryGO = Instantiate(_categoryElementPrefab, _listParent);
            categoryGO.GetComponentInChildren<TextMeshProUGUI>().text = category.name;
            _gameObjects.Add(categoryGO);

            if (category.is_custom)
            {
                categoryGO.GetComponentInChildren<Button>().onClick.AddListener(() => DeleteCategory(category.id));
                _deleteButtons.Add(categoryGO.GetComponentInChildren<Button>().gameObject);
                categoryGO.GetComponentInChildren<Button>().gameObject.SetActive(false);
            }
            else Destroy(categoryGO.GetComponentInChildren<Button>().gameObject);

            foreach (var subCategory in category.SubCategories)
            {
                GameObject subCategoryGO = Instantiate(_subCategoryElementPrefab, categoryGO.transform);
                subCategoryGO.GetComponentInChildren<TextMeshProUGUI>().text = subCategory.name;
                _gameObjects.Add(subCategoryGO);

                if (subCategory.is_custom)
                {
                    subCategoryGO.GetComponentInChildren<Button>().onClick.AddListener(() => DeleteCategory(subCategory.id));
                    _deleteButtons.Add(subCategoryGO.GetComponentInChildren<Button>().gameObject);
                    subCategoryGO.GetComponentInChildren<Button>().gameObject.SetActive(false);
                }
                else Destroy(subCategoryGO.GetComponentInChildren<Button>().gameObject);

                if (subCategory.materials.Count > 0)
                    foreach (var material in subCategory.materials)
                        AddMaterialElement(subCategoryGO.transform, material.name, material.id, subCategory.name, material.is_custom);
            }

            if (category.materials.Count > 0)
                foreach (var material in category.materials)
                    AddMaterialElement(categoryGO.transform, material.name, material.id, category.name, material.is_custom);
        }
    }

    private void AddMaterialElement(Transform parent, string materialName, int id, string categoryName, bool is_custom)
    {
        var material = Instantiate(_materialElementPrefab, parent).GetComponent<ListElement>();

        if (is_custom)
        {
            material.DeleteButton.onClick.AddListener(() => DeleteMaterial(id));
            _deleteButtons.Add(material.DeleteButton.gameObject);
            material.DeleteButton.gameObject.SetActive(false);
        }
        else Destroy(material.DeleteButton.gameObject);

        material.SetNameText(materialName);
        material.SetID(id);
        material.SetCategoryName(categoryName);
        material.SetDefaultTextColor();

        material.GetComponent<Button>().onClick.AddListener(_backButton.onClick.Invoke);
        material.SetWindowToSendMaterialData(_previousWindow);
    }


    #region Deletion

    public void DeleteCategory(int categoryID)
    {
        _deleteConfirmPanel.gameObject.SetActive(true);
        _confirmDelete.onClick.AddListener(() => ConfirmCategoryDeletion(categoryID));
    }

    public void DeleteMaterial(int materialID)
    {
        _deleteConfirmPanel.gameObject.SetActive(true);
        _confirmDelete.onClick.AddListener(() => ConfirmMaterialDeletion(materialID));
    }

    private void ConfirmCategoryDeletion(int id)
    {
        DeleteCategoryRequest request = new DeleteCategoryRequest();
        request.token = ClientInfo.token;
        request.id = id;
        request.detach = false;

        Debug.Log(id.ToString());

        var jsonDataToSend = JsonConvert.SerializeObject(request);
        DataJsonForRequest = jsonDataToSend;

        onSuccesRequest = OnSuccesRequest;
        onProtocolErrorRequest = OnProtocolError;

        StartCoroutine(SendPostRequest(UrlAddresses.DeleteMaterialCategory));
    }

    private void ConfirmMaterialDeletion(int id)
    {
        DeleteMaterialRequest request = new DeleteMaterialRequest();
        request.token = ClientInfo.token;
        request.id = id;

        Debug.Log(id.ToString());

        var jsonDataToSend = JsonConvert.SerializeObject(request);
        DataJsonForRequest = jsonDataToSend;

        onSuccesRequest = OnSuccesRequest;
        onProtocolErrorRequest = OnProtocolError;

        StartCoroutine(SendPostRequest(UrlAddresses.DeleteMaterial));
    }

    private void OnProtocolError()
    {

    }

    private void OnSuccesRequest(string webResponse)
    {
        _printQR.UpdateData();
        CloseDeleteConfirmationPanel();
        _backButton.onClick.Invoke();
    }

    public void CloseDeleteConfirmationPanel()
    {
        _deleteConfirmPanel.gameObject.SetActive(false);
        _confirmDelete.onClick.RemoveAllListeners();
    }

    public void EnableAndDisableDeleteButtons()
    {
        foreach (var button in _deleteButtons)
        {
            button.SetActive(!button.activeSelf);
        }
    }
    #endregion
}