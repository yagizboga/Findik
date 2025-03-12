using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cookable : MonoBehaviour
{
    public bool isCooked = false;
    private bool isCooking = true;
    private bool canDrag = false;

    private void Update()
    {
        if (canDrag)
        {
            Vector2 pointerPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, 10));
            transform.position = worldPos;
        }
    }
    public void SetIsCooking(bool sett)
    {
        isCooking = sett;
        Debug.Log("Is Cooking: " + isCooking);
    }
    public void DragIngredientInput(InputAction.CallbackContext context)
    {
        if (context.performed && canDrag)
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
        if (context.canceled)
        {
            canDrag = false;
            transform.position = Vector3.zero;
        }
    }

}
    