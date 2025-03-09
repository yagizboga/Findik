using UnityEngine;

public class IngredientBox : IngredientTypes
{
    public IngredientType boxType; 
    private IngredientHolder ingredientHolder;

    private void Start()
    {
        ingredientHolder = FindFirstObjectByType<IngredientHolder>();
    }

    private void OnMouseEnter() // no controller support
    {
        ingredientHolder.SetCanSpawn(true, boxType);
    }

    private void OnMouseExit() // no controller support
    {
        ingredientHolder.SetCanSpawn(false, IngredientType.Meat); 
    }
}
