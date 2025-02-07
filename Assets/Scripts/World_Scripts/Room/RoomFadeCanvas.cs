using UnityEngine;
using UnityEngine.UI;

public class RoomFadeCanvas : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [Range(0.1f, 10f), SerializeField] private float fadeOutSpeed = 5f;
    [Range(0.1f, 10f), SerializeField] private float fadeInSpeed = 5f;
    public bool isFadingOut { get; private set; }
    public bool isFadingIn { get; private set; }
    [SerializeField] private Color fadeOutStartColor;

    private void Awake()
    {
        fadeOutStartColor.a = 0f;
    }

    private void Update()
    {
        if(isFadingOut)
        {
            if(fadeImage.color.a < 1f)
            {
                fadeOutStartColor.a += Time.deltaTime * fadeInSpeed;
                fadeImage.color = fadeOutStartColor;
            }
            else
            {
                isFadingOut = false;
            }
        }

        if(isFadingIn)
        {
            if(fadeImage.color.a > 0f)
            {
                fadeOutStartColor.a -= Time.deltaTime * fadeOutSpeed;
                fadeImage.color = fadeOutStartColor;
            }
            else
            {
                isFadingIn = false;
            }
        }
    }

    public void FadeOut()
    {
        fadeImage.color = fadeOutStartColor;
        isFadingOut = true;
    }

    public void FadeIn()
    {
        if(fadeImage.color.a >= 1f)
        {
            fadeImage.color = fadeOutStartColor;
            isFadingIn = true;
        }
    }

}
