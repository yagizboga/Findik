using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class BlacksmithIron : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Image ironImage;
    [SerializeField] TextMeshProUGUI ironCountUI;
    [SerializeField] Sprite ironSprite;
    [SerializeField] Canvas canvas;

    static int ironCount = 15;

    GameObject newIronObject;
    RectTransform newIronRect;
    Vector2 offset;

    void Awake()
    {
        ironCountUI.text = ironCount.ToString();
    }

    public void SetIronCount(int c)
    {
        ironCount = c;
        ironCountUI.text = ironCount.ToString();
    }

    public int GetIronCount() { return ironCount; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Yeni UI objesi oluştur
        newIronObject = new GameObject("DraggedIron");
        newIronObject.transform.SetParent(canvas.transform, false);
        newIronRect = newIronObject.AddComponent<RectTransform>();
        Image image = newIronObject.AddComponent<Image>();
        image.sprite = ironSprite;
        image.raycastTarget = false; // Diğer UI elemanlarını etkilemesin

        // Başlangıç pozisyonu ayarla
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPos
        );
        newIronRect.localPosition = localPos;

        // Offset hesapla
        offset = newIronRect.localPosition - (Vector3)localPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPointerPosition))
        {
            newIronRect.localPosition = localPointerPosition + offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(newIronObject);
        GameObject droppedOn = eventData.pointerEnter;
        if (droppedOn!= null && droppedOn.CompareTag("oven"))
        {
            Debug.Log("melting");
            if (droppedOn.gameObject.GetComponent<BlacksmithOven>().GetIsMelting() == false)
            {
                droppedOn.gameObject.GetComponent<BlacksmithOven>().MeltIron();
            }
            
        }
    }
}