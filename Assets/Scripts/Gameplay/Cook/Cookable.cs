using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cookable : MonoBehaviour
{
    public bool isCooked = false;
    private bool isCooking = true;
    private bool isDragging = false;
    private bool canDrag = false;
    private bool isDragBlocked = false;
    private IngredientHolder ingredientHolder;

    private void Start()
    {
        ingredientHolder = GameObject.FindGameObjectWithTag("Ingredient Holder").GetComponent<IngredientHolder>();
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector2 pointerPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, 10));
            transform.position = worldPos;
        }
    }
    public void SetIsCooking(bool sett)
    {
        isCooking = sett;
        isDragBlocked = true;
        //Debug.Log("Is Cooking: " + isCooking);
    }
    public void DragIngredientInput(InputAction.CallbackContext context)
    {
        if (context.performed && canDrag && !isDragBlocked)
        {
            isDragging = true;
        }
        /*else if (context.canceled)
        {
            isDragging = false;
        }*/
    }
    public void ReleaseIngredientInput(InputAction.CallbackContext context)
    {
        if (context.canceled && isDragging && !isDragBlocked)
        {
            isDragging = false;
            ingredientHolder.SetIsGrillMatching(false);
            ingredientHolder.DropIngredient(gameObject);
            //transform.localPosition = Vector3.zero;
        }
    }

    private void OnMouseEnter()
    {
        canDrag = true;
    }

    private void OnMouseExit()
    {
        canDrag = false;
    }


}
    