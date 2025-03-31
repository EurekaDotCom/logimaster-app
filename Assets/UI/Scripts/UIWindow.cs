using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIWindow : MonoBehaviour
{
    [SerializeField]
    protected UIWindow _previousWindow;
    public UIWindow PreviousWindow { get { return _previousWindow; } set { _previousWindow = value; } }
    [SerializeField]
    protected Button _backButton;
    public Button BackButton { get { return _backButton;} }

    protected OnSuccesRequest onSuccesRequest;
    protected OnProtocolErrorRequest onProtocolErrorRequest;
    protected TryAgainRequest onTryAgain;

    private string _dataJsonForRequest;

    protected string DataJsonForRequest { get { return _dataJsonForRequest; } set { _dataJsonForRequest = value; } }


    protected void Awake()
    {
        if(_backButton != null)
        {
            _backButton.onClick.AddListener(GoBackToPreviousWindow);
        }
    }
    public void SetPreviousWindow(UIWindow prevWindow)
    {
        _previousWindow = prevWindow;
    }
    protected void GoBackToPreviousWindow()
    {
        if(_previousWindow != null)
        {
            _previousWindow.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    protected IEnumerator SendPostRequest(string url)
    {
        LoadingCanvas.Instance.Show();
        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        byte[] rawData = Encoding.UTF8.GetBytes(DataJsonForRequest);
        webRequest.uploadHandler = new UploadHandlerRaw(rawData);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        LoadingCanvas.Instance.Hide();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.InProgress:
                print("InProgress");
                break;
            case UnityWebRequest.Result.Success:
                print("Success");
                string respone = webRequest.downloadHandler.text;
                onSuccesRequest.Invoke(respone);
                break;
            case UnityWebRequest.Result.ConnectionError:
                print("ConnectionError");
                ErrorCanvas.Instance.ShowErrorPanel(onTryAgain);
                break;
            case UnityWebRequest.Result.ProtocolError:
                print("ProtocolError");
                Debug.Log(webRequest.downloadHandler.text);
                onProtocolErrorRequest.Invoke();
                break;
            case UnityWebRequest.Result.DataProcessingError:
                print("DataProcessingError");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected IEnumerator SendGetRequest(string url)
    {
        LoadingCanvas.Instance.Show();
        UnityWebRequest webRequest = new UnityWebRequest(url, "GET");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        LoadingCanvas.Instance.Hide();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.InProgress:
                print("InProgress");
                break;
            case UnityWebRequest.Result.Success:
                print("Success");
                string respone = webRequest.downloadHandler.text;
                onSuccesRequest.Invoke(respone);
                break;
            case UnityWebRequest.Result.ConnectionError:
                print("ConnectionError");
                ErrorCanvas.Instance.ShowErrorPanel(onTryAgain);
                break;
            case UnityWebRequest.Result.ProtocolError:
                print("ProtocolError");
                Debug.Log(webRequest.downloadHandler.text);
                onProtocolErrorRequest.Invoke();
                break;
            case UnityWebRequest.Result.DataProcessingError:
                print("DataProcessingError");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
