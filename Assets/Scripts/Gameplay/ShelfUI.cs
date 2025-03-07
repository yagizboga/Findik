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
    GameObject testItem;
    GameObject currentItem;


    void Awake(){
        shelfGrid = new Grid(16,9,1,gridStartPos.position,textParentCanvas);
        lastCells = new List<Vector2>();

        testItem = new GameObject();
        testItem.AddComponent<ShelfItem>();
        currentItem = testItem;

    }
    void Update(){
        currentItem.GetComponent<ShelfItem>().SetTransformToMouse();
    }   
    public Grid GetShelfGrid(){
            return shelfGrid;
    }
}
