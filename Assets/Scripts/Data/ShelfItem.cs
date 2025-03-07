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
        item = new ItemScriptableObject(ItemScriptableObject.shelfItemType.key);
        GameObject gridParent = new GameObject();
        gridParent.transform.SetParent(transform);
        gridParent.transform.localPosition = new Vector3(0,0,0);
        itemGrid = new Grid(item.size.x,item.size.y,1,gridParent.transform.position,gridParent);
        for(int x = 0;x < item.size.x;x++){
            for(int y=0;y<item.size.y;y++){
                itemGrid.SetCellValue(x,y,item.values[x,y]);
            }
        }
        lastCells = new List<Vector2>();
        gameObject.AddComponent<SpriteRenderer>();
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 8;
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,170);

    }

    void Start(){
        shelfGrid = GameObject.FindGameObjectWithTag("shelf").GetComponent<ShelfUI>().GetShelfGrid();
    }

    void Update(){
        if(shelfGrid == null){
            shelfGrid = GameObject.FindGameObjectWithTag("shelf").GetComponent<ShelfUI>().GetShelfGrid();
        }
        shelfGrid.ClearGrid();
        SendRaycastToGrid(shelfGrid);
    }

    public void SetTransformToMouse(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        itemGrid.UpdateGridPosition(transform.position);
        itemGrid.AlingToCenter(new Vector3(mousePos.x,mousePos.y,0));
        transform.position = new Vector3(mousePos.x,mousePos.y,0);
    }


    void SendRaycastToGrid(Grid grid){
        for(int x = 0;x<itemGrid.GetSize().x;x++){
            for(int y=0;y<itemGrid.GetSize().y;y++){
                Vector2 cellRaycastPosition = new Ray(new Vector2(itemGrid.GetValueText(x,y).transform.position.x,itemGrid.GetValueText(x,y).transform.position.y),new Vector3(0,0,1)).GetPoint(10f);
                //Debug.DrawLine(new Vector2(itemGrid.GetValueText(x,y).transform.position.x,itemGrid.GetValueText(x,y).transform.position.y),new Vector3(itemGrid.GetValueText(x,y).transform.position.x,itemGrid.GetValueText(x,y).transform.position.y,0) + new Vector3(0,0,20),Color.red,0.1f);
                //Debug.Log(x + " " + y + " " + cellRaycastPosition);
                Debug.Log(grid.GetWorldToGridPosition(cellRaycastPosition));
                if(itemGrid.GetCellValue(x,y) == 1){
                    grid.SetCellValue((int)grid.GetWorldToGridPosition(cellRaycastPosition).x,(int)grid.GetWorldToGridPosition(cellRaycastPosition).y,1);
                }


        }
    }



}
}
