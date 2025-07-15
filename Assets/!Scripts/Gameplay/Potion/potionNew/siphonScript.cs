using UnityEngine;

public class SiphonScript : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 originalPosition;
    public float returnSpeed = 5f;
    public float maxYOffset = 3f;
    private float yOffset;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        isDragging = true;
        // Calculate initial offset between mouse and siphon position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        yOffset = transform.position.y - mousePos.y;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            // Get mouse position in world coordinates
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = originalPosition.z; // Maintain original z position
            
            // Apply the y offset we calculated in OnMouseDown
            float targetY = mousePos.y + yOffset;
            
            // Clamp the position so it doesn't go too far down
            targetY = Mathf.Clamp(targetY, originalPosition.y - maxYOffset, originalPosition.y);
            
            transform.position = new Vector3(originalPosition.x, targetY, originalPosition.z);
        }
        else if (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            // Smoothly return to original position
            transform.position = Vector3.Lerp(transform.position, originalPosition, returnSpeed * Time.deltaTime);
        }
    }
}