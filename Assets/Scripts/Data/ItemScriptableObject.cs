using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "Item")]
public class ItemScriptableObject : ScriptableObject
{

    readonly string _id = System.Guid.NewGuid().ToString();
    [SerializeField] string _name;
    [SerializeField] Sprite _image;
    [SerializeField] Type type;


    enum Type{
        shelf_Item,
        Garden_Item
    }
    
}
