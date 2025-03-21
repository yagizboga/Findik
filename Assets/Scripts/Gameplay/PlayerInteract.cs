using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    GameObject collided;
    Vector2 playerToFarmGrid;
    bool isPlanting = false;
    GameObject farm;
    bool [,]IsFarmable;

    void Awake(){
        farm = GameObject.FindGameObjectWithTag("farm");
        IsFarmable = new bool[(int)farm.GetComponent<farm>().GetFarmGrid().GetSize().x,(int)farm.GetComponent<farm>().GetFarmGrid().GetSize().y];
    }
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
                collided.gameObject.GetComponent<IInteractable>().Interact(gameObject);
           }
        }
        if(ctx.performed){
            
        }
        if(ctx.canceled){
           
        }
    }

    void Update(){
        Debug.Log(isPlanting);
        if(isPlanting){
            playerToFarmGrid = new Vector2(farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(transform.position).x,
                                            farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(transform.position).y);

            for(int x = 0;x<farm.GetComponent<farm>().GetFarmGrid().GetSize().x;x++){
                for(int y = 0;x<farm.GetComponent<farm>().GetFarmGrid().GetSize().y;y++){
                    if(playerToFarmGrid.x + 1 <= x && playerToFarmGrid.x -1>=x){
                        if(playerToFarmGrid.y + 1 <= y && playerToFarmGrid.y -1>=y){
                            IsFarmable[x,y] = true;
                            farm.GetComponent<farm>().GetFarmGrid().GetValueText(x,y).gameObject.GetComponent<TextMeshPro>().color = new Color32(255,170,170,255);
                        }
                    }
                    else{IsFarmable[x,y] =false;
                        farm.GetComponent<farm>().GetFarmGrid().GetValueText(x,y).gameObject.GetComponent<TextMeshPro>().color = new Color32(255,255,255,255);}
                }
            }
        }
    }

    public void SetIsPlanting(bool b){
        isPlanting = b;
        if(isPlanting){
            interactText.text = "planting";
        }
        else if(isPlanting == false){
            interactText.text = "interact";
        }
    }

    public bool GetIsPlanting(){

        return isPlanting;
    }
}
