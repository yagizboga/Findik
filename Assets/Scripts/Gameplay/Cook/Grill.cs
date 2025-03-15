using UnityEngine;
using UnityEngine.InputSystem;

public class Grill : IngredientTypes
{
    private IngredientType requiredIngredient = IngredientType.Meat;
    private IngredientHolder ingredientHolder;
    private bool isDropped = false;

    private void Start()
    {
        ingredientHolder = FindFirstObjectByType<IngredientHolder>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            Ingredient ingredient = collision.GetComponent<Ingredient>();

            if (ingredient != null && ingredient.ingredientType == requiredIngredient)
            {
                ingredientHolder.SetHighlightedTrigger(gameObject);
                ingredientHolder.SetIsGrillMatching(true);
            }

            else if (ingredient == null)
            {
                Debug.LogError("ingredient is NULL!");
            }
            else
            {
                ingredientHolder.SetIsGrillMatching(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            ingredientHolder.SetIsGrillMatching(false);
            ingredientHolder.SetHighlightedTrigger(null);
        }
    }

    public void SetIsDropped(bool set)
    {
        isDropped = set;
    }
}
