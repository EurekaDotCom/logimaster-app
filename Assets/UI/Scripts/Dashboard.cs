using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Dashboard : UIWindow
{
    [SerializeField]
    private GameObject _mainDashboard;
    [SerializeField]
    private GameObject _licenseError;
    [SerializeField]
    private TextMeshProUGUI _companyNameText;
    [SerializeField]
    private TextMeshProUGUI _headerText;
    [SerializeField]
    private int _firstStringPartID;
    [SerializeField]
    private int _lastStringPartID;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        LanguageContainer currentLangContainer = LanguageManger.Instance.languages.Find(e => e.LanguageId == LanguageManger.Instance.CurrentLangueID);


        string s = $"{currentLangContainer.Strings[_firstStringPartID]} {ClientInfo.username}, {currentLangContainer.Strings[_lastStringPartID]}";
        _headerText.text = s;
        _companyNameText.text = ClientInfo.company_name;
    }

    #region LogOut
    public void LogOut()
    {
        Token tok = new Token();
        tok.token = ClientInfo.token;
        var jsonDataToSend = JsonConvert.SerializeObject(tok);
        DataJsonForRequest = jsonDataToSend;
        onSuccesRequest = OnSuccesLogOut;
        onProtocolErrorRequest = OnProtocolErrorLogOut;
        onTryAgain = LogOut;
        StartCoroutine(SendPostRequest(UrlAddresses.LogOutRequest));
    }

    private void OnSuccesLogOut(string response)
    {
        GoBackToPreviousWindow();
    }
    private void OnProtocolErrorLogOut()
    {
        Debug.Log("Error Log Out");
    }
    #endregion

}
