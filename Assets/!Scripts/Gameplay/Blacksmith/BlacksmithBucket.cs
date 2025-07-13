using UnityEngine;
using UnityEngine.EventSystems;

public class BlacksmithBucket : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private Vector2 lastMousePosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 delta = currentMousePosition - lastMousePosition;

        // X ve Y farklarını birleştirerek yön belirle
        float direction = delta.x + delta.y;

        float rotationSpeed = 100f;
        transform.Rotate(Vector3.forward, -direction * Time.deltaTime * rotationSpeed);

        lastMousePosition = currentMousePosition;
    }
}