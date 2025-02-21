using UnityEngine;

public class shelf : MonoBehaviour, IInteractable
{
    [SerializeField] Canvas shelfUI;
    public bool isActive{get; set;} = false;
    public void InteractStarted(){
        shelfUI.gameObject.SetActive(true);
        isActive = true;
    }
    public void InteractEnded(){
        shelfUI.gameObject.SetActive(false);
        isActive = false;
    }
}
