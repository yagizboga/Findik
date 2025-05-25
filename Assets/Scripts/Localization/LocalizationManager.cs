using UnityEditor.Localization.Editor;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField] private Button turkishButton;
    [SerializeField] private Button englishButton;
    [SerializeField] private Button spanishButton;
    [SerializeField] private Button russianButton;
    [SerializeField] private Button chineseButton;
    [SerializeField] private Button germanButton;

    private void Awake()
    {
        /*if (LocalizationSettings.SelectedLocale == null)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }*/
        turkishButton.onClick.AddListener(() => SetLanguage(LanguageType.Turkish));
        englishButton.onClick.AddListener(() => SetLanguage(LanguageType.English));
        spanishButton.onClick.AddListener(() => SetLanguage(LanguageType.Spanish));
        russianButton.onClick.AddListener(() => SetLanguage(LanguageType.Russian));
        chineseButton.onClick.AddListener(() => SetLanguage(LanguageType.Chinese));
        germanButton.onClick.AddListener(() => SetLanguage(LanguageType.German));
    }

    public void SetLanguage(LanguageType languageType)
    {
        foreach(var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            //Debug.Log(locale.ToString());
            if (locale.LocaleName.Contains(languageType.ToString()))
            {
                LocalizationSettings.SelectedLocale = locale;
                Debug.Log($"Language set to: {locale.LocaleName}");
                return;
            }
        }

        Debug.LogWarning($"Language {languageType} not found in available locales.");
    }
}
 