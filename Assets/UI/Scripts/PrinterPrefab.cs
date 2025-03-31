using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrinterPrefab : MonoBehaviour
{
    [SerializeField]
    private Sprite _selectedSprite;
    [SerializeField]
    private Sprite _unSelectedSprite;
    [SerializeField]
    private Image _printerBackground;
    [SerializeField]
    private TextMeshProUGUI _printerNameText;
    public TextMeshProUGUI PrinterNameText { get { return _printerNameText; } }

    [SerializeField]
    private PrintWindow _printWindow;
    public PrintWindow PrintWindow { get { return _printWindow; } set { _printWindow = value; } }

    
    private PrinterInfo _printerInfo;
    public PrinterInfo PrinterInfo { get { return _printerInfo; } }

    //call from printer button
    public void SelectPrinter()
    {
        if(_printWindow == null)
            return;
        _printWindow.SelectPrinter(this);
        _printerBackground.sprite = _selectedSprite;
    }

    public void DeselectPrinter()
    {
        _printerBackground.sprite = _unSelectedSprite;
    }

    public void SetPrinterInfo(PrinterInfo printerInfo)
    {
        _printerInfo = printerInfo;
        _printerNameText.text = printerInfo.name;
    }
}
