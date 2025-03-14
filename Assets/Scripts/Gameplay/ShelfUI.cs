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
    Vector2 [,] oldPositions;


    void Awake(){
        shelfGrid = new Grid(16,9,1,gridStartPos.position,textParentCanvas);
        lastCells = new List<Vector2>();
        
        

    }

    void Start(){
        NewItem(ItemScriptableObject.shelfItemType.key);
        NewItem(ItemScriptableObject.shelfItemType.book);

        currentItem = NewItem(ItemScriptableObject.shelfItemType.key);
    }
    void Update(){
        if(isDragging && currentItem != null){
            currentItem.GetComponent<ShelfItem>().SetTransformToMouse();
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    } 

    public void DragHandler(InputAction.CallbackContext ctx ){
        if(ctx.performed && shelf.GetComponent<shelf>().isActive){
            oldPositions = new Vector2 [(int)currentItem.GetComponent<ShelfItem>().GetItemGrid().GetSize().x,(int)currentItem.GetComponent<ShelfItem>().GetItemGrid().GetSize().y];
            isDragging = true;
            for(int x = 0;x<currentItem.GetComponent<ShelfItem>().GetItemGrid().GetSize().x;x++){
                for(int y = 0;y<currentItem.GetComponent<ShelfItem>().GetItemGrid().GetSize().y;y++){
                    oldPositions[x,y] = currentItem.GetComponent<ShelfItem>().GetItemGrid().GetGridPosToGrid(shelfGrid,x,y);
                    Debug.Log(oldPositions[x,y]);
                }
            }
        }
        else if(ctx.canceled && shelf.GetComponent<shelf>().isActive){
            isDragging = false;
            if(currentItem != null && currentItem.GetComponent<ShelfItem>().GetItemGrid().isOnGrid(shelfGrid)){
                currentItem.GetComponent<ShelfItem>().GetItemGrid().SetGridPosToGrid(shelfGrid);
                
            }
            else if(currentItem != null && !currentItem.GetComponent<ShelfItem>().GetItemGrid().isOnGrid(shelfGrid)){
                for(int x = 0;x<oldPositions.GetLength(0);x++){
                    for(int y=0;y<oldPositions.GetLength(1);y++){
                        Debug.Log(oldPositions[x,y]);
                        currentItem.GetComponent<ShelfItem>().GetItemGrid().SetCellPosition(x,y,(int)oldPositions[x,y].x,(int)oldPositions[x,y].y);
                    }
                }
                //Debug.Log(oldPositions[0,0]);
                oldPositions = null;
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
