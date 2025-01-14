using Unity.VisualScripting;
using UnityEngine;

public class LastDeath_Controller : MonoBehaviour
{
    public int currency;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null)
        {
            PlayerManager.instance.currency += currency;
            Destroy(this.gameObject);
        }
    }
}
