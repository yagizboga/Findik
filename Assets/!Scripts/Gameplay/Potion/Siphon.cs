using UnityEngine;
using UnityEngine.InputSystem;

public class Siphon : MonoBehaviour
{
    private bool canSiphon = false;
    private bool isDragging = false;
    private bool isReturning = false;
    private bool limitReached = false;

    private Vector3 initialLocalPos;
    private Vector3 targetReturnPos;
    private float minY;

    private float moveSpeed = 15f;
    private float returnSpeed = 3f;

    public PotionRecipe recipe;
    public bool resetSiphon = true;

    void Start()
    {
        initialLocalPos = transform.localPosition;
        minY = initialLocalPos.y - 1f;
    }

    void Update()
    {
        if (isDragging && !isReturning)
        {
            float mouseY = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
            float newY = Mathf.Clamp(transform.localPosition.y + mouseY, minY, initialLocalPos.y);

            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

            if (!limitReached && Mathf.Abs(newY - minY) < 0.05f)
            {
                if (resetSiphon)
                {
                    recipe.ResetRecipe();
                    Debug.Log("Resetted!");
                }
                else
                {
                    recipe.ClaimPotion();
                }
                
                limitReached = true;
            }
        }

        if (isReturning)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * returnSpeed);

            if (Vector3.Distance(transform.localPosition, initialLocalPos) < 0.01f)
            {
                transform.localPosition = initialLocalPos;
                isReturning = false;
                limitReached = false; 
            }
        }
    }

    private void OnMouseEnter()
    {
        canSiphon = true;
    }

    private void OnMouseExit()
    {
        canSiphon = false;
    }

    public void DragSiphon(InputAction.CallbackContext context)
    {
        if (context.performed && canSiphon && !isReturning)
        {
            isDragging = true;
            Cursor.visible = false;
        }
        else if (context.canceled && isDragging)
        {
            isDragging = false;
            isReturning = true;
            Cursor.visible = true;
        }
    }
}
