using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework.Constraints;
using UnityEngine.SceneManagement;

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
            Debug.Log("There can't be more than one data persistance manager instance in the scene! Destroying the new one.");
            Destroy(this.gameObject);
            return;
        }  
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void NewGame(){
        gameData = new GameData();
        SaveGame();
    }

    public void LoadGame(){
        gameData = dataHandler.Load();
        if(gameData == null){
            Debug.Log("No game was found to load!");
            return;
        }

        foreach(IDataPersistance dataPersistance in dataPersistanceObjects){
            dataPersistance.LoadData(gameData);
        }

        Debug.Log("loaded test num with " + gameData.testNum);
    }

    public void SaveGame(){
        if(gameData == null){
            Debug.Log("No game data to save!");
            return;
        }
        foreach(IDataPersistance dataPersistance in dataPersistanceObjects){
            dataPersistance.SaveData(ref gameData);
        }

        Debug.Log("saved test num with " + gameData.testNum);
        dataHandler.Save(gameData);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene){
        SaveGame();
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

    public bool HasGameData(){
        return gameData != null;
    }
}
