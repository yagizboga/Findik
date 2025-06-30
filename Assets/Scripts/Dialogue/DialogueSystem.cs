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

    // ONEMLi: Kodu guncelle, e'ye basinca diyalog acilmadan once, hangi diyalogun acilacagi bilgisi de alinsin. bu bilgi, bir liste olarak tutulacak 



    public GameObject dialogueScreen;
    public InputActionReference dialogueAdvanceAction;

    private bool inDialogue = false;
    private bool hasStartedDialogue = false;

    public Image playerImage;
    public Image npcImage;

    private Coroutine typeCoroutine;
    private bool isLineFullyRevealed = false;
    [SerializeField] private float typingSlowness = 0.03f; 



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
            if (!isLineFullyRevealed)
            {
                // Hemen tüm satýrý göster
                if (typeCoroutine != null)
                    StopCoroutine(typeCoroutine);

                var key = dialogueKeys[currentIndex];
                StartCoroutine(ShowFullLineImmediately(key));
            }
            else
            {
                NextLine();
            }
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
                string fullText = entry.GetLocalizedString();

                if (typeCoroutine != null)
                    StopCoroutine(typeCoroutine);

                typeCoroutine = StartCoroutine(TypeLine(fullText));
            }
            else
            {
                dialogueText.text = "[Key Not Found: " + key + "]";
                isLineFullyRevealed = true;
            }
        }
        else
        {
            dialogueText.text = "[Table Not Loaded]";
            isLineFullyRevealed = true;
        }
    }

    IEnumerator TypeLine(string fullLine)
    {
        isLineFullyRevealed = false;
        dialogueText.text = "";

        foreach (char letter in fullLine)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSlowness);
        }

        isLineFullyRevealed = true;
    }

    IEnumerator ShowFullLineImmediately(string key)
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

        isLineFullyRevealed = true;
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