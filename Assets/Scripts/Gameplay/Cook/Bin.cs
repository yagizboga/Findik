using UnityEngine;

public class Bin : IngredientTypes
{
    private IngredientHolder ingredientHolder;

    private void Start()
    {
        ingredientHolder = FindFirstObjectByType<IngredientHolder>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            ingredientHolder.SetHighlightedTrigger(gameObject);
            ingredientHolder.SetIsBinMatching(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            ingredientHolder.SetIsBinMatching(false);
            ingredientHolder.SetHighlightedTrigger(null);
        }
    }
}
