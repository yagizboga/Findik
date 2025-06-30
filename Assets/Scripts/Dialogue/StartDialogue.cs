using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class StartDialogue : MonoBehaviour
{
    public DialogueSystem dialogue;
    public Button dialogueButton;
    public List<string> dialogueKeys = new List<string>();

    void Start()
    {
        dialogueButton.onClick.AddListener(CallDialogue);
    }

    private void CallDialogue()
    {
        dialogue.StartDialogue(dialogueKeys);
    }

}
