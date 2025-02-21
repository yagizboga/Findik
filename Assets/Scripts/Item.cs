using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rectTransform;

    void Awake(){
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData){
        gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color32(170,170,170,255);
    }

    public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData){
        gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color32(255,255,255,255);
    }
}
