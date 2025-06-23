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

    public List<string> dialogueKeys = new List<string>(); // private yap, ba�ka scriptlerden start dialogue call ile gerekli arrayi parametre olarak al
    [SerializeField] private int currentIndex = 0;

    private bool isDialogueActive = false;

    // A�IKLAMA: tableName, b�t�n diyaloglar� i�eren tablonun ad� olmal�. tek bir tabloda b�t�n diyaloglar, farkl� key'ler ile tutulacak. hangi diyalog tetiklenecekse, o diyalogun keyleri s�ras� ile bir 
    // listeye eklenip, StartDialogue metoduna parametre olarak g�nderilecek. bu sayede tek bir script ile b�t�n diyaloglar y�netilebilecek.

    // A�IKLAMA2: dialogueKeys, diyalogun hangi sat�rlar�n�n g�sterilece�ini belirten key'leri i�eren bir liste. bu liste, diyalog ba�lat�l�rken parametre olarak al�nacak.

    // ONEMLi: Kodu guncelle, e'ye basinca diyalog acilmadan once, hangi diyalogun acilacagi bilgisi de alinsin. bu bilgi, bir liste olarak tutulacak 



    public GameObject dialogueScreen;
    public InputActionReference dialogueAdvanceAction;

    private bool inDialogue = false;
    private bool hasStartedDialogue = false;

    public Image playerImage;
    public Image npcImage;



    void Start()
    {
        continueButton.onClick.AddListener(NextLine);
        //dialoguePanel.SetActive(false);
        //StartDialogue(dialogueKeys);
        dialogueScreen.SetActive(false);
    }

    void OnEnable()
    {
        dialogueAdvanceAction.action.Enable();
        dialogueAdvanceAction.action.performed += OnDialogueAdvance;
    }

    void OnDisable()
    {
        dialogueAdvanceAction.action.performed -= OnDialogueAdvance;
        dialogueAdvanceAction.action.Disable();
    }

    private void OnDialogueAdvance(InputAction.CallbackContext context)
    {
        if (!hasStartedDialogue)
        {
            hasStartedDialogue = true;
            dialogueScreen.SetActive(true);
            inDialogue = true;

            StartDialogue(dialogueKeys);
        }
        else if (inDialogue)
        {
            NextLine();
        }
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
        UpdateSpeakerHighlight(key);
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
        dialogueScreen.SetActive(false);
        inDialogue = false;
        hasStartedDialogue = false;

        Debug.Log("Dialogue ended");
    }

    private void UpdateSpeakerHighlight(string key)
    {
        if (key.ToLower().Contains("player"))
        {
            SetImageColor(playerImage, Color.white);
            SetImageColor(npcImage, Color.black);
        }
        else if (key.ToLower().Contains("npc"))
        {
            SetImageColor(playerImage, Color.black);
            SetImageColor(npcImage, Color.white);
        }
        else
        {
            SetImageColor(playerImage, Color.white);
            SetImageColor(npcImage, Color.white);
        }
    }

    private void SetImageColor(Image image, Color color)
    {
        if (image != null)
            image.color = color;
    }

}