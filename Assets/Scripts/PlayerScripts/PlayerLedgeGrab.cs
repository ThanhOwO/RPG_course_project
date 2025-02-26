using UnityEngine;

public class PlayerLedgeGrab : MonoBehaviour
{
    public bool isLedgeDetected { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ledge"))
        {
            isLedgeDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ledge"))
        {
            isLedgeDetected = false;
        }
    }
}
