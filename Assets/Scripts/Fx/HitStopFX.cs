using System.Collections;
using UnityEngine;

public class HitStopFX : MonoBehaviour
{
    public static HitStopFX instance; // Singleton để gọi từ bất kỳ đâu
    private bool isStopping = false; // Tránh bị spam hit stop

    private void Awake()
    {
        if (instance != null && instance != this) 
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public void StopTime(float duration, float slowTimeScale = 0.1f)
    {
        if (!isStopping)
        {
            StartCoroutine(HitStopCoroutine(duration, slowTimeScale));
        }
    }

    private IEnumerator HitStopCoroutine(float duration, float slowTimeScale)
    {
        isStopping = true;

        float originalTimeScale = Time.timeScale;
        Time.timeScale = slowTimeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = originalTimeScale;
        isStopping = false;
    }
}
