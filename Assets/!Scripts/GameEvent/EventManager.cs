using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField] EventScriptableObject currentEvent;
    Sprite npcSprite;
     [SerializeField] SpriteRenderer botPlate;
     [SerializeField] SpriteRenderer topPlate;
     [SerializeField] TextMeshPro DialogueText;
    [SerializeField] GameObject npcPrefab;
    [SerializeField] Sprite mouthSprite;
    [SerializeField] Sprite eyesSprite;
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform dialoguePosition;
    [SerializeField] Transform npcSpawnPosition;
    DialogueLoader dialogueloader;
    int currentDialogueId;
    public int nextDialogueId;

    void Start()
    {
        dialogueloader = new DialogueLoader();
        LoadEvent(currentEvent);
    }


    public void LoadEvent(EventScriptableObject newEvent)
    {
        currentEvent = newEvent;

        npcSprite = newEvent.npcSprite;
        botPlate.sprite = newEvent.botPlateSprite;
        topPlate.sprite = newEvent.topPlateSPrite;

        dialogueloader.LoadDialogue(newEvent.dialogueXml);
        currentDialogueId = 1;
        nextDialogueId = 0;
        GameObject newnpc = GameObject.Instantiate(npcPrefab, npcSpawnPosition.position, Quaternion.identity);
        newnpc.GetComponent<eventNpcMono>().Init(newEvent.topDownAnimation, npcSprite, mouthSprite, eyesSprite, targetPosition.position, dialoguePosition);
        ShowDialogue(1, "en");
    }
    public void ShowDialogue(int currentDialogueId, string language)
    {
        DialogueText.text = dialogueloader.GetDialogue(language, currentDialogueId, out nextDialogueId);
    }
}