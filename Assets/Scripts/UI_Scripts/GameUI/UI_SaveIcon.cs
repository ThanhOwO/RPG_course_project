using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaveIcon : MonoBehaviour
{
    [SerializeField] private Image saveIcon;
    [SerializeField] private float fadeDuration = 0.5f;
    private Coroutine blinkingCoroutine;

    private void OnEnable()
    {
        blinkingCoroutine = StartCoroutine(BlinkEffect());
    }

    private void OnDisable()
    {
        if (blinkingCoroutine != null)
        {
            StopCoroutine(blinkingCoroutine);
            blinkingCoroutine = null;
        }
    }

    private IEnumerator BlinkEffect()
    {
        bool fadingOut = false;

        while (true)
        {
            float elapsed = 0f;
            float startAlpha = fadingOut ? 1f : 0.2f;
            float endAlpha = fadingOut ? 0.2f : 1f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
                saveIcon.color = new Color(saveIcon.color.r, saveIcon.color.g, saveIcon.color.b, alpha);
                yield return null;
            }

            fadingOut = !fadingOut;
        }
    }
}
