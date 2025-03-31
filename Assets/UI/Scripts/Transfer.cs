using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Transfer : UIWindow
{
    [SerializeField]
    private TextMeshProUGUI _quantityToTransferText;
    [SerializeField]
    private Button _printButton;
    [SerializeField]
    private Button _cancelButton;

    private void Start()
    {
        _cancelButton.onClick.AddListener(_backButton.onClick.Invoke);
        _cancelButton.onClick.AddListener(CancelButtonClick);
    }

    private void CancelButtonClick()
    {
    }

    public void SetQuantityToTransfer(TextMeshProUGUI quantity)
    {
        _quantityToTransferText.text = quantity.text;
    }
}
