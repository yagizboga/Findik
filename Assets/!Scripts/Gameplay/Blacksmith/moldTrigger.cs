using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class moldTrigger : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image moldFill;
    float fillSpeed = 5.0f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("meltedIronParticle"))
        {
            moldFill.fillAmount += fillSpeed * Time.deltaTime;
        }
        
    }
}
