using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LanguageManger : Singleton<LanguageManger>
{
 
    public List<LanguageContainer> languages = new List<LanguageContainer>();// all languages what we have in project, loading scriptableObject from Resources folder
    private List<LanguageListener> allListeners = new List<LanguageListener>();// all texts what have translate
    private LanguageContainer currentLangContainer;
    private int currentLangueID = 0;
    public int CurrentLangueID {  get { return currentLangueID; } }
  
    private new void Awake()
    {
        base.Awake();
        languages = LoadLanguagesFiles();

        if (languages == null)
        {
            languages = Resources.LoadAll<LanguageContainer>(GameKeys.LanguagesLocation).ToList();
        }

        languages =  languages.OrderBy(e => e.LanguageId).ToList();
        ChangeLang(currentLangueID);
    }

    public void ChangeLang(int id)
    {
        currentLangueID = id;
        currentLangContainer = languages.Find(e => e.LanguageId == id);
        for (int i = 0; i < allListeners.Count; i++) 
        {
            allListeners[i].ChangeText(currentLangContainer.Strings[allListeners[i].Id]);
        }
    }

    public void AddListener(LanguageListener listener)
    {
        allListeners.Add(listener);
    }

    public void RemoveListener(LanguageListener listener)
    {
        allListeners.Remove(listener);
    }

    public string LanguageAbreviature()
    {
        return currentLangContainer.LanguageAbreviature;
    }

    public string CurrentTextLang( int textId)
    {
        return currentLangContainer.Strings[textId];
    }

    [ContextMenu(nameof(tempSave))]
    public void tempSave()
    {
        string languageJSON = "";
        List<Language> serializeLanguages = new List<Language>(); 

        //Test local save
        Debug.Log($"{Application.streamingAssetsPath}/LanguagesJSON");
        if (!Directory.Exists($"{Application.streamingAssetsPath}/LanguagesJSON"))
        {
            Directory.CreateDirectory($"{Application.streamingAssetsPath}/LanguagesJSON");
        }

        foreach (LanguageContainer language in languages)
        {
            Language serializelanguage = new Language();
            serializelanguage.LanguageId = language.LanguageId;
            serializelanguage.LanguageName = language.LanguageName;
            serializelanguage.LanguageAbreviature = language.LanguageAbreviature;
            serializelanguage.Strings = language.Strings;
            serializeLanguages.Add(serializelanguage);
        }

        foreach (Language language in serializeLanguages)
        {
            languageJSON = JsonConvert.SerializeObject(language);
            File.WriteAllText($"{Application.streamingAssetsPath}/LanguagesJSON/{language.LanguageName}.txt", languageJSON);
            Debug.Log(languageJSON);
        }
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
            Debug.Log(file.Name);
            if (File.Exists($"{Application.streamingAssetsPath}/LanguagesJSON/{file.Name}"))
            {
                string loadString = File.ReadAllText($"{Application.streamingAssetsPath}/LanguagesJSON/{file.Name}");
                Debug.Log(loadString);

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

[System.Serializable]
public class Language
{
    public int LanguageId;
    public string LanguageName;
    public string LanguageAbreviature;
    public List<string> Strings;
}