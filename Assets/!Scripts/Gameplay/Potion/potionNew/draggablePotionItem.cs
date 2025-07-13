using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class draggablePotionItem : MonoBehaviour
{
    enum PotionItem
    {
        potion,
        jar,
        hanging,
        bellow,
        siphon,
        spoon

    }
    [SerializeField ]PotionItem type;
    [SerializeField] Animator animator;
    [SerializeField] Sprite draggedSprite;
    [SerializeField] GameObject draggedItemPrefab;
    [SerializeField] string animationName;
    GameObject currentDragVisual;
    bool isDragging = false;

    void OnMouseDown()
    {
        if (type == PotionItem.hanging)
        {
            isDragging = true;
            StartDrag();
        }
        else if (type == PotionItem.jar)
        {
            isDragging = true;
            StartDrag();
        }
        else if (type == PotionItem.potion)
        {
            isDragging = true;
            StartDrag();
        }

    }
    void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            EndDrag();
        }
    }

    void Update()
    {
        if (isDragging && currentDragVisual != null)
        {
            currentDragVisual.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y,10f);
        }
    }

    void StartDrag()
    {
        if (type == PotionItem.hanging)
        {
            currentDragVisual = Instantiate(draggedItemPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            currentDragVisual.GetComponent<SpriteRenderer>().sprite = draggedSprite;
            currentDragVisual.GetComponent<SpriteRenderer>().sortingOrder = 5;
            
        }
        else if (type == PotionItem.jar)
        {
            currentDragVisual = Instantiate(draggedItemPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            currentDragVisual.GetComponent<SpriteRenderer>().sprite = draggedSprite;
            currentDragVisual.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }
        else if (type == PotionItem.potion)
        {
            currentDragVisual = Instantiate(draggedItemPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            currentDragVisual.GetComponent<SpriteRenderer>().sprite = draggedSprite;
            currentDragVisual.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }   
    }
    void EndDrag()
    {
        if (type == PotionItem.potion && currentDragVisual.GetComponent<draggedItemScript>().GetIsInArea())
        {
            animator.Play(animationName);
        }
        else if (type == PotionItem.hanging && currentDragVisual.GetComponent<draggedItemScript>().GetIsInArea())
        {
            currentDragVisual.GetComponent<Animator>().Play(animationName);
        }
        if (currentDragVisual != null)
        {
            //Destroy(currentDragVisual);
            //currentDragVisual = null;
        }
        
    }
    

    
}
