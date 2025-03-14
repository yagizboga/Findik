using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject table;
    bool isActive;
    public void Interact(){
        if(isActive){
            table.gameObject.SetActive(false);
            isActive = false;
        }
        else if(!isActive){
            table.gameObject.SetActive(true);
            isActive = true;
        }
    }
}
