using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] GameObject npcPrefab;
    [SerializeField] Transform npcspawnposition;
    [SerializeField] Transform npctargetposition;
    [SerializeField] Transform dialoguepos;
    string anim;
    Sprite sprite;
    Sprite eyes;
    Sprite mouth;
    string DialogueId;
    public enum npctype
    {
        orangeMan
    }
    public void SetNpc(npctype type)
    {
        if (type == npctype.orangeMan)
        {
            sprite = Resources.Load<Sprite>("Assets/NPCPACK1/NPC1/IMG_0933");
            DialogueId = "1.0";
            anim = "testnpcanim";
            eyes = Resources.Load<Sprite>("Assets/NPCPACK1/Eyes1/IMG_0928");
            mouth = Resources.Load<Sprite>("Assets/NPCPACK1/Mouths1/IMG_0920");
        }
    }
    public void NewNpc(npctype type)
    {
        npcPrefab = Instantiate(npcPrefab);
        SetNpc(type);
        npcPrefab.GetComponent<eventNpcMono>().Init(DialogueId,anim,sprite,mouth,eyes,npctargetposition.position,dialoguepos);
        npcPrefab.transform.position = new Vector3(npcspawnposition.position.x, npcspawnposition.position.y, npcspawnposition.position.z);
    }

    void Start()
    {
        NewNpc(npctype.orangeMan);
    } 
}
