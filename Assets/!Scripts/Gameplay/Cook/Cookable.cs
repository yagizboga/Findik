using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cookable : IngredientTypes
{
    public bool isCooked = false;
    public bool isCooking = true;
    private bool isDragging = false;
    public bool canDrag = false;
    public bool isDragBlocked = false;
    private IngredientHolder ingredientHolder;
    private Ingredient ingredient;

    public Color cookedColor;
    public Color burnedColor;

    private SpriteRenderer spriteRenderer;

    private float cookTimer = 0f; 
    private float cookTime = 10f;
    private float burnTime = 20f;

    private float proximityThreshold = 0.5f;

    public static GameObject instance;

    private Transform originalParent;


    private bool inRange = false;


    public InputActionAsset inputActions;
    private InputAction dragAction;
    private InputAction releaseAction;

    private void Start()
    {
        ingredientHolder = GameObject.FindGameObjectWithTag("Ingredient Holder").GetComponent<IngredientHolder>();
        ingredient = GetComponent<Ingredient>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //instance = gameObject;

        dragAction = inputActions.FindAction("Select");
        releaseAction = inputActions.FindAction("Select");

        dragAction.performed += OnDragPerformed;
        releaseAction.canceled += OnReleaseCanceled;

        dragAction.Enable();
        releaseAction.Enable();
    }

    private void Update()
    {
        Vector3 origin = transform.position;
        Vector3 target = origin + Vector3.right * proximityThreshold; 

        Debug.DrawLine(origin, target, Color.red);



        if (isCooking && !isDragging)
        {
            cookTimer += Time.deltaTime;
            if (cookTimer >= cookTime && cookTimer < burnTime && !isCooked)
            {
                isCooked = true;
                spriteRenderer.color = cookedColor;
            }
            else if(cookTimer >= burnTime && isCooked)
            {
                isCooking = false;
                isCooked = false;
                spriteRenderer.color = burnedColor;
            }
        }

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
            else if(distance > proximityThreshold && !inRange)
            {
                canDrag = false;
                if(instance == gameObject)
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

    public void SetIsCooking(bool sett)
    {
        isCooking = sett;
        isDragBlocked = true;
    }
   /* public void DragIngredientInput(InputAction.CallbackContext context)
    {
        if (context.performed && canDrag && !isDragBlocked && instance == gameObject)
        {
            isDragging = true;

            if (originalParent == null)
                originalParent = transform.parent;

            transform.SetParent(null);
        }
    }
    public void ReleaseIngredientInput(InputAction.CallbackContext context)
    {
        if (context.canceled && isDragging && !isDragBlocked)
        {
            isDragging = false;
            //ingredientHolder.SetIsGrillMatching(false);

            if (originalParent != null)
            {
                transform.SetParent(originalParent);
            }
            ingredientHolder.DropIngredient(gameObject);
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        }
    }*/

    private void Drag()
    {
        if (canDrag && !isDragBlocked /*&& instance == gameObject*/)
        {
            isDragging = true;

            if (originalParent == null)
                originalParent = transform.parent;

            transform.SetParent(null);
        }
    }

    private void Release()
    {
        if (isDragging && !isDragBlocked)
        {
            isDragging = false;

            if (originalParent != null)
            {
                transform.SetParent(originalParent);
            }

            ingredientHolder.DropIngredient(gameObject);
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnDestroy()
    {
        if (dragAction != null)
            dragAction.performed -= OnDragPerformed;

        if (releaseAction != null)
            releaseAction.canceled -= OnReleaseCanceled;
    }

    private void OnDragPerformed(InputAction.CallbackContext ctx)
    {
        Drag();
    }

    private void OnReleaseCanceled(InputAction.CallbackContext ctx)
    {
        Release();
    }

   /* private void OnMouseEnter()
    {
        Debug.Log("Mouse Entered");
        inRange = true;
        canDrag = true;
    }

    private void OnMouseExit()
    {
        inRange = false;
        canDrag = false;
    }
   */
    private void StartCookTimer()
    {
        cookTimer = 0f; 
    }

    public void DropBread()
    {
        ingredientHolder.DropBread(gameObject);
    }
}
    