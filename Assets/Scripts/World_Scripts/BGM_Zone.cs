using UnityEngine;

public class BGM_Zone : MonoBehaviour
{
    [SerializeField] private int bgmIndex;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>() != null)
            AudioManager.instance.PlayBGM(bgmIndex);
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.GetComponent<Player>() != null)
            AudioManager.instance.StopBGM();
    }
}
