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
            isDragging = true;
        }
        else if(ctx.canceled && shelf.GetComponent<shelf>().isActive){
            isDragging = false;
            if(currentItem != null){
                currentItem.GetComponent<ShelfItem>().GetItemGrid().GetGridPosToGrid(shelfGrid);
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
