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

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;
    [SerializeField] private ParticleSystem poisonFX;

    [Header("Hit Fx")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject CritHitFx;

    [Space]
    [SerializeField] private ParticleSystem dustFx;

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

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
        poisonFX.Stop();
    }

    public void IgniteFxFor(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating(nameof(IgniteColorFx), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void PoisonFxFor(float _seconds)
    {
        poisonFX.Play();
        InvokeRepeating(nameof(PoisonColorFx), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        chillFX.Play();
        InvokeRepeating(nameof(ChillColorFx), 0,.3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        shockFX.Play();
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

    public void CreateHitFX(Transform _target, bool _critical)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        //Rotaion for normal hit fx
        Vector3 hitFxRotation = new Vector3(0,0,zRotation);
        GameObject hitPrefab = hitFx;

        if(_critical)
        {
            hitPrefab = CritHitFx;
            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if(GetComponent<Entity>().FacingDir == -1)
                yRotation = 180;
        
            //Rotaion for crit hit fx
            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFX = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);
        
        newHitFX.transform.Rotate(hitFxRotation);

        Destroy(newHitFX, .3f); 
    }

    public void PlayDustFx()
    {
        if(dustFx != null)
            dustFx.Play();
    }
}
