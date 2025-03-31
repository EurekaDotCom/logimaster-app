using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class LanguageListener : MonoBehaviour
{
    [SerializeField] private int id; // id or index from list with strings what have translate
    public int Id => id;
    [Header("Value what need to be the same in all language")]
    public string ISstring = ""; // IS - Internatiol System
    public string ISstring_Before = ""; // IS - Internatiol System
    [SerializeField] private TextMeshProUGUI textToChange;


    private void Start()
    {
        SubscribeToListener();
        ChangeText(LanguageManger.Instance.CurrentTextLang(id));
    }

    private void OnDestroy()
    {
        UnsubscribeToListener();
    }
    private void SubscribeToListener()
    {
        LanguageManger.Instance.AddListener(this);

    }

    private void UnsubscribeToListener() 
    {
        LanguageManger.Instance.RemoveListener(this);

    }
    public void ChangeText(string info)
    {
        if (textToChange == null)
        {
            textToChange = gameObject.GetComponent<TextMeshProUGUI>();
        }
        textToChange.text = ISstring_Before + info + ISstring;             
    }

    public void ChangeID(int value)
    {
        id = value;
    }
}
