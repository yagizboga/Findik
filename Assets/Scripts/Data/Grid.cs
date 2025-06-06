using UnityEngine.UI;
using UnityEngine;
using System.Collections.Specialized;
using TMPro;
using Unity.Collections;

public class Grid{
    int width,height;
    float cellSize;
    int [,] shelfGrid;
    Vector3 startPos;
    Vector2 [,] cellPositions;
    Vector2 [,] cellMiddlePositions;
    int [,] values;
    public GameObject textParentCanvas;
    GameObject [,]valueText;

    public Grid(int width, int height, int cellSize, Vector3 startPos, GameObject textParentCanvas){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.startPos = startPos;
        shelfGrid = new int[width,height];
        cellPositions = new Vector2 [width,height]; 
        cellMiddlePositions = new Vector2[width,height];
        values = new int[width,height];
        this.textParentCanvas = textParentCanvas;
        valueText = new GameObject[width,height];

        for(int x = 0;x < width;x++){
            for(int y = 0;y<height;y++){
                values[x,y] = 0;
                cellPositions[x,y] = new Vector2(startPos.x + x * cellSize, startPos.y + y * cellSize);
                cellMiddlePositions[x,y] = new Vector2(startPos.x + x * cellSize + cellSize / 2f, startPos.y + y * cellSize + cellSize / 2f);
                Debug.DrawLine(new Vector3(startPos.x + x* cellSize, startPos.y + y* cellSize)
                                            ,new Vector3(startPos.x + (x+1) * cellSize,startPos.y + y * cellSize,0),Color.white,1000f);

               Debug.DrawLine(new Vector3(startPos.x + x* cellSize, startPos.y + y* cellSize)
                                            ,new Vector3(startPos.x +x * cellSize,startPos.y + (y+1) * cellSize,0),Color.white,1000f);
                //Debug.Log(cellMiddlePositions[x,y]);
                Debug.DrawLine(new Vector3(cellMiddlePositions[x,y].x - cellSize/20f,cellMiddlePositions[x,y].y,0)
                                            ,new Vector3(cellMiddlePositions[x,y].x + cellSize/20f,cellMiddlePositions[x,y].y,0)
                                            ,Color.red,1000f);
                valueText[x,y] = new GameObject();
//                Debug.Log(valueText[x,y] + " " + valueText[x,y].transform.position);
                valueText[x,y].AddComponent<TextMeshPro>();
                valueText[x,y].GetComponent<RectTransform>().anchoredPosition = cellMiddlePositions[x,y];
                valueText[x,y].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize,cellSize);
                valueText[x,y].GetComponent<TextMeshPro>().verticalAlignment = VerticalAlignmentOptions.Middle;
                valueText[x,y].GetComponent<TextMeshPro>().horizontalAlignment = HorizontalAlignmentOptions.Center;
                valueText[x,y].GetComponent<TextMeshPro>().text = values[x,y].ToString();
                valueText[x,y].GetComponent<TextMeshPro>().fontSize = cellSize*4;
                valueText[x,y].GetComponent<TextMeshPro>().sortingOrder = 7;
                valueText[x,y].transform.SetParent(textParentCanvas.transform);

                valueText[x,y].AddComponent<BoxCollider2D>();
                valueText[x,y].GetComponent<BoxCollider2D>().isTrigger = true;
                valueText[x,y].GetComponent<BoxCollider2D>().size = new Vector2(cellSize,cellSize);
            }


        }
    }

    public void UpdateGridPosition(Vector3 newPos){
        for(int x = 0;x < width;x++){
            for(int y = 0;y<height;y++){
                cellPositions[x,y] = new Vector2(newPos.x + x * cellSize, newPos.y + y * cellSize);
                cellMiddlePositions[x,y] = new Vector2(newPos.x + x * cellSize + cellSize / 2f, newPos.y + y * cellSize + cellSize / 2f);
            }


        }
    }
    public Vector3 GetSize(){
        return new Vector3(width,height,cellSize);
    }
    public Vector2 GetWorldToGridPosition(Vector3 worldPos){
        return new Vector2((int)((worldPos.x - startPos.x) / cellSize), (int)((worldPos.y - startPos.y)/cellSize));
    }

    public void SetCellValue(int x , int y, int value){
        if(x < width && y < height && x >= 0 && y >= 0){
            values[x,y] = value;
             valueText[x,y].GetComponent<TextMeshPro>().text = values[x,y].ToString();
        }
    }
    public int GetCellValue(int x, int y){
        return values[x,y];
    }

    public int IsCellEmpty(int x,int y){
        if(x < width && y < height && x >= 0 && y >= 0){
            if(values[x,y] == 0){
                return 1;
            }
            else{return 0;}
        }
        else{
            return -1;
        }
    }

    public Vector2 GetCellMiddlePositions(int x,int y){
        return cellMiddlePositions[x,y];
    }


    public void AlignToCenter(Vector3 alignTo){
        textParentCanvas.transform.position = alignTo - new Vector3(width * cellSize / 2 , height * cellSize / 2);
    }

    public GameObject GetValueText(int x,int y){
        return valueText[x,y];
    }

    public void ClearGrid(){
        for(int x = 0;x<width;x++){
            for(int y=0;y<height;y++){
                values[x,y] = 0;
                valueText[x,y].GetComponent<TextMeshPro>().text = 0.ToString();
            }
        }
    }


    public void TransposeMatrix(){
            int [,] tempValues = new int[height,width];
            for(int x = 0;x< width;x++){
                for(int y = 0;y<height;y++){
                    values[x,y] = tempValues[y,x];
                }
            }
            values = new int[height,width];
            values = tempValues;
            for(int x = 0;x< width;x++){
                for(int y = 0;y<height;y++){
                    cellMiddlePositions[x,y] = new Vector2(startPos.x + x * cellSize + cellSize / 2f, startPos.y + y * cellSize + cellSize / 2f);
                    valueText[x,y].GetComponent<RectTransform>().anchoredPosition = cellMiddlePositions[x,y];
                }
            }
    }

    public void SendRaycastToGrid(Grid grid){
        for(int x = 0;x<GetSize().x;x++){
            for(int y=0;y<GetSize().y;y++){
                Vector2 cellRaycastPosition = new Ray(new Vector2(GetValueText(x,y).transform.position.x,GetValueText(x,y).transform.position.y),new Vector3(0,0,1)).GetPoint(10f);
                if(GetCellValue(x,y) == 1){
                    grid.SetCellValue((int)grid.GetWorldToGridPosition(cellRaycastPosition).x,(int)grid.GetWorldToGridPosition(cellRaycastPosition).y,1);
                }


            }
        }

    }

    public void SetGridPosToGrid(Grid grid){
        for(int x = 0;x<width;x++){
            for(int y = 0;y<height;y++){
                valueText[x,y].transform.position =grid.cellMiddlePositions[(int)grid.GetWorldToGridPosition(valueText[x,y].transform.position).x,(int)grid.GetWorldToGridPosition(valueText[x,y].transform.position).y];
            }
        }
    }

    public bool isOnGrid(Grid grid){
        for(int x = 0;x<width;x++){
            for(int y =0;y<height;y++){
                if(grid.GetWorldToGridPosition(new Vector3(valueText[x,y].transform.position.x,valueText[x,y].transform.position.y,0)).x > grid.GetSize().x ||
                    grid.GetWorldToGridPosition(new Vector3(valueText[x,y].transform.position.x,valueText[x,y].transform.position.y,0)).y > grid.GetSize().y ||
                    grid.GetWorldToGridPosition(new Vector3(valueText[x,y].transform.position.x,valueText[x,y].transform.position.y,0)).x < 0 ||
                    grid.GetWorldToGridPosition(new Vector3(valueText[x,y].transform.position.x,valueText[x,y].transform.position.y,0)).y < 0)
                    {
                       // Debug.Log(grid.GetWorldToGridPosition(new Vector3(valueText[x,y].transform.position.x,valueText[x,y].transform.position.y,0)).x + " " +
                        //        grid.GetWorldToGridPosition(new Vector3(valueText[x,y].transform.position.x,valueText[x,y].transform.position.y,0)).y
                         //       + " " + width + " "  + height);
                        return false;

                    }
            }
        }
        return true;
    }

    public bool isGridFull(Grid grid){
        for(int x=0;x<width;x++){
            for(int y=0;y<height;y++){
                Debug.Log(grid.GetCellValue((int)grid.GetWorldToGridPosition(new Vector3(valueText[x,y].transform.position.x,valueText[x,y].transform.position.y,0)).x,
                                    (int)grid.GetWorldToGridPosition(new Vector3(valueText[x,y].transform.position.x,valueText[x,y].transform.position.y,0)).y));
                if(grid.GetCellValue((int)grid.GetWorldToGridPosition(new Vector3(valueText[x,y].transform.position.x,valueText[x,y].transform.position.y,0)).x,
                                    (int)grid.GetWorldToGridPosition(new Vector3(valueText[x,y].transform.position.x,valueText[x,y].transform.position.y,0)).y) == 1){
                    return true;
                }
            }
        }
        return false;
    }

    public Vector2 GetCellWorldPositions(int x,int y){
            return valueText[x,y].transform.position;
    }

    public Vector2 GetGridPosToGrid(Grid grid,int xPos,int yPos){
        if(xPos < width && yPos < height && xPos >= 0 && yPos >= 0){
            return grid.GetWorldToGridPosition(valueText[xPos,yPos].transform.position);
        }
        else{return new Vector2(-1,-1);}
    }

    public void SetCellPosition(int x,int y,float posX,float posY){
        valueText[x,y].transform.position = new Vector2(posX,posY);
    }

    public void ToItem(){
        for(int x = 0;x< width;x++){
            for(int y = 0;y<height;y++){
                valueText[x,y].tag = "itemCell";
            }
        }
    }

    public void AlignToChild(){
        Vector2 [,]temp = new Vector2[width,height];
        for(int x = 0;x<width;x++){
            for(int y=0;y<height;y++){
                temp[x,y] = valueText[x,y].transform.position;
            }
        }

        valueText[0,0].transform.parent.transform.parent.gameObject.transform.position = new Vector3(width * cellSize / 2f , height * cellSize / 2f,0);
    
        for(int x = 0;x<width;x++){
            for(int y=0;y<height;y++){
                valueText[x,y].transform.position = temp[x,y];
            }
        }
    }

    public Vector2 AlignToValueText(GameObject obj){
        float xAvg=0,yAvg=0;

        for(int x=0;x<width;x++){
            xAvg += valueText[x,0].transform.position.x;
        }
        for(int y=0;y<height;y++){
            yAvg += valueText[0,y].transform.position.y;
        }
        xAvg = xAvg/width;
        yAvg = yAvg/height;
        return new Vector2(xAvg,yAvg);
    }


}
