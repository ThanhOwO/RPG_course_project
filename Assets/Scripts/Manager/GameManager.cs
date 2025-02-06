using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    private Transform player;
    [SerializeField] private Savepoint[] savePoints;

    [Header("Last Death")]
    [SerializeField] private GameObject lastDeathPrefab;
    public int lastDeathAmount;
    [SerializeField] private float lastDeathX;
    [SerializeField] private float lastDeathY;
    private Savepoint lastActivatedSavepoint;

    private void Awake() {

        if (instance != null && instance != this) 
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start() 
    {
        savePoints = FindObjectsByType<Savepoint>(FindObjectsSortMode.None);
        player = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        LoadLastDeath(_data);
        LoadLastActivatedSavePoint(_data);
        LoadSavePoint(_data);

        if (!string.IsNullOrEmpty(_data.lastActivatedRoomID))
        {
            RoomManager.instance.UpdateConfiner(_data.lastActivatedRoomID);
        }
    }

    public void SetLastActivatedSavepoint(Savepoint savepoint)
    {
        lastActivatedSavepoint = savepoint; // Update the last activated savepoint
        if (savepoint != null)
        {
            GameData data = new GameData();
            SaveData(ref data);
            data.lastActivatedRoomID = savepoint.roomId;
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.playerLastestPosition = PlayerManager.instance.player.playerLastestPosition;
        _data.lastDeathAmount = lastDeathAmount;
        _data.lastDeathX = player.position.x;
        _data.lastDeathY = player.position.y;

        if(lastActivatedSavepoint != null)
        {
            _data.lastActivatedSavepointID = lastActivatedSavepoint.id;
            _data.lastActivatedRoomID = lastActivatedSavepoint.roomId;
        }

        _data.savePoints.Clear();

        foreach (Savepoint savepoint in savePoints)
        {
            _data.savePoints.Add(savepoint.id, savepoint.activateStatus);
        }
    }

    // Alex's save logic is find the closest savepoint
    // private Savepoint FindClosestSavePoint()
    // {
    //     float closestDistance = Mathf.Infinity;
    //     Savepoint closestSavepoint = null;

    //     foreach (Savepoint savepoint in savePoints)
    //     {
    //         float distanceToSavepoint = Vector2.Distance(player.position, savepoint.transform.position);
    //         if(distanceToSavepoint < closestDistance && savepoint.activateStatus == true)
    //         {
    //             closestDistance = distanceToSavepoint;
    //             closestSavepoint = savepoint;
    //         }
    //     }

    //     return closestSavepoint;
    // }

    private void LoadLastActivatedSavePoint(GameData _data)
    {
        if(string.IsNullOrEmpty(_data.lastActivatedSavepointID))
            return;

        foreach(Savepoint savepoint in savePoints)
        {
            if(_data.lastActivatedSavepointID == savepoint.id)
            {
                player.position = savepoint.transform.position;
                RoomManager.instance.MovePlayerToRoom(savepoint.parentRoom, player.position);
                break;
            }
        }
    }

    private void LoadLastDeath(GameData _data)
    {
        Player player = PlayerManager.instance.player;
        player.playerLastestPosition = _data.playerLastestPosition;
        if (player == null) 
            return;

        lastDeathAmount = _data.lastDeathAmount;
        Vector3 spawnPosition = _data.playerLastestPosition;;

        if(lastDeathAmount > 0)
        {
            GameObject newLastDeath = Instantiate(lastDeathPrefab, spawnPosition, Quaternion.identity);
            newLastDeath.GetComponent<LastDeath_Controller>().currency = lastDeathAmount;
        }

        lastDeathAmount = 0;
    }

    private void LoadSavePoint(GameData _data)
    {
        foreach(KeyValuePair<string, bool> pair in _data.savePoints)
        {
            foreach(Savepoint savepoint in savePoints)
            {
                if(savepoint.id == pair.Key && pair.Value == true)
                {
                    savepoint.ActivateSavepoint();
                }
            }
        }
    }

    // Alex's load logic is find the closest savepoint
    // private void LoadClosestSavePoint(GameData _data)
    // {
    //     if(_data.closestSavepointID == null)
    //         return;

    //     foreach(Savepoint savepoint in savePoints)
    //     {
    //         if(_data.closestSavepointID == savepoint.id)
    //         {
    //             player.position = savepoint.transform.position;
    //         }
    //     }
    // }

    public void PauseGame(bool _pause)
    {
        if(_pause)
            Time.timeScale = 0;
        else   
            Time.timeScale = 1;
    }

}
