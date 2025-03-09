using UnityEngine;

public class Recipe : MonoBehaviour
{
    public GameObject notReady;
    public GameObject ready;
    private GameObject[] triggerObj;
    private IngredientTrigger[] triggerScript;
    public ParticleSystem readyParticle;


    private void Start()
    {
        UpdateRecipe();
        notReady.SetActive(true);
        ready.SetActive(false);
    }

    public void UpdateRecipe()
    {
        int childCount = notReady.transform.childCount;
        triggerObj = new GameObject[childCount];
        triggerScript = new IngredientTrigger[childCount];

        for (int i = 0; i < childCount; i++)
        {
            triggerObj[i] = notReady.transform.GetChild(i).gameObject;
            triggerScript[i] = triggerObj[i].GetComponent<IngredientTrigger>();
        }
    }

    public void CheckRecipeReady()
    {
        foreach (IngredientTrigger trigger in triggerScript)
        {
            if (!trigger.GetIsDropped())
            {
                return;
            }
        }
        readyParticle.Play();
        notReady.SetActive(false);
        ready.SetActive(true);
        //Debug.Log("READY");
    }
}
