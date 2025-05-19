using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientHolder : IngredientTypes
{
    public IngredientType highlightedIngredientType; 
    private IngredientType holdingIngredientType;
    private GameObject currentIngredient; 
    private bool canSpawn = false;
    public GameObject[] ingredientPrefabs;
    private GameObject highlightedTrigger;

    private bool isMatching = false;
    private bool canDrag = false;

    private bool isOvenMatching = false;
    private bool isGrillMatching = false;
    private bool isBinMatching = false;

    public GameObject[] meatCookPlace;
    public GameObject[] breadCookPlace;

    public GameObject cookableMeat;
    public GameObject cookableBread;

    public Transform ovenTransform;

    public IngredientBox breadSlot;
    public IngredientBox meatSlot;
    public IngredientBox marulSlot;
    public IngredientBox tomatoSlot;

    public Bin bin;

    private void Update()
    {
        if (canDrag && currentIngredient != null)
        {
            Vector2 pointerPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(pointerPos.x, pointerPos.y, 10));
            currentIngredient.transform.position = worldPos;
        }
    }

    public void SetCanSpawn(bool canSpawnStatus, IngredientType ingredientType, float materialAmount)
    {
        if (materialAmount <= 0 && canSpawnStatus == true)
            return;
        canSpawn = canSpawnStatus;
        if (canSpawn)
        {
            highlightedIngredientType = ingredientType;
        }
        else
        {
            highlightedIngredientType = IngredientType.Meat; 
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
    

    void SpawnIngredient()
    {
        int ingredientIndex = (int)highlightedIngredientType;
        holdingIngredientType = highlightedIngredientType;
        DecreaseIngredient(holdingIngredientType, 20); 
        if (ingredientIndex >= 0 && ingredientIndex < ingredientPrefabs.Length)
        {
            currentIngredient = Instantiate(ingredientPrefabs[ingredientIndex], Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10), Quaternion.identity);
            if(highlightedIngredientType == IngredientType.Meat)
            {
                Cookable cookable = currentIngredient.GetComponent<Cookable>();
                cookable.SetIsCooking(false);
            }
        }
    }

    public void DropIngredient(GameObject ingredient = null)
    {
        if (ingredient == null)
        {
            ingredient = currentIngredient;
        }
        
        if (isMatching)
        {
            //Debug.Log("DROPPED after Match!");
            IngredientTrigger trigger = highlightedTrigger.GetComponent<IngredientTrigger>();
            trigger.UpdateColor();
            trigger.SetIsDropped(true);
            Destroy(ingredient);
            highlightedTrigger = null;
        }
        else if (isGrillMatching)
        {
            //Debug.Log("DROPPED after Grill Match!");
            isGrillMatching = false;
            Grill grill = highlightedTrigger.GetComponent<Grill>();
            grill.SetIsDropped(true);
            Destroy(ingredient);
            highlightedTrigger = null;
            isGrillMatching = false;
            ingredient = null;
            isMatching = false;

            for (int i = 0; i < meatCookPlace.Length; i++)
            {
                if(meatCookPlace[i].transform.childCount == 0)
                {
                    isGrillMatching = false;
                    Instantiate(cookableMeat, meatCookPlace[i].transform);
                    break;
                }
            }
        }
        else if (isOvenMatching)
        {
            //Debug.Log("DROPPED after Grill Match!");
            isOvenMatching = false;
            Animator breadAnimator = ingredient.GetComponent<Animator>();
            ingredient.transform.SetParent(ovenTransform);
            breadAnimator.SetBool("isDropped", true);
            //DropBread(ingredient);
        }
        else if (isBinMatching)
        {
            bin.BinAnimation(false);
            Destroy(ingredient);
        }
        else
        {
            if (ingredient == currentIngredient)
            {
                Destroy(ingredient);
                IncreaseIngredient(holdingIngredientType, 20);
            }
            else
            {
                ingredient.transform.localPosition = Vector3.zero;
                ingredient.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                //Debug.Log("ZEROED");
            }
        }
        holdingIngredientType = IngredientType.Meat;
    }

    public void SetIsMatching(bool match)
    {
        isMatching = match;
    }

    public void SetHighlightedTrigger(GameObject obj)
    {
        highlightedTrigger = obj;
    }

    public void SetIsGrillMatching(bool ismatch)
    {
        if(ismatch == true && canDrag)
            isGrillMatching = ismatch;
        else if (ismatch == false)
            isGrillMatching = ismatch;
    }

    public void SetIsOvenMatching(bool ismatch)
    {
        if (ismatch == true && canDrag)
            isOvenMatching = ismatch;
        else if (ismatch == false)
            isOvenMatching = ismatch;
    }
    public void SetIsBinMatching(bool ismatch)
    {
        if (ismatch == true)
            isBinMatching = ismatch;
        else if (ismatch == false)
            isBinMatching = ismatch;
    }

    public void DropBread(GameObject ingredient) 
    {
        Oven oven = ovenTransform.gameObject.GetComponent<Oven>();
        oven.SetIsDropped(true);
        Destroy(ingredient);
        highlightedTrigger = null;
        isOvenMatching = false;
        oven = null;
        isMatching = false;

        for (int i = 0; i < breadCookPlace.Length; i++)
        {
            if (breadCookPlace[i].transform.childCount == 0)
            {
                isOvenMatching = false;
                GameObject inst = Instantiate(cookableBread, breadCookPlace[i].transform);
                inst.transform.localPosition = Vector3.zero;
                break;
            }
        }
        for (int i = 0; i < breadCookPlace.Length; i++)
        {
            if (breadCookPlace[i].transform.childCount == 0)
            {
                isOvenMatching = false;
                GameObject inst = Instantiate(cookableBread, breadCookPlace[i].transform);
                inst.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }

    public void IncreaseIngredient(IngredientType type, int amount)
    {
        switch (type)
        {
            case IngredientType.Bread:
                breadSlot.IncreaseMaterial(amount);
                break;
            case IngredientType.Meat:
                meatSlot.IncreaseMaterial(amount);
                break;
            case IngredientType.Marul:
                marulSlot.IncreaseMaterial(amount);
                break;
            case IngredientType.Tomato:
                tomatoSlot.IncreaseMaterial(amount);
                break;
            default:
                Debug.LogWarning("IncreaseIngredient: Unknown IngredientType " + type);
                break;
        }
    }

    public void DecreaseIngredient(IngredientType type, int amount)
    {
        switch (type)
        {
            case IngredientType.Bread:
                breadSlot.DecreaseMaterial(amount);
                break;
            case IngredientType.Meat:
                meatSlot.DecreaseMaterial(amount);
                break;
            case IngredientType.Marul:
                marulSlot.DecreaseMaterial(amount);
                break;
            case IngredientType.Tomato:
                tomatoSlot.DecreaseMaterial(amount);
                break;
            default:
                Debug.LogWarning("DecreaseIngredient: Unknown IngredientType " + type);
                break;
        }
    }

    public bool GetCanDrag()
    {
        return canDrag;
    }   

}
