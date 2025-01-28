using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleFlickering_Controller : MonoBehaviour
{
    private Light2D candleLight;
    [SerializeField] private float minInnerRadius;
    [SerializeField] private float maxInnerRadius;
    [SerializeField] private float minOuterRadius;
    [SerializeField] private float maxOuterRadius;

    private float currentInnerRadius;
    private float currentOuterRadius;

    private void Start()
    {
        candleLight = GetComponent<Light2D>();
        StartCoroutine(LightFlicker());
    }

    private IEnumerator LightFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            float targetInnerRadius = Random.Range(minInnerRadius, maxInnerRadius);
            float targetOuterRadius = Random.Range(minOuterRadius, maxOuterRadius);
            currentInnerRadius = Mathf.Lerp(currentInnerRadius, targetInnerRadius, 0.5f); // Smoothly change the radius
            currentOuterRadius = Mathf.Lerp(currentOuterRadius, targetOuterRadius, 0.5f); 
            candleLight.pointLightInnerRadius = currentInnerRadius;
            candleLight.pointLightOuterRadius = currentOuterRadius;
        }
    }
}
