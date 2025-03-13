using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "Item")]
public class ItemScriptableObject : ScriptableObject
{

    readonly string _id = System.Guid.NewGuid().ToString();
    [SerializeField] string _name;
    [SerializeField] Sprite _image;
    [SerializeField] shelfItemType type;
    public int [,] values;
    public Vector2Int size;

    public void Initialize(shelfItemType type){
        this.type = type;
        if(type == shelfItemType.book){
            values = new int [2,2] {{1,1},{1,1}};
            size = new Vector2Int(2,2);
            _image = Resources.Load<Sprite>("testItem");
        }
        else if(type == shelfItemType.key){
            values = new int [3,2] {{0,1},{0,1},{1,1}};
            size = new Vector2Int(3,2);
            _image = Resources.Load<Sprite>("testItem");

        }
    }



    public enum shelfItemType{
        book,
        key
    }
    public Sprite GetSprite(){
        return _image;
    }

    
    
}
