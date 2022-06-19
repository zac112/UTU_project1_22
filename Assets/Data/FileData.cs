using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileData
{
    private string directory = "";
    private string file = "";

    public FileData(string directory, string file){
        this.directory = directory;
        this.file = file;

    }

    // Load data from file 
    public GameData Load(){
        string path = Path.Combine(directory, file);
        GameData loadData = null;
        if (File.Exists(path)){
            try{
                string dataLoad = "";
                using(FileStream stream = new FileStream(path, FileMode.Open)){
                    using(StreamReader read = new StreamReader(stream)){
                        dataLoad = read.ReadToEnd();
                    }
                }
                loadData = JsonUtility.FromJson<GameData>(dataLoad);
            }
            catch (Exception e){
                Debug.Log("Couldn't load:"+e);
            }
        }
        return loadData;
    }

    // Save data to file 
    public void Save(GameData data){
        string path = Path.Combine(directory, file);
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string storeData = JsonUtility.ToJson(data, true);

            using(FileStream stream = new FileStream(path, FileMode.Create)){
                using(StreamWriter write = new StreamWriter(stream)){
                    write.Write(storeData);
                }
            }
        }
        catch (Exception e){
            Debug.Log("Couldn't save:"+e);
        }
    }
}
