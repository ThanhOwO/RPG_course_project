using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Savepoint : MonoBehaviour
{
    private Animator anim;
    public bool activateStatus;
    public string id;
    private Light2D light2D;

    private void Start()
    {
        anim = GetComponent<Animator>();
        light2D = GetComponentInChildren<Light2D>();
    }

    private void Update()
    {
        if(activateStatus == false)
        {
            light2D.enabled = false;
        }
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
        light2D.enabled = true;
    }
}
