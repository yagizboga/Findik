using UnityEngine;

public class shelf : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject shelfObject;
    [SerializeField] Transform shelfTransform;
    [SerializeField] Transform gameTransform;
    bool isActive = false;

    public void Interact(){
        if(isActive){
            shelfObject.transform.position = shelfTransform.position;
            isActive = false;
        }
        else if(!isActive){
            shelfObject.transform.position = gameTransform.position;
            isActive = true;
        }
    }
}
