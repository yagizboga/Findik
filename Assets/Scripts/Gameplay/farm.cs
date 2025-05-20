using System.Collections;
using Unity.Collections;
using UnityEngine;

public class farm : MonoBehaviour
{
    Grid farmGrid;
    [SerializeField] GameObject gridStartPos;
    public bool [,] isPlanted;
    public bool [,] isWatered;
    public bool [,] isGrowing;
    GameObject [,] squares;

    void Awake(){
        farmGrid = new Grid(4,4,1,gridStartPos.transform.position,gridStartPos);
        isPlanted = new bool[(int)farmGrid.GetSize().x,(int)farmGrid.GetSize().y];
        isWatered = new bool[(int)farmGrid.GetSize().x,(int)farmGrid.GetSize().y];
        isGrowing = new bool[(int)farmGrid.GetSize().x,(int)farmGrid.GetSize().y];
        squares = new GameObject[(int)farmGrid.GetSize().x,(int)farmGrid.GetSize().y];
        for(int x = 0;x<farmGrid.GetSize().x;x++){
            for(int y = 0;y<farmGrid.GetSize().y;y++){
                squares[x,y] = new GameObject();
                squares[x,y].transform.position = farmGrid.GetValueText(x,y).transform.position;
                farmGrid.GetValueText(x,y).gameObject.tag = "farmGridCell";
                squares[x,y].transform.parent = farmGrid.GetValueText(x,y).transform;
                squares[x,y].AddComponent<SpriteRenderer>();
                squares[x,y].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("FarmSquare");
                squares[x,y].GetComponent<SpriteRenderer>().sortingOrder = 1;
                squares[x,y].GetComponent<SpriteRenderer>().color = new Color32(0,150,45,255);
                isPlanted[x,y] = false;
                isWatered[x,y] = false;
                isGrowing[x,y] = false;
            }
        }
    }
    public Grid GetFarmGrid(){return farmGrid;}
    public GameObject GetFarmSquares(int x,int y){return squares[x,y];}

    public IEnumerator GrowPlant(int x, int y, int time){
        if(isWatered[x,y] && isPlanted[x,y]){
            squares[x,y].GetComponent<SpriteRenderer>().color = new Color32(0,75,45,255);
        }
        yield return new WaitForSeconds(time);
        squares[x,y].GetComponent<SpriteRenderer>().color = new Color32(0,150,45,255);
        isWatered[x,y] = false;
        isPlanted[x,y] = false;
    }
}
