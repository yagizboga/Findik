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

    public float detectionRadius = 2.0f;
    private LayerMask ingredientLayer;
    private bool isChecking = false;

    private void Start()
    {
        ingredientHolder = FindFirstObjectByType<IngredientHolder>();
        sprite = GetComponent<SpriteRenderer>();
        initialColor = sprite.color;
        targetColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        recipe = transform.parent.parent.GetComponent<Recipe>();

        ingredientLayer = LayerMask.GetMask("Ingredient");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ingredient") || isDropped)
        {
            /*ingredientHolder.SetIsMatching(false);
            ingredientHolder.SetHighlightedTrigger(null);*/
            return;
        }

        Ingredient ingredient = collision.GetComponent<Ingredient>();
        if (ingredient == null) return;

        if (ingredient.ingredientType != requiredIngredient) return;

        bool isValid = false;

        if (requiredIngredient == IngredientType.Meat || requiredIngredient == IngredientType.Bread)
        {
            Cookable cookable = collision.GetComponent<Cookable>();
            if (cookable != null && cookable.isCooked)
                isValid = true;
        }
        else
        {
            isValid = true;
        }

        if (isValid)
        {
            ingredientHolder.SetIsMatching(true);
            //Debug.Log("TRUEEE");
            ingredientHolder.SetHighlightedTrigger(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitIngredient();
    }

    private void Update()
    {
        //UpdateRayCastHit(); // not in use anymore
    }

    public void CheckIngredient(Collider2D hit)
    {
        if (!hit.CompareTag("Ingredient") || isDropped)
        {
            /*ingredientHolder.SetIsMatching(false);
            ingredientHolder.SetHighlightedTrigger(null);*/
            return;
        }

        Ingredient ingredient = hit.GetComponent<Ingredient>();
        if (ingredient == null) return;

        if (ingredient.ingredientType != requiredIngredient) return;

        bool isValid = false;

        if (requiredIngredient == IngredientType.Meat || requiredIngredient == IngredientType.Bread)
        {
            Cookable cookable = hit.GetComponent<Cookable>();
            if (cookable != null && cookable.isCooked)
                isValid = true;
        }
        else 
        {
            isValid = true;
        }

        if (isValid)
        {
            ingredientHolder.SetIsMatching(true);
            //Debug.Log("TRUEEE");
            ingredientHolder.SetHighlightedTrigger(gameObject);
        }
    }

    public void ExitIngredient()
    {
        ingredientHolder.SetIsMatching(false);
        ingredientHolder.SetHighlightedTrigger(null);
    }

    public void UpdateColor()
    {
        sprite.color = targetColor;
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
        isDropped = false;
        sprite.color = initialColor;
    }

    private void UpdateRayCastHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, ingredientLayer);

        if (hit != null && hit.CompareTag("Ingredient"))
        {
            if (!isChecking)
            {
                isChecking = true;
                //Debug.Log("Ingredient near");
                CheckIngredient(hit);
            }
        }
        else
        {
            if (isChecking)
            {
                isChecking = false;
                //Debug.Log("Ingredient far");
                ExitIngredient();
            }
        }
    }
}
