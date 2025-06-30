    using UnityEngine;
using UnityEngine.InputSystem;

public class Spoon : MonoBehaviour
{
    private float detectionRange = 30f; // not in need anymore
    private float moveSpeed = 30f;
    private float targetMix = 30f;

    private bool canSpoon = false;
    private float mixAmount = 0f;
    private Vector3 initialPosition;
    private float minX, maxX;
    private bool isDragging = false;
    private float lastXPosition;

    public PotionRecipe recipe;

    void Start()
    {
        initialPosition = transform.position;
        minX = initialPosition.x - 1f;
        maxX = initialPosition.x + 1f;
        lastXPosition = transform.position.x;
    }

    void Update()
    {
        //Debug.Log(mixAmount);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;


        if (isDragging)
        {
            float mouseXMovement = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;  // NO CONTROLLERR SUPPORT FOR NOW !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            float newX = Mathf.Clamp(transform.position.x + mouseXMovement, minX, maxX);

            if (newX != transform.position.x)
            {
                mixAmount += Mathf.Abs(newX - lastXPosition);
            }

            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            lastXPosition = newX;

            if (mixAmount >= targetMix)
            {
                Debug.Log("Mix Done");
                mixAmount = 0f;
                recipe.SetDidSpoon();
                recipe.CheckStatus();
            }
        }
    }

    public void ResetMix()
    {
        mixAmount = 0f;
        recipe.SetDidSpoon();
    }

    private void OnMouseEnter()
    {
        canSpoon = true;
    }

    private void OnMouseExit()
    {
        canSpoon = false;
    }

    public void DragSpoon(InputAction.CallbackContext context)
    {
        if (context.performed && canSpoon)
        {
            isDragging = true;
            Cursor.visible = false;
        }
        else if (context.canceled)
        {
            isDragging = false;
            Cursor.visible = true;
        }
    }

}
