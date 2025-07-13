using UnityEngine;

public class Oven : IngredientTypes
{
    private IngredientType requiredIngredient = IngredientType.Bread;
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
                ingredientHolder.SetIsOvenMatching(true);
            }

            else if (ingredient == null)
            {
                Debug.LogError("ingredient is NULL!");
            }
            else
            {
                ingredientHolder.SetIsOvenMatching(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            ingredientHolder.SetIsOvenMatching(false);
            ingredientHolder.SetHighlightedTrigger(null);
        }
    }

    public void SetIsDropped(bool set)
    {
        isDropped = set;
    }
}
