using UnityEngine;

public class draggedItemScript : MonoBehaviour
{
    bool isInArea = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("pourarea"))
        {
            isInArea = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("pourarea"))
        {
            isInArea = false;
        }
    }

    public bool GetIsInArea()
    {
        return isInArea;
    }
    public void DestroyThis(){ Destroy(gameObject); }
}
