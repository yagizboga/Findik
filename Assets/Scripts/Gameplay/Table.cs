using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject tableCanvas;
    bool isActive;
    public void Interact(){
        if(isActive){
            tableCanvas.gameObject.SetActive(false);
            isActive = false;
        }
        else if(!isActive){
            tableCanvas.gameObject.SetActive(true);
            isActive = true;
        }
    }
}
