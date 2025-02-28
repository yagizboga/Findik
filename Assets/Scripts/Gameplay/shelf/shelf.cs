using UnityEngine;

public class shelf : MonoBehaviour, IInteractable
{
    [SerializeField] Canvas shelfUI;
    bool isActive = false;

    public void Interact(){
        if(isActive){
            shelfUI.gameObject.SetActive(false);
            isActive = false;
        }
        else if(!isActive){
            shelfUI.gameObject.SetActive(true);
            isActive = true;
        }
    }
}
