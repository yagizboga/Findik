using UnityEngine;

public class IngredientHolder : IngredientTypes
{
    public IngredientType highlightedIngredient; 
    public GameObject meatPrefab, breadPrefab, tomatoPrefab, marulPrefab; 
    private GameObject currentIngredient; 
    private bool canSpawn = false; 

    private bool isMatching = false;

    public void SetCanSpawn(bool canSpawnStatus, IngredientType ingredientType)
    {
        canSpawn = canSpawnStatus;
        if (canSpawn)
        {
            highlightedIngredient = ingredientType;
        }
        else
        {
            highlightedIngredient = IngredientType.Meat; 
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSpawn && currentIngredient == null)
        {
            SpawnIngredient();
        }

        if (Input.GetMouseButton(0) && currentIngredient != null)
        {
            currentIngredient.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        }

        if (Input.GetMouseButtonUp(0) && currentIngredient != null)
        {
            DropIngredient();
        }
    }

    void SpawnIngredient()
    {
        GameObject prefabToSpawn = null;

        switch (highlightedIngredient)
        {
            case IngredientType.Meat:
                prefabToSpawn = meatPrefab;
                break;
            case IngredientType.Bread:
                prefabToSpawn = breadPrefab;
                break;
            case IngredientType.Tomato:
                prefabToSpawn = tomatoPrefab;
                break;
            case IngredientType.Marul:
                prefabToSpawn = marulPrefab;
                break;
        }

        if (prefabToSpawn != null)
        {
            currentIngredient = Instantiate(prefabToSpawn, Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10), Quaternion.identity);
        }
    }

    void DropIngredient()
    {
        if (isMatching)
        {
            Debug.Log("DROPPED after Match!");
            Destroy(currentIngredient);
        }
        else
        {
            Destroy(currentIngredient);
        }
        currentIngredient = null;
        isMatching = false;
    }

    public void SetIsMatching(bool match)
    {
        isMatching = match;
    }
}
