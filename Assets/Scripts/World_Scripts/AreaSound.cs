using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areSoundIndex;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>() != null)
            AudioManager.instance.PlaySFX(areSoundIndex, null);
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.GetComponent<Player>() != null)
            AudioManager.instance.StopFSXWithTime(areSoundIndex);
    }
}
