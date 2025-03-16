using System.Collections;
using UnityEngine;

public class IngredientTrigger : IngredientTypes
{
    public IngredientType requiredIngredient;
    private IngredientHolder ingredientHolder;
    private SpriteRenderer sprite;
    private Color targetColor;
    private Color initialColor;
    private bool isDropped = false;

    private Recipe recipe;
    private void Start()
    {
        ingredientHolder = FindFirstObjectByType<IngredientHolder>();
        sprite = GetComponent<SpriteRenderer>();
        initialColor = sprite.color;
        targetColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        recipe = transform.parent.parent.GetComponent<Recipe>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            Ingredient ingredient = collision.GetComponent<Ingredient>();

            if (ingredient != null && ingredient.ingredientType == requiredIngredient && !isDropped)
            {
                if(requiredIngredient != IngredientType.Meat && requiredIngredient != IngredientType.Bread)
                {
                    ingredientHolder.SetIsMatching(true);
                    ingredientHolder.SetHighlightedTrigger(gameObject);
                }
                else if(requiredIngredient == IngredientType.Meat)
                {
                    Cookable cookable = collision.GetComponent<Cookable>();
                    if(cookable.isCooked == true)
                    {
                        ingredientHolder.SetIsMatching(true);
                        ingredientHolder.SetHighlightedTrigger(gameObject);
                    }
                    else
                    {
                        //Debug.Log("is not COOKED");
                    }
                }
                else if (requiredIngredient == IngredientType.Bread)
                {
                    Cookable cookable = collision.GetComponent<Cookable>();
                    if (cookable.isCooked == true)
                    {
                        ingredientHolder.SetIsMatching(true);
                        ingredientHolder.SetHighlightedTrigger(gameObject);
                    }
                    else
                    {
                        //Debug.Log("is not COOKED");
                    }
                }

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            ingredientHolder.SetIsMatching(false);
            ingredientHolder.SetHighlightedTrigger(null);
        }
    }

    public void UpdateColor()
    {
        sprite.color = targetColor;
        //Debug.Log("UPDATED COLOR!");
    }

    public void SetIsDropped(bool set)
    {
        isDropped = set;
        recipe.CheckRecipeReady();
    }

    public bool GetIsDropped()
    {
        return isDropped;
    }

    public void ResetRecipe()
    {
        //SetIsDropped(false);
        isDropped = false;
        sprite.color = initialColor;
    }
}
