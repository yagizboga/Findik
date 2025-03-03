using UnityEngine;

public class ShelfItem : MonoBehaviour
{
     ItemScriptableObject item;
    Grid itemGrid;

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

    void Update(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x,mousePos.y,0);
    }



}
