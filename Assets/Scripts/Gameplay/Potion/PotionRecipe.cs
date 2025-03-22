using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotionRecipe : PotionIngredientTypes
{
    public PotionIngredientType[] requiredTypes;
    private List<PotionIngredientType> addedIngredients = new List<PotionIngredientType>();

    private Dictionary<PotionIngredientType, int> requiredCounts = new Dictionary<PotionIngredientType, int>();
    private Dictionary<PotionIngredientType, int> addedCounts = new Dictionary<PotionIngredientType, int>();

    private bool isPotionReady = false;

    private GameObject currentIngredient;
    private PotionIngredient potion;
    private void Start()
    {
        InitializeRequiredCounts();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            currentIngredient = collision.gameObject;
            potion = collision.GetComponent<PotionIngredient>();
            potion.SetRecipe(gameObject);
            potion.SetIsMatching(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            currentIngredient = null;
            potion.SetIsMatching(false);
        }
    }

    private void InitializeRequiredCounts()
    {
        requiredCounts.Clear();
        addedCounts.Clear();

        foreach (PotionIngredientType type in requiredTypes)
        {
            if (requiredCounts.ContainsKey(type))
            {
                requiredCounts[type]++;
            }
            else
            {
                requiredCounts[type] = 1;
                addedCounts[type] = 0;
            }
        }
    }

    private bool IsRecipeComplete()
    {
        foreach (var type in requiredCounts.Keys)
        {
            if (addedCounts[type] < requiredCounts[type])
                return false;
        }
        return true;
    }

    public void DropIngredient(GameObject ingredient)
    {
        PotionIngredient potionComponent = ingredient.GetComponent<PotionIngredient>();
        if (potionComponent == null) return;

        PotionIngredientType ingredientType = potionComponent.type;

        if (requiredCounts.ContainsKey(ingredientType) && addedCounts[ingredientType] < requiredCounts[ingredientType])
        {
            addedCounts[ingredientType]++;
            Destroy(ingredient);

            if (IsRecipeComplete()) 
            {
                isPotionReady = true;
                Debug.Log("POTION IS READY!'");
               // ResetRecipe(); /
            }
        }
        else
        {
            ingredient.transform.localPosition = Vector3.zero;
        }
    }

    private void ResetRecipe()
    {
        InitializeRequiredCounts();
        isPotionReady = false;
    }
}
