using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveManager : MonoBehaviour
{
    private GameData gameData;
    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    public static SaveManager instance;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
    }

    private void Awake() 
    {

        if (instance != null && instance != this) 
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start() 
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if(this.gameData == null)
        {
            NewGame();
            Debug.Log("No save data found. Starting new game.");
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

        Debug.Log("Loaded currency: " + gameData.currency);
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        
        // Save game data to file
        dataHandler.Save(gameData);
        Debug.Log("Saving game data...");
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSaveData()
    {
        if(dataHandler.Load() != null)
            return true;
            
        return false;
    }
}
