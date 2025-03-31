using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ZXing;

public class MakePicture : UIWindow
{
    WebCamTexture webcamTexture;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private RawImage _rawImage;
    [SerializeField]
    private Button _makeButton, _restart;


    byte[] Result;

    string jsonDataToSend;
    bool _isWebcamOn = true;

    void OnEnable()
    {
        Restart();
    }
    public void StartNewScan()
    {
        var renderer = _rawImage;
        webcamTexture = new WebCamTexture(360, 190);
        renderer.texture = webcamTexture;
        renderer.material.mainTexture = webcamTexture;
        StartCoroutine(RestartCorutine());
    }

    IEnumerator RestartCorutine()
    {
        _makeButton.gameObject.SetActive(true);
        _restart.gameObject.SetActive(false);
        webcamTexture.Play();
        yield return new WaitForSeconds(.5f);
        _image.gameObject.SetActive(false);
        var snap = new Texture2D(webcamTexture.height, webcamTexture.width, TextureFormat.ARGB32, false);

        while (_isWebcamOn)
        {
            snap.SetPixels32(webcamTexture.GetPixels32());
            Result = snap.GetRawTextureData();
            yield return null;
        }
        webcamTexture.Stop();
    }

    public void CreateTexture()
    {
        _makeButton.gameObject.SetActive(false);
        _restart.gameObject.SetActive(true);
        TakeAndEncodePhoto();

        DecodePhoto();
        _isWebcamOn = false;
        webcamTexture.Stop();
        StopCoroutine(RestartCorutine());
    }

    void TakeAndEncodePhoto()
    {
        var encodedString = Convert.ToBase64String(Result);
        TestingClassForJSON forJSON = new TestingClassForJSON();
        forJSON.ImageByts = encodedString;

        jsonDataToSend = JsonUtility.ToJson(forJSON);
    }

    void DecodePhoto()
    {
        int width = webcamTexture.width;
        int height = webcamTexture.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);

        TestingClassForJSON fromJsonData = JsonUtility.FromJson<TestingClassForJSON>(jsonDataToSend);
        var decodedByts = Convert.FromBase64String(fromJsonData.ImageByts);
        Result = decodedByts;

        tex.LoadRawTextureData(Result);
        tex.Apply();

        Rect rec = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rec, new Vector2(0, 0), 1);

        _image.gameObject.SetActive(true);
        _image.sprite = sprite;
        SetImage(sprite);
    }

    void SetImage(Sprite sprite)
    {
        if(PreviousWindow as PrintQR)
        {
            (PreviousWindow as PrintQR).SetImage(sprite);
            _backButton.onClick.Invoke();
        }
    }

    public void Restart()
    {
        _isWebcamOn = true;
        StartNewScan();
    }

    private void OnDisable()
    {
        webcamTexture.Stop();
        StopCoroutine(RestartCorutine());
    }
}

[Serializable]
public class TestingClassForJSON
{
    public string ImageByts;
}