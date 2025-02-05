using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    public SpriteRenderer interactSprite;
    private bool isPlayerInRange;
    private IInteractable interactableObject;

    void Start()
    {
        if(interactSprite != null)
            interactSprite.gameObject.SetActive(false);
    }

    void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.E))
            interactableObject?.Interact();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player>() != null)
        {
            interactableObject = GetComponent<IInteractable>();
            if(interactableObject != null)
            {
                isPlayerInRange = true;
                interactSprite.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        if(other.GetComponent<Player>() != null)
        {
            isPlayerInRange = false;
            interactSprite.gameObject.SetActive(false);
        }

    }
}
