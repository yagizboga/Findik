using System.Collections;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    public GameObject notReady;
    public GameObject ready;
    private GameObject[] triggerObj;
    private IngredientTrigger[] triggerScript;
    public ParticleSystem readyParticle;

    private bool isReady = false;
    public GameObject cookingTable;


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
        isReady = true;
        //Debug.Log("READY");


        /////////////////////////////////////////////////
        //// give the food to the hand of the dealer ////
        /////////////////////////////////////////////////
        

        //StartCoroutine(CloseTheTable()); // close the tab at the end
    }

    private IEnumerator CloseTheTable()
    {
        yield return new WaitForSeconds(1.5f);
        cookingTable.SetActive(false);
        //Debug.Log("Reseting");
        ResetRecipe();
    }

    public bool GetIsReady()
    {
        return isReady; 
    }

    public void SetIsReady(bool _ready)
    {
        isReady = _ready;
    }

    private void ResetRecipe()
    {
        notReady.SetActive(true);
        ready.SetActive(false);

        foreach (IngredientTrigger trigger in triggerScript)
        {
            trigger.ResetRecipe();
        }
    }
}
