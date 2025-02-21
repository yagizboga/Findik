using UnityEngine;

public interface IInteractable
{
    public void InteractStarted();
    public void InteractEnded();
    public bool isActive{get; set;}
}
