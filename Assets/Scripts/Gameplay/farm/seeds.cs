using UnityEngine;
using System.Collections.Generic;

public class seeds : MonoBehaviour, IInteractable
{
    List<SeedScriptableObject> seedobjects = new List<SeedScriptableObject>();
    public void Interact(GameObject player)
    {
        if (player.GetComponent<PlayerInteract>().GetIsPlanting())
        {
            player.GetComponent<PlayerInteract>().SetIsPlanting(false);
        }
        else if (!player.GetComponent<PlayerInteract>().GetIsPlanting())
        {
            player.GetComponent<PlayerInteract>().SetIsPlanting(true);
        }
    }

    public void AddSeed(SeedScriptableObject seed){
        seedobjects.Add(seed);
    }
    

}
