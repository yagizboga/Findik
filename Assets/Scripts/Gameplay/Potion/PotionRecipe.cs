using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro; 
using UnityEngine.UI; 

public class PotionRecipe : PotionIngredientTypes
{
    public PotionIngredientType[] requiredTypes;
    private List<PotionIngredientType> addedIngredients = new List<PotionIngredientType>();

    private Dictionary<PotionIngredientType, int> requiredCounts = new Dictionary<PotionIngredientType, int>();
    private Dictionary<PotionIngredientType, int> addedCounts = new Dictionary<PotionIngredientType, int>();

    private bool isPotionReady = false;

    private GameObject currentIngredient;
    private PotionIngredient potion;

    private bool didSpoon = false;
    private bool didWarm = false;

    public Spoon spoon;

    public TMP_Text[] ingredientTexts; 
    private Dictionary<PotionIngredientType, TMP_Text> ingredientTextMap = new Dictionary<PotionIngredientType, TMP_Text>();

    public TMP_Text spoonText;
    public TMP_Text warmText;

    private void Start()
    {
        InitializeRequiredCounts();
        InitializeIngredientTextMap();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            currentIngredient = collision.gameObject;
            potion = collision.GetComponent<PotionIngredient>();
            potion.SetRecipe(gameObject);
            potion.SetIsMatching(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            currentIngredient = null;
            potion.SetIsMatching(false);
        }
    }

    private void InitializeRequiredCounts()
    {
        requiredCounts.Clear();
        addedCounts.Clear();

        foreach (PotionIngredientType type in requiredTypes)
        {
            if (requiredCounts.ContainsKey(type))
            {
                requiredCounts[type]++;
            }
            else
            {
                requiredCounts[type] = 1;
                addedCounts[type] = 0;
            }
        }
    }

    private bool IsRecipeComplete()
    {
        foreach (var type in requiredCounts.Keys)
        {
            if (addedCounts[type] < requiredCounts[type])
                return false;
        }
        if (!didSpoon)
            return false;
        if (!didWarm)
            return false;
        return true;
    }

    public void DropIngredient(GameObject ingredient)
    {
        PotionIngredient potionComponent = ingredient.GetComponent<PotionIngredient>();
        if (potionComponent == null) return;

        PotionIngredientType ingredientType = potionComponent.type;

        if (requiredCounts.ContainsKey(ingredientType) && addedCounts[ingredientType] < requiredCounts[ingredientType])
        {
            addedCounts[ingredientType]++;
            Destroy(ingredient);
            spoon.ResetMix();
            UpdateIngredientTextUI(ingredientType);
            CheckStatus();
        }
        else
        {
            ingredient.transform.localPosition = Vector3.zero;
        }
    }

    private void ResetRecipe()
    {
        InitializeRequiredCounts();
        isPotionReady = false;
    }

    public void SetDidSpoon(bool spoon)
    {
        didSpoon = spoon;
        foreach (var type in requiredCounts.Keys)
        {
            if (addedCounts[type] >= requiredCounts[type])
            {
                UpdateSpoonTextUI();
            }
        }
    }

    public void SetDidWarm(bool warm)
    {
        didWarm = warm;
        UpdateWarmTextUI();
    }

    public void CheckStatus()
    {
        if (IsRecipeComplete())
        {
            isPotionReady = true;
            Debug.Log("POTION IS READY!'");
            // ResetRecipe(); /
        }
    }

    private void InitializeIngredientTextMap()
    {
        ingredientTextMap.Clear();

        for (int i = 0; i < requiredTypes.Length && i < ingredientTexts.Length; i++)
        {
            ingredientTextMap[requiredTypes[i]] = ingredientTexts[i];
        }
    }
    private void UpdateIngredientTextUI(PotionIngredientType ingredientType)
    {
        if (ingredientTextMap.ContainsKey(ingredientType))
        {
            TMP_Text text = ingredientTextMap[ingredientType];
            Color color = text.color;
            color.a = 0.5f; 
            text.color = color;
        }
    }

    private void UpdateSpoonTextUI()
    {
        if (spoonText != null && didSpoon)
        {
            Color color = spoonText.color;
            color.a = 0.5f;
            spoonText.color = color;
        }
    }

    private void UpdateWarmTextUI()
    {
        if (warmText != null && didWarm)
        {
            Color color = warmText.color;
            color.a = 0.5f;
            warmText.color = color;
        }
    }

}
