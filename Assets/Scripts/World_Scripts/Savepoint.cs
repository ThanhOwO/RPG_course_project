using UnityEngine;

public class Savepoint : MonoBehaviour
{
    private Animator anim;
    public bool activateStatus;
    public string id;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate Savepoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>() != null)
        {
            ActivateSavepoint();
        }
    }

    public void ActivateSavepoint()
    {
        if(activateStatus == false)
            AudioManager.instance.PlaySFX(5, transform);
            
        activateStatus = true;
        anim.SetBool("Active", true);
    }
}
