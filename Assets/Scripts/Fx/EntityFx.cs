using System.Collections;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Ailment color")]
    [SerializeField] private Color[] chillcolor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;
    [SerializeField] private Color[] poisonColor;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        sr.color = currentColor;
        sr.material = originalMat;
    }

    public void makeTransparent(bool _transparent)
    {
        if(_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private void ColorBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.black;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating(nameof(IgniteColorFx), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void PoisonFxFor(float _seconds)
    {
        InvokeRepeating(nameof(PoisonColorFx), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating(nameof(ChillColorFx), 0,.3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        InvokeRepeating(nameof(ShockColorFx), 0,.3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    private void IgniteColorFx()
    {
        if(sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void PoisonColorFx()
    {
        if(sr.color != poisonColor[0])
            sr.color = poisonColor[0];
        else
            sr.color = poisonColor[1];
    }

    private void ShockColorFx()
    {
        if(sr.color!= shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    private void ChillColorFx()
    {
        if(sr.color!= chillcolor[0])
            sr.color = chillcolor[0];
        else
            sr.color = chillcolor[1];
    }

}
