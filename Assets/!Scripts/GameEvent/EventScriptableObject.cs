using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "GameEvent")]
public class EventScriptableObject : ScriptableObject
{
    public string eventName;
    public Sprite botPlateSprite;
    public Sprite topPlateSPrite;
    public Sprite npcSprite;
    public TextAsset dialogueXml;
    public string topDownAnimation;
}