using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    [SerializeField] List<Sprite> headSprites;
    [SerializeField] SpriteRenderer headSpriteRenderer;

    byte greenvalue;
    byte bluevalue;

    public void ChangeHeadSprite()
    {
        headSpriteRenderer.sprite = headSprites[Random.Range(0, 5)];
    }
    public void ChangeHeadSpriteColor()
    {
        headSpriteRenderer.gameObject.GetComponent<Animator>().SetTrigger("heatTrigger");
    }
}
