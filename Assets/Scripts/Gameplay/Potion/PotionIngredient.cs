using UnityEngine;
using UnityEngine.InputSystem;

public class PotionIngredient : PotionIngredientTypes
{
    public PotionIngredientType type;
    private bool isDragging = false;
    public bool canDrag = false;
    public bool isDragBlocked = false;


    private float proximityThreshold = 0.25f;

    public static GameObject instance;

    private bool inRange = false;

    private PotionRecipe potionRecipe;
    private bool isMatching = false;


    private void Update()
    {
        HandleDragging();
    }

    private void HandleDragging()
    {
        if (Pointer.current != null)
        {
            Vector2 pointerPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, 10));

            float distance = Vector2.Distance(worldPos, transform.position);

            if (distance <= proximityThreshold)
            {
                canDrag = true;
                if (instance == null)
                {
                    instance = gameObject;
                }
            }
            else if (distance > proximityThreshold && !inRange)
            {
                canDrag = false;
                if (instance == gameObject)
                {
                    instance = null;
                }
            }
            else if (inRange)
            {
                canDrag = true;
                if (instance == null)
                {
                    instance = gameObject;
                }
            }
            if (isDragging)
            {
                transform.position = worldPos;
            }
        }
        else if (Gamepad.current != null)
        {
            Vector2 controllerInput = Gamepad.current.leftStick.ReadValue();
            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(controllerInput.x, controllerInput.y));
            canDrag = controllerInput.magnitude > 0.1f;

            if (isDragging)
            {
                transform.position += new Vector3(controllerInput.x, controllerInput.y, 0) * Time.deltaTime * 5f;
            }
        }
    }

    public void DragIngredientInput(InputAction.CallbackContext context)
    {
        if (context.performed && canDrag && !isDragBlocked && instance == gameObject)
        {
            isDragging = true;
        }
    }
    public void ReleaseIngredientInput(InputAction.CallbackContext context)
    {
        if (context.canceled && isDragging && !isDragBlocked)
        {
            isDragging = false;
            if (isMatching)
            {
                //ingredientHolder.SetIsGrillMatching(false);
                potionRecipe.DropIngredient(gameObject);
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }
            
        }
    }

    public void SetRecipe(GameObject recipe)
    {
        potionRecipe = recipe.GetComponent<PotionRecipe>();
    }

    private void OnMouseEnter()
    {
        inRange = true;
        canDrag = true;
    }

    private void OnMouseExit()
    {
        inRange = false;
        canDrag = false;
    }

    public void SetIsMatching(bool match)
    {
        isMatching = match;
    }
}
