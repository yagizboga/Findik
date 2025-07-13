using UnityEngine;

public class Bin : IngredientTypes
{
    private IngredientHolder ingredientHolder;
    private Animator animator;

    private void Start()
    {
        ingredientHolder = FindFirstObjectByType<IngredientHolder>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            ingredientHolder.SetHighlightedTrigger(gameObject);
            ingredientHolder.SetIsBinMatching(true);
            BinAnimation(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            ingredientHolder.SetIsBinMatching(false);
            ingredientHolder.SetHighlightedTrigger(null);
            BinAnimation(false);
        }
    }

    public void BinAnimation(bool hold)
    {
        animator.SetBool("isHolding", hold);
    }
}
