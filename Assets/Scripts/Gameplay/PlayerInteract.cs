using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] GameObject SeedBasketUI;
    GameObject collided;
    Vector2 playerToFarmGrid;
    bool isPlanting = false;
    bool isWatering = false;
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

    public void LClick_Input(InputAction.CallbackContext ctx){
         if(ctx.performed){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos,new Vector3(0,0,1),100f);
            if(isPlanting){
            if(hit.collider != null){
                Debug.Log(hit.collider.gameObject);
                if(hit.collider.CompareTag("farmGridCell") && IsFarmable[(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).x,(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).y]){
                    Debug.Log(farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos));
                    farm.GetComponent<farm>().isPlanted[(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).x,(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).y] = true;
                    farm.GetComponent<farm>().GetFarmSquares((int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).x,(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).y).GetComponent<SpriteRenderer>().color = new Color32(0,150,90,255);
                    if(farm.GetComponent<farm>().isWatered[(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).x,(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).y]){
                        farm.GetComponent<farm>().StartCoroutine(farm.GetComponent<farm>().GrowPlant((int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).x,(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).y,5));
                    }
                }

            }
            }
            if(isWatering){
            if(hit.collider != null){
                Debug.Log(hit.collider.gameObject);
                if(hit.collider.CompareTag("farmGridCell") && IsFarmable[(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).x,(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).y]){
                    Debug.Log(farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos));
                    farm.GetComponent<farm>().isWatered[(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).x,(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).y] = true;
                    farm.GetComponent<farm>().GetFarmSquares((int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).x,(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).y).GetComponent<SpriteRenderer>().color = new Color32(0,150,150,255);
                    if(farm.GetComponent<farm>().isPlanted[(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).x,(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).y]){
                        farm.GetComponent<farm>().StartCoroutine(farm.GetComponent<farm>().GrowPlant((int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).x,(int)farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos).y,5));
                    }
                }

            }
         }
    }
    }

    void Update(){
        if(isPlanting || isWatering){
            playerToFarmGrid = new Vector2(farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(transform.position).x,
                                            farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(transform.position).y);

            for(int x = 0;x<farm.GetComponent<farm>().GetFarmGrid().GetSize().x;x++){
                for(int y = 0;y<farm.GetComponent<farm>().GetFarmGrid().GetSize().y;y++){
                    //Debug.Log(playerToFarmGrid.x  + " " + playerToFarmGrid.y);
                    if(playerToFarmGrid.x <= x +1 && playerToFarmGrid.x >= x-1 && playerToFarmGrid.y <= y+1 && playerToFarmGrid.y>=y-1){
                            IsFarmable[x,y] = true;
                    }
                    else{IsFarmable[x,y] =false;
                }
            }


        }
        }
    }

    public void SetIsPlanting(bool b){
        if(isWatering){
            SetIsWatering(false);
        }
        isPlanting = b;
        if(isPlanting){
            interactText.text = "planting";
            SeedBasketUI.SetActive(true);
        }
        else if(isPlanting == false){
            interactText.text = "interact";
            SeedBasketUI.SetActive(false);
        }
    }

    public void SetIsWatering(bool b){
        if(isPlanting){
            SetIsPlanting(false);
        }
        isWatering = b;
        if(isWatering){
            interactText.text = "watering";
        }
        else if(isWatering == false){
            interactText.text = "interact";
        }
    }

    public bool GetIsPlanting(){

        return isPlanting;
    }

    public bool GetIsWatering(){
        return isWatering;
    }

}
