using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework.Constraints;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] string fileName;

    private GameData gameData;
    public static DataPersistanceManager instance {get; private set;}
    private FileDataHandler dataHandler;
    List<IDataPersistance> dataPersistanceObjects;

    private void Awake()
    {
        if(instance != null){
            Debug.LogError("There can't be more than one data persistance manager instance in the scene!");
        }   
        instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName);
        dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();   
    }

    public void NewGame(){
        gameData = new GameData();
    }

    public void LoadGame(){
        gameData = dataHandler.Load();
        if(gameData == null){
            Debug.LogError("No game was found to load!");
            NewGame();
        }

        foreach(IDataPersistance dataPersistance in dataPersistanceObjects){
            dataPersistance.LoadData(gameData);
        }

        Debug.Log("loaded test num with " + gameData.testNum);
    }

    public void SaveGame(){
        foreach(IDataPersistance dataPersistance in dataPersistanceObjects){
            dataPersistance.SaveData(ref gameData);
        }

        Debug.Log("saved test num with " + gameData.testNum);
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects(){
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID)
        .OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
