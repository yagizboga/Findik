using Unity.VisualScripting;
using UnityEngine;

public class mousehoveranimscript : MonoBehaviour
{
    void OnMouseEnter()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (gameObject.GetComponent<draggablePotionItem>().GetPotionItemType() == PotionItem.hanging)
        {
            if (mousePos.x > transform.position.x)
            {
                GetComponent<Animator>().SetTrigger("asılıright");
            }
            else if (mousePos.x < transform.position.x)
            {
                GetComponent<Animator>().SetTrigger("asılıleft");
            }

        }
        else if (gameObject.GetComponent<draggablePotionItem>().GetPotionItemType() == PotionItem.jar)
        {
            gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("jaropen");
        }
    }

    void OnMouseExit()
    {
        if (gameObject.GetComponent<draggablePotionItem>().GetPotionItemType() == PotionItem.jar)
        {
            gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("jarclose");
        }
    }
}
