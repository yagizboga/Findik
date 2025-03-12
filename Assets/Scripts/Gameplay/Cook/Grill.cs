using UnityEngine;
using UnityEngine.InputSystem;

public class Grill : IngredientTypes
{
    private IngredientType requiredIngredient = IngredientType.Meat;
    private IngredientHolder ingredientHolder;
    private bool isDropped = false;

    //private Recipe recipe;
    private void Start()
    {
        ingredientHolder = FindFirstObjectByType<IngredientHolder>();
        //recipe = transform.parent.parent.GetComponent<Recipe>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            Ingredient ingredient = collision.GetComponent<Ingredient>();

            if (ingredient != null && ingredient.ingredientType == requiredIngredient)
            {
                //Debug.Log("Match: " + requiredIngredient);
                //ingredientHolder.SetIsMatching(true);
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
            //ingredientHolder.SetIsMatching(false);
            ingredientHolder.SetIsGrillMatching(false);
            ingredientHolder.SetHighlightedTrigger(null);
        }
    }

    public void UpdateColor()
    {
        //sprite.color = targetColor;
        //Debug.Log("UPDATED COLOR!");
    }

    public void SetIsDropped(bool set)
    {
        isDropped = set;
        //recipe.CheckRecipeReady();
    }

    public bool GetIsDropped()
    {
        return isDropped;
    }

    public void ResetRecipe()
    {
        //SetIsDropped(false);
        isDropped = false;
        //sprite.color = initialColor;
    }
}
