using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shelf_UI_Item : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    ItemScriptableObject item;
    RectTransform rectTransform;
    SpriteRenderer image;
    Rigidbody2D rb;
    bool isDragging = false;
    bool isCollidedWithBar = false;
    bool isTriggeredWithBar = false;

    void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        image = gameObject.GetComponent<SpriteRenderer>(); 
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update(){
        if(isDragging){
            rb.gravityScale = 0;
        }
        else{
            rb.gravityScale = 1;
        }

        if(isCollidedWithBar || isTriggeredWithBar){
            if(isDragging){
                GetComponent<PolygonCollider2D>().isTrigger = true;
            }
        }
        else{
            GetComponent<PolygonCollider2D>().isTrigger = false;
        }

//        Debug.Log(isCollidedWithBar + " " + isTriggeredWithBar);
    }

    public void OnBeginDrag(PointerEventData eventData){
        image.color = new Color32(255,255,255,170);
        isDragging = true;
    }
    public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta;
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }
    public void OnEndDrag(PointerEventData eventData){
        image.color = new Color32(255,255,255,255);
        isDragging = false;
        GetComponent<PolygonCollider2D>().isTrigger = false;
    }


    

}
