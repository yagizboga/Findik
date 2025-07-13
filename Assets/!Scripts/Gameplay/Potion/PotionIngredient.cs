using UnityEngine;
using UnityEngine.InputSystem;

public class PotionIngredient : PotionIngredientTypes
{
    public PotionIngredientType type;
    private bool isDragging = false;
    public bool canDrag = false;
    public bool isDragBlocked = false;

    private float proximityThreshold = 0.25f;
    public static GameObject instance;

    private bool inRange = false;
    private bool inPourArea = false; // PourArea kontrolü

    private PotionRecipe potionRecipe;
    private bool isMatching = false;

    public Sprite fullVisual;
    public Sprite medVisual;
    public Sprite lowVisual;
    public Sprite emptyVisual;

    public int amount = 3;

    private Vector3 originalPosition; // İksirin başladığı yer

    private SpriteRenderer spriteRenderer;

    private Animator pourAreaAnimator; // PourArea'daki animator

    [SerializeField] private string dropAnimationName = "Pour"; // PourArea'daki animasyon ismi

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        originalPosition = transform.localPosition; // Başlangıç pozisyonunu kaydet
        UpdateVisual();
    }

    private void Update()
    {
        Vector3 pointerPosition = transform.position;

        // Mouse pozisyonu
        if (Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            pointerPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
        }

        // Mesafe kontrolü (drag başlamadan önce)
        float distance = Vector2.Distance(pointerPosition, transform.position);
        if (distance <= proximityThreshold)
        {
            canDrag = true;
            if (instance == null)
                instance = gameObject;
        }
        else if (distance > proximityThreshold && !inRange)
        {
            canDrag = false;
            if (instance == gameObject)
                instance = null;
        }
        else if (inRange)
        {
            canDrag = true;
            if (instance == null)
                instance = gameObject;
        }

        // Drag başlat
        if ((Mouse.current.leftButton.wasPressedThisFrame ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)) &&
            canDrag && !isDragBlocked && amount > 0)
        {
            isDragging = true;
        }

        // Drag bırak
        if ((Mouse.current.leftButton.wasReleasedThisFrame ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasReleasedThisFrame)) &&
            isDragging && !isDragBlocked)
        {
            isDragging = false;

            if (inPourArea && amount > 0 && pourAreaAnimator != null)
            {
                Debug.Log("PourArea içindeyken bırakıldı.");
                PlayPourAreaAnimation();
            }
            else
            {
                Debug.Log("PourArea dışında bırakıldı, geri dönüyor.");
                ReturnToOriginalPosition();
            }
        }

        // Eğer drag aktifse objeyi hareket ettir
        if (isDragging)
        {
            if (Gamepad.current != null && Gamepad.current.buttonSouth.isPressed)
            {
                // Analog stick ile hareket
                Vector2 stickInput = Gamepad.current.leftStick.ReadValue();
                transform.position += new Vector3(stickInput.x, stickInput.y, 0) * Time.deltaTime * 5f;
            }
            else
            {
                // Mouse imlecini takip et
                transform.position = pointerPosition;
            }
        }
    }

    private void PlayPourAreaAnimation()
    {
        // PourArea animasyonu oynat
        pourAreaAnimator.Play(dropAnimationName);

        // Miktarı azalt ve tarife ekle
        UseIngredient();
        if (potionRecipe != null)
        {
            potionRecipe.DropIngredient(gameObject);
        }

        // Objeyi eski yerine döndür
        ReturnToOriginalPosition();
    }

    private void ReturnToOriginalPosition()
    {
        transform.localPosition = originalPosition;
    }

    public void UseIngredient()
    {
        amount = Mathf.Max(0, amount - 1);
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (amount >= 3)
        {
            spriteRenderer.sprite = fullVisual;
        }
        else if (amount == 2)
        {
            spriteRenderer.sprite = medVisual;
        }
        else if (amount == 1)
        {
            spriteRenderer.sprite = lowVisual;
        }
        else // amount == 0
        {
            spriteRenderer.sprite = emptyVisual;
        }
    }

    public void SetRecipe(GameObject recipe)
    {
        potionRecipe = recipe.GetComponent<PotionRecipe>();
    }

    private void OnMouseEnter()
    {
        inRange = true;
        canDrag = true;
    }

    private void OnMouseExit()
    {
        inRange = false;
        canDrag = false;
    }

    public void SetIsMatching(bool match)
    {
        isMatching = match;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("pourarea"))
        {
            inPourArea = true;
            pourAreaAnimator = other.GetComponent<Animator>(); // PourArea'daki animatoru al
            Debug.Log("PourArea'ya girdim.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("pourarea"))
        {
            inPourArea = false;
            pourAreaAnimator = null; // Çıkınca animator referansını temizle
            Debug.Log("PourArea'dan çıktım.");
        }
    }
}