using UnityEngine;

public class farm : MonoBehaviour
{
    Grid farmGrid;
    [SerializeField] GameObject gridStartPos;

    void Awake(){
        farmGrid = new Grid(4,4,1,gridStartPos.transform.position,gridStartPos);
    }
}
