using TMPro;
using UnityEngine;

public class eventNpcMono : MonoBehaviour
{
    string DialogueId;
    string anim;
    Sprite fullSprite;
    Vector3 targetPosition;
    float speed = 5f;
    [SerializeField] GameObject fullspriteobject;
    [SerializeField] GameObject mouth;
    [SerializeField] GameObject eyes;
    [SerializeField] GameObject npcObject;

    TextMeshPro DialogueText;
    Transform dialoguepos;

    public void Init(string DialogueId, string animation, Sprite dialoguesprite, Sprite mouthsprite, Sprite eyessprite, Vector3 targetPosition, Transform dialoguepos)
    {
        this.DialogueId = DialogueId;
        anim = animation;
        this.targetPosition = targetPosition;
        fullspriteobject.GetComponent<SpriteRenderer>().sprite = dialoguesprite;
        mouth.GetComponent<SpriteRenderer>().sprite = mouthsprite;
        eyes.GetComponent<SpriteRenderer>().sprite = eyessprite;
        fullspriteobject.GetComponent<SpriteRenderer>().sortingOrder = -1;
        mouth.GetComponent<SpriteRenderer>().sortingOrder = -1;
        eyes.GetComponent<SpriteRenderer>().sortingOrder = -1;
        this.dialoguepos = dialoguepos;
        Debug.Log(fullSprite);
        

    }

    void Awake()
    {
        Debug.Log(anim);
        npcObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        Debug.Log(fullSprite);
    }
    void Start()
    {
        Vector3 dir = (targetPosition - npcObject.transform.position).normalized;
        npcObject.GetComponent<Rigidbody2D>().linearVelocity = dir * speed;
        npcObject.GetComponent<Animator>().Play(anim);
        Debug.Log(fullSprite);
        DialogueText = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<TextMeshPro>();
    }
    void Update()
    {
        if (npcObject.GetComponent<Rigidbody2D>().linearVelocity.magnitude < 0.2f && npcObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime % 1f >= 0.9)
        {
            npcObject.GetComponent<Animator>().speed = 0;
            fullspriteobject.transform.position = dialoguepos.transform.position;
            fullspriteobject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            mouth.GetComponent<SpriteRenderer>().sortingOrder = 2;
            eyes.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        if ((targetPosition - npcObject.transform.position).magnitude < 0.2f)
        {
            npcObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector3(0,0,0);
        }
    }


}
