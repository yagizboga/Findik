using UnityEngine;
using System;
using System.IO;
using System.Data.SqlTypes;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dirPath,string fileName){
        dataDirPath = dirPath;
        dataFileName = fileName;
    }

    public GameData Load(){
        string fullPath = Path.Combine(dataDirPath,dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath)){
            try{
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath,FileMode.Open)){
                    using(StreamReader reader = new StreamReader(stream)){
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch(Exception e){
                Debug.LogError("Error occured when loading data from file! " + fullPath+"\n"+e);
            }

            
        }
        return loadedData;
    }

    public void Save(GameData data){
        string fullPath = Path.Combine(dataDirPath,dataFileName);
        Debug.Log(fullPath);
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data,true);

            using (FileStream stream = new FileStream(fullPath,FileMode.Create)){
                using (StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e){
            Debug.LogError("Error occured when saving data to file! " + fullPath+"\n"+e);
        }
    }
}
