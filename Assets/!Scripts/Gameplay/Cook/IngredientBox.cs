using UnityEngine;

public class IngredientBox : IngredientTypes
{
    public IngredientType boxType;
    private IngredientHolder ingredientHolder;

    [Header("Material Sprites")]
    public Sprite emptySprite;
    public Sprite lowSprite;
    public Sprite mediumSprite;
    public Sprite fullSprite;

    [Header("Material Settings")]
    [Range(0, 100)]
    public int materialAmount = 0; 
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        ingredientHolder = FindFirstObjectByType<IngredientHolder>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void Update()
    {
        // INCREASE - DECREASE DEBUG
        if (Input.GetKeyDown(KeyCode.I))
        {
            IncreaseMaterial(10);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            DecreaseMaterial(10);
        }
    }

    private void OnMouseEnter() // no controller support
    {
        ingredientHolder.SetCanSpawn(true, boxType, materialAmount);
    }

    private void OnMouseExit() // no controller support
    {
        ingredientHolder.SetCanSpawn(false, IngredientType.Meat, materialAmount);
    }

    public void IncreaseMaterial(int amount)
    {
        materialAmount = Mathf.Clamp(materialAmount + amount, 0, 100);
        UpdateSprite();
    }

    public void DecreaseMaterial(int amount)
    {
        materialAmount = Mathf.Clamp(materialAmount - amount, 0, 100);
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (materialAmount == 0)
        {
            spriteRenderer.sprite = emptySprite;
        }
        else if (materialAmount > 0 && materialAmount <= 33)
        {
            spriteRenderer.sprite = lowSprite;
        }
        else if (materialAmount > 33 && materialAmount <= 66)
        {
            spriteRenderer.sprite = mediumSprite;
        }
        else // 67 to 100
        {
            spriteRenderer.sprite = fullSprite;
        }
    }
}
