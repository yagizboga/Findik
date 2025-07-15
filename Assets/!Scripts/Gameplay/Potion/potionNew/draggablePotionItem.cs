using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PotionItem
    {
        potion,
        jar,
        hanging,
        bellow,
        siphon,
        spoon

}
public class draggablePotionItem : MonoBehaviour
{
    [SerializeField] PotionItem type;
    [SerializeField] Animator animator;
    [SerializeField] Sprite draggedSprite;
    [SerializeField] GameObject draggedItemPrefab;
    [SerializeField] string animationName;
    [SerializeField] Animator legsAnimator;
    GameObject currentDragVisual;
    PotionManager potionManager;
    bool isDragging = false;

    public PotionItem GetPotionItemType()
    {
        return type;
    }
    void Awake()
    {
        potionManager = GameObject.FindGameObjectWithTag("potionmanager").GetComponent<PotionManager>();
    }
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
        else if (type == PotionItem.bellow)
        {
            animator.SetTrigger("HeatUp");
            legsAnimator.SetTrigger("heatTrigger");
            potionManager.ChangeHeadSpriteColor();
        }
        else if (type == PotionItem.siphon)
        {

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
            currentDragVisual.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 10f);
        }
    }

    void StartDrag()
    {
        Debug.Log("dragging...");
        if (type == PotionItem.hanging)
        {
            currentDragVisual = Instantiate(draggedItemPrefab, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f), Quaternion.identity);
            if (currentDragVisual.GetComponent<SpriteRenderer>() != null && draggedSprite != null)
            {
                currentDragVisual.GetComponent<SpriteRenderer>().sprite = draggedSprite;
                currentDragVisual.GetComponent<SpriteRenderer>().sortingOrder = 5;
            }


        }
        else if (type == PotionItem.jar)
        {
            currentDragVisual = Instantiate(draggedItemPrefab, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f), Quaternion.identity);
            if (currentDragVisual.GetComponent<SpriteRenderer>() != null && draggedSprite != null)
            {
                currentDragVisual.GetComponent<SpriteRenderer>().sprite = draggedSprite;
                currentDragVisual.GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
            else
            {
                Debug.Log("renderer is missing");
            }
        }
        else if (type == PotionItem.potion)
        {
            currentDragVisual = Instantiate(draggedItemPrefab, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f), Quaternion.identity);
            if (currentDragVisual.GetComponent<SpriteRenderer>() != null && draggedSprite != null)
            {
                currentDragVisual.GetComponent<SpriteRenderer>().sprite = draggedSprite;
                currentDragVisual.GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
            else
            {
                Debug.Log("renderer is missing");
            }
        }
    }
    void EndDrag()
    {
        if (type == PotionItem.potion && currentDragVisual.GetComponent<draggedItemScript>().GetIsInArea())
        {
            animator.Play(animationName);
            Destroy(currentDragVisual);
            currentDragVisual = null;
            potionManager.ChangeHeadSprite();
        }
        else if (type == PotionItem.hanging && currentDragVisual.GetComponent<draggedItemScript>().GetIsInArea())
        {
            currentDragVisual.GetComponent<Animator>().Play(animationName);
            currentDragVisual = null;
            potionManager.ChangeHeadSprite();
        }
        else if (type == PotionItem.jar && currentDragVisual.GetComponent<draggedItemScript>().GetIsInArea())
        {
            currentDragVisual.GetComponent<Animator>().Play(animationName);
            currentDragVisual = null;
            potionManager.ChangeHeadSprite();
        }
        if (!currentDragVisual.GetComponent<draggedItemScript>().GetIsInArea())
        {
            Destroy(currentDragVisual);
            currentDragVisual = null;
        }

    }




}
