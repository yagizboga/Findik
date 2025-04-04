using UnityEngine;
using UnityEngine.InputSystem;

public class Bellows : MonoBehaviour
{
    private float moveSpeed = 125f;
    private float startRotation = 0f;
    private float endRotation = 15f;

    private bool canBellows = false;
    private bool isDragging = false;
    private float lastRotation;

    private float warmingAmount = 0f; 
    private float targetWarm = 100f; 
    private float lastWarmThreshold = 0f; 

    public ParticleSystem warmParticle;
    public PotionRecipe recipe;

    void Start()
    {
        lastRotation = startRotation;
    }

    void Update()
    {
        //Debug.Log(warmingAmount);
        if (isDragging)
        {
            float mouseYMovement = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime; // NO CONTROLLERR SUPPORT FOR NOW !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            float newZRotation = Mathf.Clamp(transform.eulerAngles.z - mouseYMovement, startRotation, endRotation);


            if (newZRotation > lastRotation)
            {
                warmingAmount += newZRotation - lastRotation;
                warmParticle.Play();

                if (warmingAmount - lastWarmThreshold >= 10f)
                {
                    //warmParticle.Play();
                    lastWarmThreshold += 10f; 
                }
            }
            else
            {
                warmParticle.Stop();
            }

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);
            lastRotation = newZRotation;

            if (warmingAmount >= targetWarm)
            {
                Debug.Log("Warm Done");
                recipe.SetDidWarm();
                recipe.CheckStatus();
                ResetWarm();
            }
        }
    }

    public void ResetWarm()
    {
        warmingAmount = 0f;
        lastWarmThreshold = 0f; 
    }

    private void OnMouseEnter()
    {
        canBellows = true;
    }

    private void OnMouseExit()
    {
        canBellows = false;
    }

    public void DragBellows(InputAction.CallbackContext context)
    {
        if (context.performed && canBellows)
        {
            isDragging = true;
            Cursor.visible = false;
        }
        else if (context.canceled)
        {
            isDragging = false;
            Cursor.visible = true;
            warmParticle.Stop();
        }
    }
}
