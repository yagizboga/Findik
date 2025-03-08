using System.Collections;
using UnityEngine;

public class IngredientTrigger : IngredientTypes
{
    public IngredientType requiredIngredient;
    private IngredientHolder ingredientHolder;

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
                Debug.Log("Match: " + requiredIngredient);
                ingredientHolder.SetIsMatching(true);
            }

            else if(ingredient == null)
            {
                Debug.LogError("ingredient is NULL!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            ingredientHolder.SetIsMatching(false);
        }
    }
}
