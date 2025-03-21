using UnityEngine;

public class farm : MonoBehaviour
{
    Grid farmGrid;
    [SerializeField] GameObject gridStartPos;

    void Awake(){
        farmGrid = new Grid(4,4,1,gridStartPos.transform.position,gridStartPos);
        for(int x = 0;x<farmGrid.GetSize().x;x++){
            for(int y = 0;y<farmGrid.GetSize().y;y++){
                farmGrid.GetValueText(x,y).gameObject.tag = "farmGridCell";
            }
        }
    }
    public Grid GetFarmGrid(){return farmGrid;}
}
