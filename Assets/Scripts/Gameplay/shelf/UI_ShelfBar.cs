using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UI_ShelfBar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    Image image;
    bool isCollidedShelfBar = false;

    void Awake(){
        rectTransform = gameObject.GetComponent<RectTransform>();
        image = gameObject.GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData){
        image.color = new Color32(255,255,255,170);
    }
    public void OnDrag(PointerEventData eventData){
        //if(!isCollidedShelfBar){
            rectTransform.anchoredPosition += new Vector2(0,eventData.delta.y);
        //}
        
    }
    public void OnEndDrag(PointerEventData eventData){
        image.color = new Color32(255,255,255,255);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collided withslehf bar");
        if(other.gameObject.CompareTag("shelfBar")){
            isCollidedShelfBar = true;
            


        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("shelfBar")){
            isCollidedShelfBar = false;
            Debug.Log("exited withslehf bar");
        }
    }
}
