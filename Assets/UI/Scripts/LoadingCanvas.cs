using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingCanvas : Singleton<LoadingCanvas>
{
    //[SerializeField] private TextMeshProUGUI loadingInfoText;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject _loadingIcon;

    private void Start()
    {
        _loadingPanel.SetActive(false);
    }
    public void Show(string loadInfo = "")
    {
        _loadingPanel.SetActive(true);
    }
    public void Hide()
    {
        _loadingPanel.SetActive(false);
    }
}
