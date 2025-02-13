using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Savepoint : MonoBehaviour, IInteractable
{
    private Animator anim;
    public bool activateStatus;
    public string id;
    private Light2D light2D;
    public Room parentRoom;
    public string roomId;
    private Player player;
    [SerializeField] private SavePoint_UI savePointUI;
    [SerializeField] private GameObject mapIcon;

    private void Start()
    {
        anim = GetComponent<Animator>();
        light2D = GetComponentInChildren<Light2D>();
        parentRoom = GetComponentInParent<Room>();
        player = PlayerManager.instance.player;

        if (!activateStatus)
        {
            light2D.enabled = false;
            mapIcon.SetActive(false);
        }

        if (parentRoom != null)
            roomId = parentRoom.roomID;
    }

    //Remember to generate savepoint-id for each savepoint
    [ContextMenu("Generate Savepoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void Interact()
    {
        if(savePointUI.isOpen)
            return;

        StartCoroutine(SaveAction()); 
    }

    public void ActivateSavepoint()
    {
        if(activateStatus == false)
            AudioManager.instance.PlaySFX(5, transform);
            
        activateStatus = true;
        anim.SetBool("Active", true);
        light2D.enabled = true;
        mapIcon.SetActive(true);
    }

    private void SaveGame()
    {
        Debug.Log("Game Saved at Savepoint: " + id);
        AudioManager.instance.PlaySFX(5, transform);
        GameManager.instance.SetLastActivatedSavepoint(this);
        GameManager.instance.RespawnEnemies();
        SaveManager.instance.SaveGame();
    }

    private void RestorePlayerHealth()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if(playerStats != null)
        {
            playerStats.IncreaseHealthBy(playerStats.GetMaxHealthValue());
        }
    }

    private IEnumerator SaveAction()
    {
        player.stateMachine.ChangeState(player.restState);

        yield return new WaitForSeconds(1f);

        if (!activateStatus)
        {
            ActivateSavepoint();
            GameManager.instance.SetLastActivatedSavepoint(this);
        }
        RestorePlayerHealth();
        SaveGame();
        if (savePointUI != null)
            savePointUI.OpenMenu();
    }
}
