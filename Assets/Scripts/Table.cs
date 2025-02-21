using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{
    [SerializeField] Canvas tableCanvas;
    public bool isActive{get; set;} = false;
    public void InteractStarted(){
        tableCanvas.gameObject.SetActive(true);
        isActive = true;
    }
    public void InteractEnded(){
        tableCanvas.gameObject.SetActive(false);
        isActive = false;
    }
}
