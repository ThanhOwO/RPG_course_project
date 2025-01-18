using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private float colorLooseRate;

    public void SetupAfterImage(float _loosingSpd, Sprite _spriteImg)
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = _spriteImg;
        colorLooseRate = _loosingSpd;
    }

    private void Update()
    {
        float alpha = sr.color.a - colorLooseRate * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if(sr.color.a <= 0)
            Destroy(gameObject);

    }
}
