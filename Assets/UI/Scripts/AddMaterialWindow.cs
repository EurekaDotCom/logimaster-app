using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddMaterialWindow : UIWindow
{
    [SerializeField]
    private Button _addMaterialButton;

    [SerializeField]
    private SelectMaterial _selectMaterial = null;
    [SerializeField]
    private TextMeshProUGUI _parentCategoryText;
    [SerializeField]
    private TMP_InputField _matrialName;
    [SerializeField]
    private TMP_InputField _matrialDescription;
    private int? _categoryID = null;

    public void SetParrentCategory(string parentCategoryName, int? categoryID)
    {
        _parentCategoryText.text = parentCategoryName;
        _categoryID = categoryID;
    }

    public void AddNewCustomMaterial()
    {
        if (_matrialName.text == "" || _parentCategoryText.text == "" || _parentCategoryText.text == "none")
        {
            Debug.Log("empty fields");
            return;
        }

        CreateMaterialRequest _createMaterialRequest = new CreateMaterialRequest();
        _createMaterialRequest.token = ClientInfo.token;
        _createMaterialRequest.name = _matrialName.text;
        _createMaterialRequest.category_id = _categoryID;
        

        var jsonDataToSend = JsonConvert.SerializeObject(_createMaterialRequest);
        DataJsonForRequest = jsonDataToSend;

        onSuccesRequest = OnSuccesRequset;
        onProtocolErrorRequest = OnProtocolError;

        StartCoroutine(SendPostRequest(UrlAddresses.CreateMaterial));
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
