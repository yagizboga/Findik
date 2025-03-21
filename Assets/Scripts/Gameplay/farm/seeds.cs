using UnityEngine;

public class seeds : MonoBehaviour, IInteractable
{
    public void Interact(GameObject player){
        if(player.GetComponent<PlayerInteract>().GetIsPlanting()){
            player.GetComponent<PlayerInteract>().SetIsPlanting(false);
        } 
        else if(!player.GetComponent<PlayerInteract>().GetIsPlanting()){
            player.GetComponent<PlayerInteract>().SetIsPlanting(true);
        } 
    }
}
