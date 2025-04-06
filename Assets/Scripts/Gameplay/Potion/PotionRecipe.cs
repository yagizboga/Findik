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

    private int currentIngredientIndex = 0;
    private int potionQuality;
    private int initialQuality = 5;
    private bool[] ingredientLocked;
    public TMP_Text potionQualityText;

    private Color originalSpoonColor;
    private Color originalWarmColor;

    private Dictionary<PotionIngredientType, Color> originalTextColors = new Dictionary<PotionIngredientType, Color>();

    private void Start()
    {
        InitializeRequiredCounts();
        InitializeIngredientTextMap();
        ingredientLocked = new bool[requiredTypes.Length];
        UpdatePotionQualityUI();

        if (spoonText != null)
            originalSpoonColor = spoonText.color;

        if (warmText != null)
            originalWarmColor = warmText.color;

        potionQuality = initialQuality;
    }

    private void FixedUpdate()
    {
        //Debug.Log(potionQuality);
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
        /*if (!didSpoon)
            return false;
        if (!didWarm)
            return false;*/
        return true;
    }

    public void DropIngredient(GameObject ingredient)
    {
        PotionIngredient potionComponent = ingredient.GetComponent<PotionIngredient>();
        if (potionComponent == null) return;

        PotionIngredientType ingredientType = potionComponent.type;

        if (currentIngredientIndex >= requiredTypes.Length || ingredientLocked[currentIngredientIndex])
        {
            if (potionQuality > 0)
            {
                potionQuality--;
            }
            else
            {
                potionQuality = 0;
            }
            UpdatePotionQualityUI();
            ingredient.transform.localPosition = Vector3.zero;
        }
        else if (ingredientType == requiredTypes[currentIngredientIndex])
        {
            addedCounts[ingredientType]++;
            Destroy(ingredient);
            //spoon.ResetMix();
            UpdateIngredientTextUI(ingredientType);
            currentIngredientIndex++;
            CheckStatus();
        }
        else
        {
            int futureIndex = System.Array.IndexOf(requiredTypes, ingredientType);
            if (futureIndex > currentIngredientIndex)
            {
                int skippedCount = futureIndex - currentIngredientIndex; 
                potionQuality -= skippedCount;
                potionQuality = Mathf.Max(potionQuality, 0);
                UpdatePotionQualityUI();
                Debug.Log($"Wrong ingredient! Skipped {skippedCount} steps. PotionQuality: {potionQuality}");

                for (int i = currentIngredientIndex; i < futureIndex; i++)
                {
                    ingredientLocked[i] = true;
                    addedCounts[requiredTypes[i]]++;
                    UpdateIngredientTextToRed(requiredTypes[i]);
                }

                addedCounts[ingredientType]++;
                Destroy(ingredient);
                //spoon.ResetMix();
                UpdateIngredientTextUI(ingredientType);
                currentIngredientIndex = futureIndex + 1;
                CheckStatus();
            }
            else
            {
                if (ingredientType == PotionIngredientType.Spoon || ingredientType == PotionIngredientType.Warm)
                {
                    if (potionQuality > 0)
                    {
                        potionQuality--;
                    }
                    UpdatePotionQualityUI();
                    Debug.Log($"Wrong ingredient! PotionQuality: {potionQuality}");
                }
                else
                {
                    if (potionQuality > 0)
                    {
                        potionQuality--;
                    }
                    UpdatePotionQualityUI();
                    Debug.Log($"Wrong ingredient! PotionQuality: {potionQuality}");
                    Destroy(ingredient);
                }
            }
        }
    }


    private void UpdateIngredientTextToRed(PotionIngredientType ingredientType)
{
    if (ingredientTextMap.ContainsKey(ingredientType))
    {
        TMP_Text text = ingredientTextMap[ingredientType];
        text.color = new Color(1, 0, 0, 0.5f);
    }
}

    public void ResetRecipe()
    {
        InitializeRequiredCounts();
        isPotionReady = false;
        currentIngredientIndex = 0;

        for (int i = 0; i < ingredientLocked.Length; i++)
        {
            ingredientLocked[i] = false;
        }

        foreach (var kvp in ingredientTextMap)
        {
            PotionIngredientType type = kvp.Key;
            TMP_Text text = kvp.Value;

            if (originalTextColors.ContainsKey(type))
                text.color = originalTextColors[type];
        }

        if (spoonText != null)
            spoonText.color = originalSpoonColor;

        if (warmText != null)
            warmText.color = originalWarmColor;

        didSpoon = false;
        didWarm = false;

        potionQuality = initialQuality;
        UpdatePotionQualityUI();
    }

    public void ClaimPotion()
    {
        int missingCount = 0;

        foreach (var type in requiredCounts.Keys)
        {
            int required = requiredCounts[type];
            int added = addedCounts.ContainsKey(type) ? addedCounts[type] : 0;

            if (added < required)
            {
                missingCount += required - added;
            }
        }

        potionQuality -= missingCount;
        potionQuality = Mathf.Max(potionQuality, 0);

        Debug.Log($"Potion is claimed with quality: {potionQuality} (Missing {missingCount} ingredients)");
        potionQuality = initialQuality;
        ResetRecipe();
        UpdatePotionQualityUI();
    }

    public void SetDidSpoon()
    {
        if (currentIngredientIndex >= requiredTypes.Length || ingredientLocked[currentIngredientIndex])
        {
            if (potionQuality > 0)
            {
                potionQuality--;
            }
            else
            {
                potionQuality = 0;
            }
            Debug.Log($"Wrong ingredient! PotionQuality: {potionQuality}");
            UpdatePotionQualityUI();
        }
        else if (requiredTypes[currentIngredientIndex] == PotionIngredientType.Spoon)
        {
            addedCounts[PotionIngredientType.Spoon]++;
            UpdateIngredientTextUI(PotionIngredientType.Spoon);
            currentIngredientIndex++;
            CheckStatus();
        }
        else
        {
            if (potionQuality > 0)
            {
                potionQuality--;
            }
            else
            {
                potionQuality = 0;
            }
            Debug.Log($"Wrong ingredient! PotionQuality: {potionQuality}");
            UpdatePotionQualityUI();
        }
    }

    public void SetDidWarm()
    {
        if (currentIngredientIndex >= requiredTypes.Length || ingredientLocked[currentIngredientIndex])
        {
            if (potionQuality > 0)
            {
                potionQuality--;
            }
            else
            {
                potionQuality = 0;
            }
            Debug.Log($"Wrong ingredient! PotionQuality: {potionQuality}");
            UpdatePotionQualityUI();
        }
        else if (requiredTypes[currentIngredientIndex] == PotionIngredientType.Warm)
        {
            addedCounts[PotionIngredientType.Warm]++;
            UpdateIngredientTextUI(PotionIngredientType.Warm);
            currentIngredientIndex++;
            CheckStatus();
        }
        else
        {
            if (potionQuality > 0)
            {
                potionQuality--;
            }
            else
            {
                potionQuality = 0;
            }
            Debug.Log($"Wrong ingredient! PotionQuality: {potionQuality}");
            UpdatePotionQualityUI();
        }
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
        originalTextColors.Clear(); 

        for (int i = 0; i < requiredTypes.Length && i < ingredientTexts.Length; i++)
        {
            ingredientTextMap[requiredTypes[i]] = ingredientTexts[i];
            originalTextColors[requiredTypes[i]] = ingredientTexts[i].color; 
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

    /*private void UpdateSpoonTextUI()
    {
        if (spoonText != null && didSpoon)
        {
            Color color = spoonText.color;
            color.a = 0.5f;
            spoonText.color = color;
        }
    }*/

    /*private void UpdateWarmTextUI()
    {
        if (warmText != null && didWarm)
        {
            Color color = warmText.color;
            color.a = 0.5f;
            warmText.color = color;
        }
    }*/

    private void UpdatePotionQualityUI()
    {
        potionQuality = Mathf.Max(potionQuality, 0);
        if (potionQualityText != null)
        {
            potionQualityText.text = $"Potion Quality: {potionQuality}";
        }
    }
}
