using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Teleport_UI : MonoBehaviour
{
    [SerializeField] private GameObject savePointButton;
    [SerializeField] private Transform savePointParent;

    private void OnEnable() 
    {
        PopulateSavepointList();
    }

    private void PopulateSavepointList()
    {
        foreach (Transform child in savePointParent)
        {
            Destroy(child.gameObject);
        }

        List<Savepoint> activeSavePoints = GetActiveSavePoints();

        foreach (Savepoint savePoint in activeSavePoints)
        {
            CreateSavePointButton(savePoint);
        }
    }

    private void CreateSavePointButton(Savepoint savePoint)
    {
        GameObject buttonObj = Instantiate(savePointButton, savePointParent);
        TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
        Button button = buttonObj.GetComponent<Button>();

        buttonText.text = $"{savePoint.parentRoom.roomID}";

        button.onClick.AddListener(() => SelectSavePoint(savePoint));
    }

    private void SelectSavePoint(Savepoint savePoint)
    {
        Debug.Log($"Selected Save Point: Room {savePoint.parentRoom.roomID}");
    }

    private List<Savepoint> GetActiveSavePoints()
    {
        List<Savepoint> activePoints = new List<Savepoint>();
        Savepoint[] allSavePoints = FindObjectsByType<Savepoint>(FindObjectsSortMode.None);

        foreach (Savepoint sp in allSavePoints)
        {
            if (sp.activateStatus)
            {
                activePoints.Add(sp);
            }
        }

        return activePoints;
    }
}
