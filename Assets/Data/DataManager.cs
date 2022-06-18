using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataManager : MonoBehaviour
{

    [SerializeField] private string file;

    private GameData gameData;
    private List<IDataManager> dataManagers;
    private FileData dataFromFiles;

    public static DataManager instance { get; private set; }

    private void Awake() 
    {
        // Checking that there are not multiple datamanager instances
        if (instance != null) 
        { 
            Destroy(gameObject);
            return;        
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() 
    {
        this.dataFromFiles = new FileData(Application.persistentDataPath, file);
        this.dataManagers = FindManagers();
        LoadGame();
    }

    public void NewGame() 
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // Here we check if the data file is null
        this.gameData = dataFromFiles.Load();
        
        // if no data null --> set data to default GameData values and start a new game
        if (this.gameData == null) 
        {
            NewGame();
        }

        // Loaddata to every script that is linked by interface
        foreach (IDataManager dataManager in dataManagers) 
        {
            dataManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataManager dataManager in dataManagers) 
        {
            dataManager.SaveData(gameData);
        }
        dataFromFiles.Save(gameData);
    }

    // Exiting the game will result in saving the game
    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    // This function will return all the scripts that are linked by the interface IDataManager
    private List<IDataManager> FindManagers() 
    {
        IEnumerable<IDataManager> dataManagers = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataManager>();

        return new List<IDataManager>(dataManagers);
    }
}

