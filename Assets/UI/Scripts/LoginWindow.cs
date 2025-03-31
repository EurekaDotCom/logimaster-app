using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public delegate void OnSuccesRequest(string webResponse);
public delegate void OnProtocolErrorRequest();

public class LoginWindow : UIWindow
{
    [SerializeField]
    private TMP_InputField _emailInputField;
    [SerializeField]
    private TMP_InputField _passwordInputField;
    [SerializeField]
    private UIWindow _dashboard;
    [SerializeField]
    private GameObject _loginErrorGameObject;
    [SerializeField]
    private GameObject _passwordErrorGameObject;
    [SerializeField]
    private GameObject _textErrorGameObject;

    public PostLoginRequest postLoginRequest = new PostLoginRequest();

    #region Login
    public void Login()
    {
        DisableErrors();
        if (CheckConditionAndConvertData())
        {
            onSuccesRequest = OnSuccesLogin;
            onProtocolErrorRequest = OnProtocolErrorLogin;
            onTryAgain = Login;
            StartCoroutine(SendPostRequest(UrlAddresses.LoginRequest));
        }
    }

    private void OnSuccesLogin(string response)
    {
        LoginPostResponse loginPostResponse = new LoginPostResponse();
        loginPostResponse = ConvertLoginRespons(response);

        ClientInfo.token = loginPostResponse.token;
        ClientInfo.email = postLoginRequest.email;
        ClientInfo.company_id = loginPostResponse.company_id;
        ClientInfo.username = loginPostResponse.username;
        ClientInfo.company_name = loginPostResponse.company_name;

        PlayerPrefs.SetString(GameKeys.Saved_Email, postLoginRequest.email);

        _dashboard.PreviousWindow = this;
        _dashboard.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnProtocolErrorLogin()
    {
        EnableErrors();
    }
    #endregion

    private bool CheckConditionAndConvertData()
    {
        if (_emailInputField.text != "" && _passwordInputField.text != "")
        {
            postLoginRequest.email = _emailInputField.text;
            postLoginRequest.password = _passwordInputField.text;

            var jsonDataToSend = JsonConvert.SerializeObject(postLoginRequest);
            DataJsonForRequest = jsonDataToSend;

            return true;
        }
        return false;
    }

    private void EnableErrors()
    {
        _loginErrorGameObject.SetActive(true);
        _passwordErrorGameObject.SetActive(true);
        _textErrorGameObject.SetActive(true);
    }

    private void DisableErrors()
    {
        _loginErrorGameObject.SetActive(false);
        _passwordErrorGameObject.SetActive(false);
        _textErrorGameObject.SetActive(false);
    }

    private LoginPostResponse ConvertLoginRespons(string json)
    {
        LoginPostResponse loginResponse = JsonUtility.FromJson<LoginPostResponse>(json);
        return loginResponse;
    }
}
