using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Teleport_UI : MonoBehaviour
{
    [SerializeField] private GameObject savePointButtonPrefab;
    [SerializeField] private Transform savePointParent;
    [SerializeField] private TeleportCameraController teleportMap;
    private SavePoint_UI savePointUI;
    private List<Savepoint> activeSavePoints = new List<Savepoint>();
    private List<Button> savePointButtons = new List<Button>();
    private int selectedIndex = 0;

    private void OnEnable() 
    {
        savePointUI = GetComponentInParent<SavePoint_UI>();
        PopulateSavepointList();

        if (activeSavePoints.Count > 0)
        {
            selectedIndex = 0;
            HighlightButton(selectedIndex);
            MoveMiniMapToSavePoint(selectedIndex);
        }
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;
        
        //Navigation buttons by keyboard
        if(savePointButtons.Count == 0) return;
        Navigate();
    }

    private void Navigate()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
            selectedIndex = (selectedIndex - 1 + savePointButtons.Count) % savePointButtons.Count;
        else if(Input.GetKeyDown(KeyCode.DownArrow))
            selectedIndex = (selectedIndex + 1) % savePointButtons.Count;
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            savePointButtons[selectedIndex].onClick.Invoke();
            return;
        }

        HighlightButton(selectedIndex);
        MoveMiniMapToSavePoint(selectedIndex);
    }

    private void PopulateSavepointList()
    {
        savePointButtons.Clear();
        activeSavePoints.Clear();

        foreach (Transform child in savePointParent)
            Destroy(child.gameObject);

        foreach (Savepoint savePoint in FindObjectsByType<Savepoint>(FindObjectsSortMode.None))
        {
            if(savePoint.activateStatus)
            {
                activeSavePoints.Add(savePoint);
                CreateSavePointButton(savePoint);
            }
        }
    }

    private void CreateSavePointButton(Savepoint savePoint)
    {
        GameObject buttonObj = Instantiate(savePointButtonPrefab, savePointParent);
        TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
        Button button = buttonObj.GetComponent<Button>();

        buttonText.text = $"{savePoint.parentRoom.roomID}";

        button.onClick.AddListener(() => TeleportToSavePoint(savePoint));
        savePointButtons.Add(button);
    }

    private void TeleportToSavePoint(Savepoint savePoint)
    {
        RoomManager.instance.MovePlayerToRoom(savePoint.parentRoom, savePoint.transform.position);
        savePointUI.CloseAll();
    }

    private void HighlightButton(int index)
    {
        for (int i = 0; i < savePointButtons.Count; i++)
        {
            Image buttonImg = savePointButtons[i].GetComponent<Image>();
            if(buttonImg != null)
            {
                Color32 color = buttonImg.color;
                color.a = (i == index) ? (byte)255 : (byte)36;
                buttonImg.color = color;
            }
        }
    }
    
    private void MoveMiniMapToSavePoint(int index)
    {
        if (index >= 0 && index < activeSavePoints.Count)
            teleportMap.FocusOnSavePoint(activeSavePoints[index].transform.position);
    }

}
