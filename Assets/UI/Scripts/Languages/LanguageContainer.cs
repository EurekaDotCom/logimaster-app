using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Multiform/LanguageContainer",fileName = "LanguageContainer")]
public class LanguageContainer : ScriptableObject
{
    [SerializeField] private int languageID;
    public int LanguageId => languageID;

    [SerializeField] private string languageName;
    public string LanguageName => languageName;
    [SerializeField] private string languageAbreviature;
    public string LanguageAbreviature => languageAbreviature;

    [SerializeField] private List<string> strings;
    public List<string> Strings => strings;


    public void SetLanguageID(int id)
    {
        languageID = id;
    }

    public void SetLanguageName(string name)
    {
        languageName = name;
    }

    public void SetLanguageAbreviature(string abreviature)
    {
        languageAbreviature = abreviature;
    }

    public void SetLanguageStrings(List<string> strings)
    {
        this.strings = strings;
    }

    [ContextMenu(nameof(WriteInfo))]
    public void WriteInfo()
    {
        var languages = LoadLanguagesFiles();

        var language = languages.Where(f  => f.LanguageId == this.languageID).First();

        if (language != null)
        {
            this.strings = language.Strings;
            this.languageName = language.LanguageName;
            this.languageAbreviature = language.LanguageAbreviature;
        }
        else Debug.Log("Fail");
    }

    private List<LanguageContainer> LoadLanguagesFiles()
    {
        if (!Directory.Exists($"{Application.streamingAssetsPath}/LanguagesJSON"))
            return null;

        List<LanguageContainer> languageContainers = new List<LanguageContainer>();

        var info = new DirectoryInfo($"{Application.streamingAssetsPath}/LanguagesJSON");
        var fileInfo = info.GetFiles().Where(f => f.Extension == ".txt").ToArray();

        if (fileInfo == null)
            return null;

        foreach (var file in fileInfo)
        {
            if (File.Exists($"{Application.streamingAssetsPath}/LanguagesJSON/{file.Name}"))
            {
                string loadString = File.ReadAllText($"{Application.streamingAssetsPath}/LanguagesJSON/{file.Name}");

                Language language = JsonUtility.FromJson<Language>(loadString);

                LanguageContainer languageContainer = new LanguageContainer();
                languageContainer.SetLanguageName(language.LanguageName);
                languageContainer.SetLanguageID(language.LanguageId);
                languageContainer.SetLanguageAbreviature(language.LanguageAbreviature);
                languageContainer.SetLanguageStrings(language.Strings);
                languageContainers.Add(languageContainer);
            }
        }
        return languageContainers;
    }
}
