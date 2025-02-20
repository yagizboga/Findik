using UnityEngine;

public class Grid
{
    int width;
    int height;
    int cellSize;
    float startPosX;
    float startPosY;


    public Grid(int width,int height,float startPosX,float startPosY,int cellSize){
        this.width = width;
        this.height = height;
        this.startPosX = startPosX;
        this.startPosY = startPosY;
        this.cellSize = cellSize;
    
        for(int x = 0;x<width;x++){
            for(int y=0;y<height;y++){
                Debug.Log(x + " "+ y);
                Debug.DrawRay(new Vector3(startPosX + x * cellSize,startPosY + y * cellSize),new Vector3(cellSize,0,0),Color.white,1000f);
                Debug.DrawRay(new Vector3(startPosX + x * cellSize,startPosY + y * cellSize),new Vector3(0,cellSize,0),Color.white,1000f);
            }
        }
    

        
    }

    public Vector2Int GetMouseGridPosition(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x > startPosX && mousePos.x < startPosX + width * cellSize
            &&mousePos.y > startPosY && mousePos.y < startPosY + width * cellSize){

                int gridX = Mathf.FloorToInt((mousePos.x - startPosX) / cellSize);
                int gridY = Mathf.FloorToInt((mousePos.y - startPosY) / cellSize);

                return new Vector2Int(gridX,gridY);

        }
        else{
             return new Vector2Int(-1, -1);
        }
    }


}
