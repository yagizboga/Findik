using UnityEngine;

public class CookCamMovement : MonoBehaviour
{
    [Header("Target Offset (X and Y movement from current position)")]
    public Vector2 movementOffset = new Vector2(0f, 13f);

    public GameObject cameraObj;
    public GameObject[] buttons; // assign here the VISUAL of "main obj" and the other "scripted" BUTTONS

    private Transform camTransform;
    private Vector3 initialCamPosition;
    private Vector3 targetCamPosition;

    private bool isMoving = false;
    private float moveSpeedLerp = 5f;
    public float moveSpeedMT = 20f;


    private void Start()
    {
        camTransform = cameraObj.transform;
    }

    private void Update()
    {
        MoveByLerp();
        //MoveByMoveTowards();
    }

    private void MoveByLerp()
    {
        if (isMoving)
        {
            camTransform.position = Vector3.Lerp(camTransform.position, targetCamPosition, Time.deltaTime * moveSpeedLerp);

            if (Vector3.Distance(camTransform.position, targetCamPosition) < 0.025f)
            {
                camTransform.position = targetCamPosition;

                foreach (GameObject btn in buttons)
                {
                    btn.SetActive(true);
                }
                isMoving = false;
            }
        }
    }

    private void MoveByMoveTowards()
    {
        if (isMoving)
        {
            camTransform.position = Vector3.MoveTowards(
                camTransform.position,
                targetCamPosition,
                moveSpeedMT * Time.deltaTime
            );

            if (Vector3.Distance(camTransform.position, targetCamPosition) < 0.001f)
            {
                camTransform.position = targetCamPosition;
                
                foreach (GameObject btn in buttons)
                {
                    btn.SetActive(true);
                }
                isMoving = false;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!isMoving)
        {
            initialCamPosition = camTransform.position;
            targetCamPosition = initialCamPosition + new Vector3(movementOffset.x, movementOffset.y, 0f);

            foreach (GameObject btn in buttons)
            {
                btn.SetActive(false);
            }
            isMoving = true;
        }
    }
}
