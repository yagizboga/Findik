using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Localization.Platform.Android;

public class FrogHeater : MonoBehaviour
{
    private int clickCount = 0;              // Kaç kere tıklanıldığını takip eder
    private Color originalColor;             // Kurbağanın orijinal rengi
    private SpriteRenderer spriteRenderer;   // Kurbağanın Sprite Renderer'ı

    public Animator frogAnimator;            // Animasyonu oynatmak için
    //public ParticleSystem warmEffect;        // Isınma efekti
    public PotionRecipe recipe;              // İksir tarifi (ısınma için)
    [SerializeField] Animator legsAnimator;
    bool isheating = false;

    void Start()
    {
        // Başlangıçta Sprite Renderer ve orijinal rengi al
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnMouseDown()
    {
        clickCount++;

        if (clickCount < 3)
        {
            DarkenColor();
        }
        if (clickCount == 1)
        {
            // Animasyonu oynat
            if (frogAnimator != null)
            {
                frogAnimator.SetTrigger("HeatUp");
                legsAnimator.SetTrigger("heatTrigger");

            }

            // İksir tarifi tamamlandı
            recipe.SetDidWarm();
            recipe.CheckStatus();

            // Konsola bilgi yaz
            Debug.Log("Kurbağa ısındı!");

            // Sistemi sıfırla
            ResetFrog();
        }
    }

    void DarkenColor()
    {
        // Kurbağanın rengini biraz koyulaştır
        Color darkerColor = spriteRenderer.color * 0.8f;
        darkerColor.a = 1f; // Şeffaflığı bozma
        spriteRenderer.color = darkerColor;
    }

    void ResetFrog()
    {
        clickCount = 0;
        spriteRenderer.color = originalColor;
        legsAnimator.SetTrigger("heatTrigger");
        
    }
}