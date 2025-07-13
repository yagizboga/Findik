using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "Customer")]
public class CustomerScriptableObject : ScriptableObject
{
    [SerializeField] Sprite _sprite;
    [SerializeField] string _name;

    public Sprite GetSprite(){
        return _sprite;
    }
    
}
