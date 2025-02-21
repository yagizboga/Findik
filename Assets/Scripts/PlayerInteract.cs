using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    bool isEPressing = false;
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
            collided.GetComponent<IInteractable>().InteractEnded();
            collided = null;
        }
    }

    public void E_Input(InputAction.CallbackContext ctx){
        if(ctx.started){
            isEPressing = true;
            if(collided.GetComponent<IInteractable>().isActive){
                collided.GetComponent<IInteractable>().InteractEnded();
            }
        }
        if(ctx.performed){
            isEPressing = true;
        }
        if(ctx.canceled){
            isEPressing = false;
        }
        if(isEPressing && collided != null && !collided.GetComponent<IInteractable>().isActive){
                collided.GetComponent<IInteractable>().InteractStarted();
        }
    }
}
