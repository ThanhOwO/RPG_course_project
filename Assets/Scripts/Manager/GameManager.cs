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
        LoadClosestSavePoint(_data);
        LoadSavePoint(_data);
    }

    public void SaveData(ref GameData _data)
    {
        _data.lastDeathAmount = lastDeathAmount;
        _data.lastDeathX = player.position.x;
        _data.lastDeathY = player.position.y;

        if(FindClosestSavePoint() != null)
            _data.closestSavepointID = FindClosestSavePoint().id;

        _data.savePoints.Clear();

        foreach (Savepoint savepoint in savePoints)
        {
            _data.savePoints.Add(savepoint.id, savepoint.activateStatus);
        }
    }

    private Savepoint FindClosestSavePoint()
    {
        float closestDistance = Mathf.Infinity;
        Savepoint closestSavepoint = null;

        foreach (Savepoint savepoint in savePoints)
        {
            float distanceToSavepoint = Vector2.Distance(player.position, savepoint.transform.position);
            if(distanceToSavepoint < closestDistance && savepoint.activateStatus == true)
            {
                closestDistance = distanceToSavepoint;
                closestSavepoint = savepoint;
            }
        }

        return closestSavepoint;
    }

    private void LoadLastDeath(GameData _data)
    {
        lastDeathAmount = _data.lastDeathAmount;
        lastDeathX = _data.lastDeathX;
        lastDeathY = _data.lastDeathY;

        if(lastDeathAmount > 0)
        {
            GameObject newLastDeath = Instantiate(lastDeathPrefab, new Vector3(lastDeathX, lastDeathY), Quaternion.identity);
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

    private void LoadClosestSavePoint(GameData _data)
    {
        if(_data.closestSavepointID == null)
            return;

        foreach(Savepoint savepoint in savePoints)
        {
            if(_data.closestSavepointID == savepoint.id)
            {
                player.position = savepoint.transform.position;
            }
        }
    }

    public void PauseGame(bool _pause)
    {
        if(_pause)
            Time.timeScale = 0;
        else   
            Time.timeScale = 1;
    }

}
