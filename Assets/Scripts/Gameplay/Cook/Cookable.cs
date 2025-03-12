using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cookable : MonoBehaviour
{
    public bool isCooked = false;
    private bool isCooking = true;

    public void SetIsCooking(bool sett)
    {
        isCooking = sett;
        Debug.Log("Is Cooking: " + isCooking);
    }
    /*public void SpawnIngredientInput(InputAction.CallbackContext context)
    {
        if (context.performed && canSpawn && currentIngredient == null)
        {
            SpawnIngredient();
        }
    }
    public void DragIngredientInput(InputAction.CallbackContext context)
    {
        if (context.performed && currentIngredient != null)
        {
            canDrag = true;
        }
        else if (context.canceled)
        {
            canDrag = false;
        }
    }
    public void ReleaseIngredientInput(InputAction.CallbackContext context)
    {
        if (context.canceled && currentIngredient != null)
        {
            canDrag = false;
            DropIngredient();
        }
    }*/
}
    