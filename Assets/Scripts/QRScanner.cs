using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using TMPro;

public class QRScanner : MonoBehaviour
{
    WebCamTexture webcamTexture;
    string QrCode = string.Empty;
    [SerializeField]
    private QRScreenWindow _qrScreenWindow;

    private Texture2D snap;

    void OnEnable()
    {
        if (_qrScreenWindow == null)
            _qrScreenWindow = FindAnyObjectByType<QRScreenWindow>();
        StartNewScan();
    }
    //TODO: add translation to messagess
    IEnumerator GetQRCode()
    {
        _qrScreenWindow.ScanerStatusText.text = "QR Code scanned: Waiting for QR code";
        IBarcodeReader barCodeReader = new BarcodeReader();
        webcamTexture.Play();

        yield return new WaitForSeconds(1);

        snap = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);

        while (string.IsNullOrEmpty(QrCode))
        {
            try
            {
                snap.SetPixels32(webcamTexture.GetPixels32());

#if UNITY_IOS
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                    FlipTextureHorizontally(snap);
#endif
                var Result = barCodeReader.Decode(snap.GetRawTextureData(), webcamTexture.width, webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);

                //_qrScreenWindow.ScanerStatusText.text = Result.ToString();

                if (Result != null)
                {
                    Debug.Log("Result is NOT null!");
                    QrCode = Result.Text;
                    if (!string.IsNullOrEmpty(QrCode))
                    {
                        _qrScreenWindow.ScanerStatusText.text = "QR Code scanned: OK";
                        _qrScreenWindow.GetDataFromQRScan(QrCode);

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
                _qrScreenWindow.ScanerStatusText.text = "QR Code scanned: Invalid QR code";
                _qrScreenWindow.ErrorWindow.SetActive(true);
            }
            yield return null;
        }
        webcamTexture.Stop();
    }

    public void StartNewScan()
    {
        var renderer = GetComponent<RawImage>();
        webcamTexture = new WebCamTexture(360, 360);

        renderer.texture = webcamTexture;
        QrCode = string.Empty;
        renderer.material.mainTexture = webcamTexture;
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            renderer.gameObject.transform.localScale = new Vector3 (1f, -1f, 1f);
#endif
        StartCoroutine(GetQRCode());
    }

    public static void FlipTextureHorizontally(Texture2D original)
    {
        var originalPixels = original.GetPixels32();

        var newPixels = new Color32[originalPixels.Length];

        var width = original.width;
        var rows = original.height;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < rows; y++)
            {
                newPixels[x + y * width] = originalPixels[(width - x - 1) + y * width];
            }
        }

        original.SetPixels32(newPixels);
        original.Apply();
    }

    private void OnDisable()
    {
        webcamTexture.Stop();
        StopCoroutine(GetQRCode());
    }
}
