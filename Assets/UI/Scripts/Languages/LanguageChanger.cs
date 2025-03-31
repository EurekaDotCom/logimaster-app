using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

public class LanguageChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abrevLangText;
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        AddDropDownOptions();
        abrevLangText.text = LanguageManger.Instance.LanguageAbreviature();
    }
    public void AddDropDownOptions()
    {
        dropdown.ClearOptions();
     

        List<OptionData> options = new List<OptionData>();
        for (int i = 0; i < LanguageManger.Instance.languages.Count; i++)
        {
            options.Add(new OptionData()
            {
                text = LanguageManger.Instance.languages[i].LanguageName,
            });
        }
        dropdown.AddOptions(options);
    }


    public void ChangeDropDownLang(int dropDownValue)
    {
        LanguageManger.Instance.ChangeLang(dropDownValue);
        abrevLangText.text = LanguageManger.Instance.LanguageAbreviature();
    }
}
