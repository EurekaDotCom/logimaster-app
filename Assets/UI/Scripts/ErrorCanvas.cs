
using UnityEngine;

public delegate void TryAgainRequest();

public class ErrorCanvas : Singleton<ErrorCanvas>
{
    [SerializeField]
    private GameObject _errorGameObject;

    private TryAgainRequest request;

    private void Start()
    {
        _errorGameObject.SetActive(false);
    }
    public void ShowErrorPanel(TryAgainRequest lastRequest)
    {
        
        request = null;
        request = lastRequest;
        _errorGameObject.SetActive(true);
    }
    public void TryAgainaBtn()
    {
        //request.Invoke();
        _errorGameObject.SetActive(false);

    }
}
