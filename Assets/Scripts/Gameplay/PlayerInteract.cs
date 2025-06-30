using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] GameObject SeedBasketUI;
    GameObject collided;
    bool[,] IsFarmable;
    SeedScriptableObject currentSeed;
    GameObject farm;
    bool isPlanting = false;
    bool isWatering = false;

    void Awake()
    {
        currentSeed = Resources.Load<SeedScriptableObject>("seedScriptableObject/seed1");
        farm = GameObject.FindGameObjectWithTag("farm");
        int width = (int)farm.GetComponent<farm>().GetFarmGrid().GetSize().x;
        int height = (int)farm.GetComponent<farm>().GetFarmGrid().GetSize().y;
        IsFarmable = new bool[width, height];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactText.gameObject.SetActive(true);
            collided = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactText.gameObject.SetActive(false);
            collided = null;
        }
    }

    public void E_Input(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (collided != null)
            {
                collided.gameObject.GetComponent<IInteractable>().Interact(gameObject);
            }
        }
    }

    public void LClick_Input(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var gridPos = farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(mousePos);
            int x = (int)gridPos.x;
            int y = (int)gridPos.y;

            if (x < 0 || y < 0 || x >= IsFarmable.GetLength(0) || y >= IsFarmable.GetLength(1))
                return;

            if (isPlanting)
            {
                if (IsFarmable[x, y] && !farm.GetComponent<farm>().isPlanted[x, y])
                {
                    farm.GetComponent<farm>().InitializePlant(x, y, currentSeed);
                    farm.GetComponent<farm>().isPlanted[x, y] = true;
                    farm.GetComponent<farm>().currentPhase[x, y] = 1;

                    farm.GetComponent<farm>().GetFarmSquares(x, y).GetComponent<SpriteRenderer>().color = new Color32(0, 150, 90, 255);

                    
                }
            }
            else if (isWatering)
            {
                if (IsFarmable[x, y])
                {
                    farm.GetComponent<farm>().isWatered[x, y] = true;

                    
                    farm.GetComponent<farm>().GetFarmSquares(x, y).GetComponent<SpriteRenderer>().color = new Color32(0, 75, 45, 255);

                    
                    if (farm.GetComponent<farm>().isPlanted[x, y] && !farm.GetComponent<farm>().isGrowing[x, y])
                    {
                        farm.GetComponent<farm>().StartCoroutine(farm.GetComponent<farm>().GrowPlant(x, y, 5, currentSeed));
                    }
                }
            }
        }
    }

    void Update()
    {
        Vector2 playerGridPos = farm.GetComponent<farm>().GetFarmGrid().GetWorldToGridPosition(transform.position);

        for (int x = 0; x < IsFarmable.GetLength(0); x++)
        {
            for (int y = 0; y < IsFarmable.GetLength(1); y++)
            {
                if (playerGridPos.x <= x + 1 && playerGridPos.x >= x - 1 && playerGridPos.y <= y + 1 && playerGridPos.y >= y - 1)
                {
                    IsFarmable[x, y] = true;
                }
                else
                {
                    IsFarmable[x, y] = false;
                }
            }
        }
    }

    public void SetIsPlanting(bool b)
    {
        if (isWatering) SetIsWatering(false);

        isPlanting = b;
        if (isPlanting)
        {
            interactText.text = "planting";
            SeedBasketUI.SetActive(true);
        }
        else
        {
            interactText.text = "interact";
            SeedBasketUI.SetActive(false);
        }
    }

    public void SetIsWatering(bool b)
    {
        if (isPlanting) SetIsPlanting(false);

        isWatering = b;
        if (isWatering)
        {
            interactText.text = "watering";
        }
        else
        {
            interactText.text = "interact";
        }
    }

    public bool GetIsPlanting()
    {
        return isPlanting;
    }

    public bool GetIsWatering()
    {
        return isWatering;
    }
}