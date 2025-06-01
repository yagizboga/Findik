using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class DialogueSystem : MonoBehaviour
{
    //public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    public string tableName = "DialogueTable";

    public List<string> dialogueKeys = new List<string>(); // private yap, baþka scriptlerden start dialogue call ile gerekli arrayi parametre olarak al
    [SerializeField] private int currentIndex = 0;

    private bool isDialogueActive = false;

    // AÇIKLAMA: tableName, bütün diyaloglarý içeren tablonun adý olmalý. tek bir tabloda bütün diyaloglar, farklý key'ler ile tutulacak. hangi diyalog tetiklenecekse, o diyalogun keyleri sýrasý ile bir 
    // listeye eklenip, StartDialogue metoduna parametre olarak gönderilecek. bu sayede tek bir script ile bütün diyaloglar yönetilebilecek.

    // AÇIKLAMA2: dialogueKeys, diyalogun hangi satýrlarýnýn gösterileceðini belirten key'leri içeren bir liste. bu liste, diyalog baþlatýlýrken parametre olarak alýnacak.


    void Start()
    {
        continueButton.onClick.AddListener(NextLine);
        //dialoguePanel.SetActive(false);
        StartDialogue(dialogueKeys);
    }

    public void StartDialogue(List<string> keys)
    {
        dialogueKeys = keys;
        currentIndex = 0;
        isDialogueActive = true;
        //dialoguePanel.SetActive(true);
        ShowLine();
    }

    public void ShowLine()
    {
        string key = dialogueKeys[currentIndex];
        StartCoroutine(LoadLocalizedLine(key));
    }


    IEnumerator LoadLocalizedLine(string key)
    {
        var tableLoading = LocalizationSettings.StringDatabase.GetTableAsync(tableName);
        yield return tableLoading;

        StringTable table = tableLoading.Result;

        if (table != null)
        {
            StringTableEntry entry = table.GetEntry(key);

            if (entry != null)
            {
                dialogueText.text = entry.GetLocalizedString();
            }
            else
            {
                dialogueText.text = "[Key Not Found: " + key + "]";
            }
        }
        else
        {
            dialogueText.text = "[Table Not Loaded]";
        }
    }


    void NextLine()
    {
        if (currentIndex < dialogueKeys.Count - 1)
        {
            currentIndex++;
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        //dialoguePanel.SetActive(false);
        isDialogueActive = false;
        Debug.Log("dialogue ended");
    }

}