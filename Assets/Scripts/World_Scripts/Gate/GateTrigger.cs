using System.Collections;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public GameObject gate;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.GetComponent<Player>())
        {
            triggered = true;
            TriggerGateFall();
        }
    }

    private void TriggerGateFall()
    {
        StartCoroutine(delayCloseGate());
    }

    private IEnumerator delayCloseGate()
    {
        yield return new WaitForSeconds(1f);
        Rigidbody2D gateRb = gate.GetComponent<Rigidbody2D>();

        if (gateRb != null)
        {
            gateRb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
