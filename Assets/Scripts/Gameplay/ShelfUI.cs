using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ShelfUI : MonoBehaviour
{
    [SerializeField] Transform gridStartPos;
    [SerializeField] GameObject textParentCanvas;
    Grid shelfGrid;
    Vector3 mousePos;
    List<Vector2> lastCells;


    void Awake(){
        shelfGrid = new Grid(16,9,1,gridStartPos.position,textParentCanvas);
        lastCells = new List<Vector2>();

    }
    void Update(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        shelfGrid.SetCellValue((int)shelfGrid.GetWorldToGridPosition(mousePos).x , (int)shelfGrid.GetWorldToGridPosition(mousePos).y , 1);
        Vector2 lastCell = shelfGrid.GetWorldToGridPosition(mousePos);
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
