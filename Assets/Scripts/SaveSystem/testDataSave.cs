using UnityEngine;

public class testDataSave : MonoBehaviour, IDataPersistance
{
    public int tint = 5;

    void Start(){
        Debug.Log(tint);
    }

    public void LoadData(GameData data){
        tint = data.testNum;
    }
    public void SaveData(ref GameData data){
        data.testNum = tint;
    }

    void Update(){
        if(Input.GetKeyDown("g")){
            tint++;
        }
    }
}
