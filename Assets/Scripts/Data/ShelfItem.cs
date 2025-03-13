using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ShelfItem : MonoBehaviour
{
    ItemScriptableObject item;
    Grid itemGrid;
    Grid shelfGrid;
    List<Vector2> lastCells;
    Vector2 lastCell;

    void Awake(){
        lastCells = new List<Vector2>();
        //gridParent.GetComponent<SpriteRenderer>().sortingOrder = 8;
        //gridParent.GetComponent<SpriteRenderer>().sprite = item.GetSprite();

    }

    public void Initialize(ItemScriptableObject.shelfItemType type){
        GameObject gridParent = new GameObject();
        item = new ItemScriptableObject();
        item.Initialize(type);
        if(GetComponent<SpriteRenderer>() == null){
            gridParent.AddComponent<SpriteRenderer>();
        }
        gridParent.transform.SetParent(transform);
        gridParent.transform.localPosition = new Vector3(0,0,0);
        itemGrid = new Grid(item.size.x,item.size.y,1,gridParent.transform.position,gridParent);
        for(int x = 0;x < item.size.x;x++){
            for(int y=0;y<item.size.y;y++){
                itemGrid.SetCellValue(x,y,item.values[x,y]);
            }
        }
    }

    void Start(){
        shelfGrid = GameObject.FindGameObjectWithTag("shelf").GetComponent<ShelfUI>().GetShelfGrid();
    }

    void Update(){
        if(shelfGrid == null){
            shelfGrid = GameObject.FindGameObjectWithTag("shelf").GetComponent<ShelfUI>().GetShelfGrid();
        }
        shelfGrid.ClearGrid();
        itemGrid.SendRaycastToGrid(shelfGrid);
    }

    public void SetTransformToMouse(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        itemGrid.UpdateGridPosition(transform.position);
        itemGrid.AlingToCenter(new Vector3(mousePos.x,mousePos.y,0));
        transform.position = new Vector3(mousePos.x,mousePos.y,0);
    }

    public Grid GetItemGrid(){
        return itemGrid;
    }





}

