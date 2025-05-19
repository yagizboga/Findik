using UnityEngine;

public class CookCamRotation : MonoBehaviour
{
    [Header("Target Rotation Offset (Z axis in degrees)")]
    public float rotationOffsetZ = 180f; 

    public GameObject desksObj;
    public GameObject[] buttons; // assign here the VISUAL of "main obj" and the other "scripted" BUTTONS

    private Transform desksTransform;
    private float initialZRotation;
    private float targetZRotation;

    private bool isRotating = false;
    public float rotationSpeedLerp = 3f;
    public float rotationSpeedMT = 90f; 

    private void Start()
    {
        desksTransform = desksObj.transform;
    }

    private void Update()
    {
        RotateByLerp();
        //RotateByMoveTowardsAngle();
    }

    private void RotateByLerp()
    {
        if (isRotating)
        {
            float currentZ = desksTransform.eulerAngles.z;
            float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currentZ, targetZRotation));

            float dynamicLerpSpeed = rotationSpeedLerp + (1f / Mathf.Max(angleDiff, 0.01f)) * 10f;

            float newZ = Mathf.LerpAngle(currentZ, targetZRotation, Time.deltaTime * dynamicLerpSpeed);
            desksTransform.rotation = Quaternion.Euler(0f, 0f, newZ);

            if (angleDiff < 0.1f)
            {
                desksTransform.rotation = Quaternion.Euler(0f, 0f, targetZRotation);
                foreach (GameObject btn in buttons)
                {
                    btn.SetActive(true);
                }
                isRotating = false;
            }
        }
    }


    private void RotateByMoveTowardsAngle()
    {
        if (isRotating)
        {
            float currentZ = desksTransform.eulerAngles.z;
            float newZ = Mathf.MoveTowardsAngle(currentZ, targetZRotation, rotationSpeedMT * Time.deltaTime);
            desksTransform.rotation = Quaternion.Euler(0f, 0f, newZ);

            if (Mathf.Abs(Mathf.DeltaAngle(currentZ, targetZRotation)) < 0.1f)
            {
                desksTransform.rotation = Quaternion.Euler(0f, 0f, targetZRotation);
                foreach (GameObject btn in buttons)
                {
                    btn.SetActive(true);
                }
                isRotating = false;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!isRotating)
        {
            initialZRotation = desksTransform.eulerAngles.z;
            targetZRotation = initialZRotation + rotationOffsetZ;

            foreach (GameObject btn in buttons)
            {
                btn.SetActive(false);
            }

            isRotating = true;
        }
    }
}
