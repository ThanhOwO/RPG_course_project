using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    private GameData gameData;
    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    [SerializeField] private UI_SaveIcon saveIconUI;
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

        string path = Application.persistentDataPath;
        dataHandler = new FileDataHandler(path, fileName, encryptData);
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
        PlayerPrefs.DeleteKey("BossCutscenePlayed");
        PlayerPrefs.Save();
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

        //Debug.Log("Loaded currency: " + gameData.currency);
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
            saveManager.SaveData(ref gameData);

        StartCoroutine(ShowSaveIcon());

        // Save game data to file
        dataHandler.Save(gameData);
        Debug.Log("Saving game data...");
    }

    private void OnApplicationQuit() 
    {
        //SaveGame(); 
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSaveData()
    {
        if (dataHandler == null)
            return false;

        return dataHandler.Load() != null;
    }

        //Boss room 4 gate 
    public void SetGateOpened(bool opened)
    {
        gameData.isGateOpened = opened;
        SaveGame();
    }

    public bool IsGateOpened()
    {
        return gameData.isGateOpened;
    }

    private IEnumerator ShowSaveIcon()
    {
        saveIconUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        saveIconUI.gameObject.SetActive(false);
    }
}
