using UnityEngine;

public class customTimeController : MonoBehaviour
{
    public Material liquidMaterial;
    public float timeScale = 1f;
    
    void Update()
    {
        if (liquidMaterial != null)
        {
            liquidMaterial.SetFloat("_TimeVal", Time.time * timeScale);
        }
    }
}