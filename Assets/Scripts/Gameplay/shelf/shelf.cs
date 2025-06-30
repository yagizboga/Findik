using UnityEngine;

public class shelf : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject shelfObject;
    public bool isActive = false;

    public void Interact(GameObject player){
        if(isActive){
            shelfObject.SetActive(false);
            isActive = false;
        }
        else if(!isActive){
            shelfObject.SetActive(true);
            isActive = true;
        }
    }
}
