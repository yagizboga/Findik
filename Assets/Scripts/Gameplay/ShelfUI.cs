using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class ShelfUI : MonoBehaviour
{
    [SerializeField] Transform gridStartPos;
    [SerializeField] GameObject textParentCanvas;
    [SerializeField] GameObject shelf;
    Grid shelfGrid;
    Vector3 mousePos;
    List<Vector2> lastCells;
    GameObject currentItem;
    bool isDragging = false;
    Vector2 oldPosition;


    void Awake(){
        shelfGrid = new Grid(16,9,1,gridStartPos.position,textParentCanvas);
        lastCells = new List<Vector2>();
        oldPosition = new Vector2();
        
        

    }

    void Start(){
        NewItem(ItemScriptableObject.shelfItemType.GOLDANDCOPPERSWORD);
        NewItem(ItemScriptableObject.shelfItemType.UGLYRIPPEDOFFTOYHEAD);
        //NewItem(ItemScriptableObject.shelfItemType.book);
    }
    void Update(){
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos,new Vector3(0,0,1),100f);
        if(hit.collider != null){
            Debug.Log(hit.collider.gameObject);
            if(hit.collider.CompareTag("itemCell")){
                currentItem = hit.collider.gameObject.transform.parent.transform.parent.gameObject;
            }
            else{
                currentItem = null;
            }
        }
        if(isDragging && currentItem != null){
            currentItem.GetComponent<ShelfItem>().SetTransformToMouse();
        }
    } 

    public void DragHandler(InputAction.CallbackContext ctx ){
        if(ctx.performed && shelf.GetComponent<shelf>().isActive && currentItem != null){
            //currentItem.GetComponent<ShelfItem>().GetItemGrid().AlignToChild();
            //currentItem.GetComponent<ShelfItem>().GetItemGrid().AlignToCenter(mousePos);
            oldPosition = new Vector2(currentItem.gameObject.transform.position.x,currentItem.gameObject.transform.position.y);
            isDragging = true;
        }
        else if(ctx.canceled && shelf.GetComponent<shelf>().isActive){
            isDragging = false;
            if(currentItem != null && currentItem.GetComponent<ShelfItem>().GetItemGrid().isOnGrid(shelfGrid)){
                currentItem.GetComponent<ShelfItem>().GetItemGrid().SetGridPosToGrid(shelfGrid);
                
            }
            else if(currentItem != null && !currentItem.GetComponent<ShelfItem>().GetItemGrid().isOnGrid(shelfGrid)){
                
                currentItem.transform.position = new Vector3(oldPosition.x,oldPosition.y,0);
                oldPosition = new Vector2();
            }
        }
    }  
    public Grid GetShelfGrid(){
            return shelfGrid;
    }

    public GameObject NewItem(ItemScriptableObject.shelfItemType type){
        GameObject newItem = new GameObject();
        newItem.AddComponent<ShelfItem>();
        newItem.GetComponent<ShelfItem>().Initialize(type);
        newItem.name = type.ToString();
        newItem.transform.parent = gameObject.transform;
        return newItem;
    }
}
