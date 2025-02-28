using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    GameObject collided;
    void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<IInteractable>() != null ){
            interactText.gameObject.SetActive(true);
            collided = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.GetComponent<IInteractable>() != null ){
            interactText.gameObject.SetActive(false);
            collided = null;
        }
    }

    public void E_Input(InputAction.CallbackContext ctx){
        if(ctx.started){
           if(collided != null){
                collided.gameObject.GetComponent<IInteractable>().Interact();
           }
        }
        if(ctx.performed){
            
        }
        if(ctx.canceled){
           
        }
    }
}
