using UnityEngine;

public class Customer : MonoBehaviour
{
    CustomerScriptableObject customerData;
    Transform firstDestination = null;
    SpriteRenderer spriteRenderer;
    float speed = 2f;
    void Awake(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(customerData != null){
            spriteRenderer.sprite = customerData.GetSprite();
        }
    }
    public void SetCustomerData(CustomerScriptableObject customerScriptableObject){
        customerData = customerScriptableObject;
        spriteRenderer.sprite = customerData.GetSprite();
    }
    public void SetFirstDestination(Transform transform){
        firstDestination = transform;
    }

    void Update(){
        if(firstDestination != null && Mathf.Abs(new Vector2(transform.position.x,transform.position.y).magnitude - new Vector2(transform.position.x,transform.position.y).magnitude) < 0.1f){
            transform.position+= (firstDestination.position - transform.position).normalized * Time.deltaTime * speed;
        }
    }

}
