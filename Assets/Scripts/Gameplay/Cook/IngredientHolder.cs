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

    private bool isOvenMatching = false;
    private bool isGrillMatching = false;

    public GameObject meatHolder;
    public GameObject[] meatCookPlace;
    //private bool[] isMeatPlaceEmpty;

    public GameObject cookableMeat;

    private void Start()
    {
       /* isMeatPlaceEmpty = new bool[meatHolder.transform.childCount];
        for(int i = 0; i < isMeatPlaceEmpty.Length; i++)
        {
            isMeatPlaceEmpty[i] = false;
        }*/
    }

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
        //Debug.Log(isGrillMatching);
    }

    void SpawnIngredient()
    {
        int ingredientIndex = (int)highlightedIngredient; 

        if (ingredientIndex >= 0 && ingredientIndex < ingredientPrefabs.Length)
        {
            currentIngredient = Instantiate(ingredientPrefabs[ingredientIndex], Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10), Quaternion.identity);
            if(highlightedIngredient == IngredientType.Meat)
            {
                Cookable cookable = currentIngredient.GetComponent<Cookable>();
                cookable.SetIsCooking(false);

                PlayerInput playerInput = currentIngredient.GetComponent<PlayerInput>();
                PlayerInput player = FindFirstObjectByType<PlayerInput>(); // Ana oyuncu giri�ini bul

                if (playerInput != null && player != null)
                {
                    // Mevcut kontrol �emas�n� al ve yeni nesneye aktar
                    string currentScheme = player.currentControlScheme;
                    playerInput.SwitchCurrentControlScheme(currentScheme, player.devices.ToArray());
                }
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
            grill.UpdateColor();
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
        else
        {
            if (ingredient == currentIngredient)
            {
                Destroy(ingredient);
            }
            else
            {
                ingredient.transform.localPosition = Vector3.zero;
                //Debug.Log("ZEROED");
            }
            
        }
        
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
}
