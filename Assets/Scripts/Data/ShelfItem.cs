using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class ShelfItem : MonoBehaviour
{
    ItemScriptableObject item;
    Grid itemGrid;
    Grid shelfGrid;
    List<Vector2> lastCells;
    Vector2 lastCell;
    GameObject sprite;
    Vector3 mousePosStart;

    void Awake(){
        lastCells = new List<Vector2>();
        //gridParent.GetComponent<SpriteRenderer>().sortingOrder = 8;
        //gridParent.GetComponent<SpriteRenderer>().sprite = item.GetSprite();

    }

    public void Initialize(ItemScriptableObject.shelfItemType type){
        GameObject gridParent = new GameObject();
        item = new ItemScriptableObject();
        item.Initialize(type);
        sprite = new GameObject();
        sprite.transform.parent = gridParent.transform;
        if(sprite.GetComponent<SpriteRenderer>() == null){
            sprite.AddComponent<SpriteRenderer>();
        }
        sprite.GetComponent<SpriteRenderer>().sprite = item.GetSprite();
        sprite.GetComponent<SpriteRenderer>().sortingOrder = 1;
        gridParent.transform.SetParent(transform);
        gridParent.transform.localPosition = new Vector3(0,0,0);
        itemGrid = new Grid(item.size.x,item.size.y,1,gridParent.transform.position,gridParent);
        for(int x = 0;x < item.size.x;x++){
            for(int y=0;y<item.size.y;y++){
                itemGrid.SetCellValue(x,y,item.values[x,y]);
            }
        }
        itemGrid.ToItem(); 
        sprite.gameObject.transform.localPosition = itemGrid.AlignToValueText(sprite.gameObject);
    }

    void Start(){
        shelfGrid = transform.parent.GetComponent<ShelfUI>().GetShelfGrid();
        mousePosStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update(){
        sprite.gameObject.transform.position = itemGrid.AlignToValueText(sprite.gameObject);
        if(shelfGrid == null){
            shelfGrid = transform.parent.GetComponent<ShelfUI>().GetShelfGrid();
        }
        shelfGrid.ClearGrid();
        itemGrid.SendRaycastToGrid(shelfGrid);
    }

    public void SetTransformToMouse(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //itemGrid.UpdateGridPosition(transform.position);
        //itemGrid.AlignToCenter(new Vector3(mousePos.x,mousePos.y,0));
        //transform.position = new Vector3(mousePos.x,mousePos.y,0);
        gameObject.transform.position = mousePos - new Vector3(itemGrid.GetSize().x /2f,itemGrid.GetSize().y /2f,0);
    }

    public Grid GetItemGrid(){
        return itemGrid;
    }

    





}

