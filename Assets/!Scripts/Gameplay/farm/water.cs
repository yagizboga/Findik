using UnityEngine;

public class water : MonoBehaviour, IInteractable
{
    public void Interact(GameObject player){
        if(player.GetComponent<PlayerInteract>().GetIsWatering()){
            player.GetComponent<PlayerInteract>().SetIsWatering(false);
        } 
        else if(!player.GetComponent<PlayerInteract>().GetIsWatering()){
            player.GetComponent<PlayerInteract>().SetIsWatering(true);
        } 
    }
}
