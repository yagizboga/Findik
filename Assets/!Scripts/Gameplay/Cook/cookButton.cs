using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class cookButton : MonoBehaviour
{
    [SerializeField] Vector3 targetPoint;
    [SerializeField] Camera cam;
    float camSpeed = 5f;

    void GoToTarget()
    {
        cam.transform.position = new Vector3(cam.transform.position.x, Mathf.Lerp(cam.transform.position.y, targetPoint.y, cam.transform.position.z));
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GoToTarget();

        }
        
    }

}
