using UnityEngine;

[System.Serializable]
public class GameData
{
    public int testNum;
    public SerializableDictionary<string,bool> level;

    public GameData(){
        this.testNum = 0;
        level = new SerializableDictionary<string, bool>();
    }
}
