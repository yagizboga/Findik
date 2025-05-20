using UnityEngine;
[CreateAssetMenu(fileName = "ScriptableObject", menuName = "seed")]
public class SeedScriptableObject : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] Sprite image;
    [SerializeField] seedType type;

    public enum seedType{
        seed1,
        seed2
    }

    public void Initialize(seedType type){
        this.type = type;

        if(type == seedType.seed1){
            
        }
        else if(type == seedType.seed2){

        }
    }
    
}
