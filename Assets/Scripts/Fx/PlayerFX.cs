using Cinemachine;
using UnityEngine;

public class PlayerFX : EntityFx
{
    [Header("After image fx")]
    [SerializeField] private GameObject afterImgPrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImgCooldown;
    private float afterImgTimer;

    [Header("Screen shake fx")] 
    [SerializeField] private float shakeMultiplier;
    private CinemachineImpulseSource screenShake;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDmg;

    [Space]
    [SerializeField] private ParticleSystem dustFx;
    [SerializeField] private GameObject dustTrailPrefab;


    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update() {
        afterImgTimer -= Time.deltaTime;
    }

    public void CreateAfterImage()
    {
        if(afterImgTimer <= 0)
        {
            afterImgTimer = afterImgCooldown;
            GameObject newAfterImage = Instantiate(afterImgPrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
        }
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.FacingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void PlayDustFx()
    {
        if(dustFx != null)
            dustFx.Play();
    }

    public void LandingDust()
    {
        if (dustTrailPrefab != null)
        {
            GameObject dust = Instantiate(dustTrailPrefab, transform.position + new Vector3(0f, -0.9f, 0), Quaternion.identity);
            Destroy(dust, 0.3f);
        }
    }

}
