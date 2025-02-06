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

    private void Start()
    {
        anim = GetComponent<Animator>();
        light2D = GetComponentInChildren<Light2D>();
        parentRoom = GetComponentInParent<Room>();

        if (!activateStatus)
            light2D.enabled = false;

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
        SaveGame();
        
        if (!activateStatus)
        {
            ActivateSavepoint();
            GameManager.instance.SetLastActivatedSavepoint(this);
        }
    }

    public void ActivateSavepoint()
    {
        if(activateStatus == false)
            AudioManager.instance.PlaySFX(5, transform);
            
        activateStatus = true;
        anim.SetBool("Active", true);
        light2D.enabled = true;
    }

    private void SaveGame()
    {
        Debug.Log("Game Saved at Savepoint: " + id);
        AudioManager.instance.PlaySFX(5, transform);
        GameManager.instance.SetLastActivatedSavepoint(this);
    }
}
