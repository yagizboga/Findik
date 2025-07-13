using UnityEngine;

[ExecuteAlways] // Works in both play mode and edit mode
public class spoon : MonoBehaviour
{
    [Header("Shader References")]
    [Tooltip("The liquid material using the MagicalParticleLiquid shader")]
    public Material liquidMaterial;
    
    [Header("Particle Settings")]
    [Range(1, 100), Tooltip("Number of particles in the liquid")]
    public int particleCount = 30;
    [Range(0.001f, 0.1f), Tooltip("Size of individual particles")]
    public float particleSize = 0.02f;
    [Range(0f, 5f), Tooltip("Base movement speed of particles")]
    public float particleSpeed = 0.5f;
    [Range(1f, 10f), Tooltip("Brightness of particles")]
    public float particleGlow = 3f;
    
    [Header("Spoon Interaction")]
    [Range(0.05f, 1f), Tooltip("Radius of spoon's influence area")]
    public float effectRadius = 0.3f;
    [Range(0f, 1f), Tooltip("Intensity of distortion effects")]
    public float effectIntensity = 0.3f;
    [Range(0f, 2f), Tooltip("Strength of ripple effects")]
    public float rippleIntensity = 1f;
    [Range(0f, 1f), Tooltip("How much spoon affects particle speed")]
    public float spoonInfluence = 0.3f;
    
    private Camera mainCamera;
    private Vector3 lastPosition;
    private float lastSpoonSpeed;
    private Vector4 lastSpoonPos;

    void Start()
    {
        InitializeReferences();
        UpdateMaterialProperties();
    }

    void Update()
    {
        CalculateSpoonMovement();
        UpdateMaterialProperties();
    }

    void OnValidate()
    {
        // Update properties when values change in inspector
        UpdateMaterialProperties();
    }

    void InitializeReferences()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        lastPosition = transform.position;
    }

    void CalculateSpoonMovement()
    {
        if (Time.deltaTime > 0)
        {
            lastSpoonSpeed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
            lastPosition = transform.position;
        }
    }

    void UpdateMaterialProperties()
    {
        if (liquidMaterial == null)
        {
            Debug.LogWarning("Liquid material not assigned!", this);
            return;
        }

        if (mainCamera != null)
        {
            // Convert spoon position to viewport coordinates (0-1 range)
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
            lastSpoonPos = new Vector4(viewportPos.x, viewportPos.y, 0, 0);
        }

        // Update all shader properties in one pass
        liquidMaterial.SetVector("_SpoonPos", lastSpoonPos);
        liquidMaterial.SetInt("_ParticleCount", particleCount);
        liquidMaterial.SetFloat("_ParticleSize", particleSize);
        
        // Apply damped speed based on spoon movement
        float dampedSpeed = particleSpeed * (1 + (lastSpoonSpeed * 0.1f * spoonInfluence));
        liquidMaterial.SetFloat("_ParticleSpeed", dampedSpeed);
        
        liquidMaterial.SetFloat("_ParticleGlow", particleGlow);
        liquidMaterial.SetFloat("_Radius", effectRadius);
        liquidMaterial.SetFloat("_DistortStrength", effectIntensity);
        liquidMaterial.SetFloat("_RippleIntensity", rippleIntensity);
    }

    void OnDrawGizmosSelected()
    {
        if (mainCamera != null && effectRadius > 0)
        {
            Gizmos.color = Color.cyan;
            float worldRadius = mainCamera.orthographicSize * effectRadius;
            Gizmos.DrawWireSphere(transform.position, worldRadius);
        }
    }

    void OnDisable()
    {
        // Reset shader properties when disabled
        if (liquidMaterial != null)
        {
            liquidMaterial.SetVector("_SpoonPos", new Vector4(0.5f, 0.5f, 0, 0));
        }
    }
}