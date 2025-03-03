using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    TradeManager tradeManager;
    float GAMETIME = 0;
    bool isSpawned = false;

    void Awake(){
        tradeManager = GetComponent<TradeManager>();
    }

    void Update(){
        GAMETIME+= Time.deltaTime;
//        Debug.Log(GAMETIME);
        if(GAMETIME >= 4 && !isSpawned){
            tradeManager.SpawnTrade();
            isSpawned = true;
            Debug.Log("SpawnTrade");
        }
    }
}

