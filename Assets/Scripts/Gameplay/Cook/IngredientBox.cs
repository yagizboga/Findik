using UnityEngine;

public class IngredientBox : IngredientTypes
{
    public IngredientType boxType; 
    private IngredientHolder ingredientHolder;

    private void Start()
    {
        ingredientHolder = FindFirstObjectByType<IngredientHolder>();
    }

    private void OnMouseEnter()
    {
        ingredientHolder.SetCanSpawn(true, boxType);
    }

    private void OnMouseExit()
    {
        ingredientHolder.SetCanSpawn(false, IngredientType.Meat); 
    }
}
