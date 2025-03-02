using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shelf_UI_Item : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    ItemScriptableObject item;
    RectTransform rectTransform;
    Image image;
    Rigidbody2D rb;
    bool isDragging = false;
    bool isCollidedWithBar = false;

    void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        image = gameObject.GetComponent<Image>(); 
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update(){
        if(isDragging || isCollidedWithBar){
            rb.gravityScale = 0;
        }
        else{
            rb.gravityScale = 1;
        }
    }

    public void OnBeginDrag(PointerEventData eventData){
        image.color = new Color32(255,255,255,170);
        isDragging = true;
    }
    public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta;
    }
    public void OnEndDrag(PointerEventData eventData){
        image.color = new Color32(255,255,255,255);
        isDragging = false;
    }


    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("shelfBar")){
            isCollidedWithBar = true;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("shelfBar")){
            isCollidedWithBar = false;
        }
    }

}
