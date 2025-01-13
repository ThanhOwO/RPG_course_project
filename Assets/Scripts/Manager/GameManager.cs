using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    [SerializeField] private Savepoint[] savePoints;
    private void Awake() {

        if (instance != null && instance != this) 
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start() 
    {
        savePoints = FindObjectsByType<Savepoint>(FindObjectsSortMode.None);
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
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

        foreach(Savepoint savepoint in savePoints)
        {
            if(_data.closestSavepointID == savepoint.id)
            {
                PlayerManager.instance.player.transform.position = savepoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
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
            float distanceToSavepoint = Vector2.Distance(PlayerManager.instance.player.transform.position, savepoint.transform.position);
            if(distanceToSavepoint < closestDistance && savepoint.activateStatus == true)
            {
                closestDistance = distanceToSavepoint;
                closestSavepoint = savepoint;
            }
        }

        return closestSavepoint;
    }
}
