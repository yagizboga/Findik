using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientHolder : IngredientTypes
{
    public IngredientType highlightedIngredient; 
    private GameObject currentIngredient; 
    private bool canSpawn = false;
    public GameObject[] ingredientPrefabs;
    private GameObject highlightedTrigger;

    private bool isMatching = false;
    private bool canDrag = false;

    public void SetCanSpawn(bool canSpawnStatus, IngredientType ingredientType)
    {
        canSpawn = canSpawnStatus;
        if (canSpawn)
        {
            highlightedIngredient = ingredientType;
        }
        else
        {
            highlightedIngredient = IngredientType.Meat; 
        }
    }

    public void SpawnIngredientInput(InputAction.CallbackContext context)
    {
        if (context.performed && canSpawn && currentIngredient == null)
        {
            SpawnIngredient();
        }
    }
    public void DragIngredientInput(InputAction.CallbackContext context)
    {
        if (context.performed && currentIngredient != null)
        {
            canDrag = true;
        }
        else if (context.canceled)
        {
            canDrag = false;
        }
    }
    public void ReleaseIngredientInput(InputAction.CallbackContext context)
    {
        if (context.canceled && currentIngredient != null)
        {
            canDrag = false;
            DropIngredient();
        }
    }

    private void Update()
    {
        if (canDrag && currentIngredient != null)
        {
            Vector2 pointerPos = Pointer.current.position.ReadValue(); 
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, 10));
            currentIngredient.transform.position = worldPos;
        }

        /*if (Input.GetMouseButtonDown(0) && canSpawn && currentIngredient == null)
        {
            SpawnIngredient();
        }

        if (Input.GetMouseButton(0) && currentIngredient != null)
        {
            currentIngredient.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        }

        if (Input.GetMouseButtonUp(0) && currentIngredient != null)
        {
            DropIngredient();
        }*/
    }

    void SpawnIngredient()
    {
        int ingredientIndex = (int)highlightedIngredient; 

        if (ingredientIndex >= 0 && ingredientIndex < ingredientPrefabs.Length)
        {
            currentIngredient = Instantiate(ingredientPrefabs[ingredientIndex], Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10), Quaternion.identity);
        }
    }

    void DropIngredient()
    {
        if (isMatching)
        {
            //Debug.Log("DROPPED after Match!");
            IngredientTrigger trigger = highlightedTrigger.GetComponent<IngredientTrigger>();
            trigger.UpdateColor();
            trigger.SetIsDropped(true);
            Destroy(currentIngredient);
            highlightedTrigger = null;
        }
        else
        {
            Destroy(currentIngredient);
        }
        currentIngredient = null;
        isMatching = false;
    }

    public void SetIsMatching(bool match)
    {
        isMatching = match;
    }

    public void SetHighlightedTrigger(GameObject obj)
    {
        highlightedTrigger = obj;
    }
}
