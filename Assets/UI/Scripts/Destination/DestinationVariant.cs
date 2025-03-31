using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum DestinationColor
{
    Warehouse,
    Company
}
public class DestinationVariant : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _destinationNameText;
    [SerializeField]
    private Color _warehouseColor;
    [SerializeField]
    private Color _compatyColor;

    private QRScreenWindow _qRScreenWindow;
    private PrintQR _printQR;
    private bool _isTransfer;
    private int? _id = null;

    public void SetID(int id)
    {
        _id = id;
    }

    public void SetNameText(string destinationName)
    {
        _destinationNameText.text = destinationName;
    }

    public void SetTextColor(DestinationColor color)
    {
        switch (color)
        {
            case DestinationColor.Warehouse:
                _destinationNameText.color = _warehouseColor;
                _isTransfer = false;
                break;
            case DestinationColor.Company:
                _destinationNameText.color = _compatyColor;
                _isTransfer = true;
                break;
            default:
                _destinationNameText.color = _warehouseColor;
                _isTransfer = false;
                break;
        }
    }

    public void SetDestination()
    {
        if (_qRScreenWindow != null)
            _qRScreenWindow.SetDestination(_destinationNameText.text, _destinationNameText.color, _isTransfer);
        else if (_printQR != null)
            _printQR.SetDestination(_destinationNameText.text, _destinationNameText.color, _id);

    }

    public void SetQRScreenWindow(QRScreenWindow qRScreen)
    {
        _qRScreenWindow = qRScreen;
    }

    public void SetPrintQR(PrintQR printQR)
    {
        _printQR = printQR;
    }
}
