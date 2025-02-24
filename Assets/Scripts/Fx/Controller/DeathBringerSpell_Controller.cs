using Cinemachine;
using UnityEngine;

public class DeathBringerSpell_Controller : MonoBehaviour
{
    [SerializeField] private BoxCollider2D hitbox;
    private CharacterStats myStats;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void SetupSpell(CharacterStats _stats) => myStats = _stats; 

    private void Start()
    {
        hitbox.enabled = false;
    }

    public void ActivateHitbox()
    {
        hitbox.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Entity>().SetupKnockBackDir(transform);
            myStats.DoDamge(other.GetComponent<CharacterStats>());
            other.GetComponent<Player>().Stagger();
            
            hitbox.enabled = false;
        }
    }

    private void SelfDestroy() => Destroy(gameObject);

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    private void GenerateImpulse() => impulseSource?.GenerateImpulse();

    //Spell sfx
    private void MagicStartSFX() => AudioManager.instance.PlaySFXNoPitch(23, null);
    private void ImpactSFX() => AudioManager.instance.PlaySFXNoPitch(24, null);

}
