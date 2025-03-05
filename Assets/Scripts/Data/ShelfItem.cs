using UnityEngine;
using System.Collections.Generic;

public class ShelfItem : MonoBehaviour
{
     ItemScriptableObject item;
    Grid itemGrid;
    Grid shelfGrid;
    List<Vector2> lastCells;

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
    }

    void Start(){
        shelfGrid = GameObject.FindGameObjectWithTag("shelf").GetComponent<ShelfUI>().GetShelfGrid();
    }

    void Update(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        itemGrid.UpdateGridPosition(transform.position);
        itemGrid.AlingToCenter(new Vector3(mousePos.x,mousePos.y,0));
        transform.position = new Vector3(mousePos.x,mousePos.y,0);
        SendRaycastToGrid(shelfGrid);
    }

    void SendRaycastToGrid(Grid grid){
        for(int x = 0;x<itemGrid.GetSize().x;x++){
            for(int y=0;y<itemGrid.GetSize().y;y++){
                Vector2 cellRaycastPosition = new Ray(itemGrid.GetCellMiddlePositions(x,y) - new Vector2(itemGrid.GetSize().x * itemGrid.GetSize().z / 2,itemGrid.GetSize().y * itemGrid.GetSize().z / 2),new Vector3(0,0,1)).GetPoint(10f);
                //Debug.DrawLine(itemGrid.GetCellMiddlePositions(x,y) - new Vector2(itemGrid.GetSize().x * itemGrid.GetSize().z / 2,itemGrid.GetSize().y * itemGrid.GetSize().z / 2),new Vector3(itemGrid.GetCellMiddlePositions(x,y).x,itemGrid.GetCellMiddlePositions(x,y).y,0) + new Vector3(0,0,40),Color.red,0.1f);
                //Debug.Log(x + " " + y + " " + cellRaycastPosition);
                grid.SetCellValue((int)grid.GetWorldToGridPosition(cellRaycastPosition).x,(int)grid.GetWorldToGridPosition(cellRaycastPosition).y,1);
                Vector2 lastCell = shelfGrid.GetWorldToGridPosition(cellRaycastPosition);
                lastCells.Add(lastCell);
                if(lastCells.Count > 0){
                    for(int i = 0;i<lastCells.Count;i++){
                        if(lastCells[i] != lastCell){
                             shelfGrid.SetCellValue((int)lastCells[i].x,(int)lastCells[i].y,0);
                        }
                    }

                }
        }
    }



}
}
