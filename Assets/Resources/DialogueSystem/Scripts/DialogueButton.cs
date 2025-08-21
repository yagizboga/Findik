using UnityEngine;

public class DialogueButton : MonoBehaviour
{
    void OnMouseDown()
    {
        GameObject eventManager = GameObject.FindGameObjectWithTag("EventManager");
        int nextDialogueId = eventManager.GetComponent<EventManager>().nextDialogueId;
        eventManager.GetComponent<EventManager>().ShowDialogue(nextDialogueId, "en");
    }
}
