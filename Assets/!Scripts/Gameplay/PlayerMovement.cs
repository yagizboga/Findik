using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    float playerSpeed = 5f;
    bool isMoving= false;
    Vector2 movement;
    Vector3 mousePos;
    Vector3 lookDir;
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>(); 
    }  
    void Update()
    {
        if (isMoving)
        {
            transform.position += new Vector3(movement.x, movement.y, 0) * Time.deltaTime * playerSpeed;
            animator.SetBool("isWalking", true);
        }
        else if (!isMoving)
        {
            animator.SetBool("isWalking", false);
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z =  transform.position.z;
        lookDir = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle -90);
    }   
    public void Movement(InputAction.CallbackContext ctx){
        if (ctx.performed)
        {
            isMoving = true;
            movement = ctx.ReadValue<Vector2>();
            
        }
        else if (ctx.canceled)
        {
            isMoving = false;
        }

    }
}
