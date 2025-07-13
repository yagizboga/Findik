using UnityEngine;

public class Table : MonoBehaviour, IInteractable
{
    //[SerializeField] GameObject table;
    [SerializeField] Camera cam;
    [SerializeField] Transform tablePos;
    [SerializeField] Transform MainRoomPos;
    bool isActive;
    public void Interact(GameObject player){
        if (isActive)
        {
            //table.gameObject.SetActive(false);
            isActive = false;
            cam.transform.position = new Vector3(MainRoomPos.transform.position.x,MainRoomPos.transform.position.y,cam.transform.position.z);
        }
        else if (!isActive)
        {
            //table.gameObject.SetActive(true);
            isActive = true;
            cam.transform.position = new Vector3(tablePos.transform.position.x,tablePos.transform.position.y,cam.transform.position.z);
        }
    }
}
