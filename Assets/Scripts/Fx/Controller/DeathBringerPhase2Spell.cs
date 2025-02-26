using System.Collections;
using Cinemachine;
using UnityEngine;

public class DeathBringerPhase2Spell : MonoBehaviour
{
    [SerializeField] private BoxCollider2D smallHitbox;
    [SerializeField] private BoxCollider2D largeHitbox;
    private CharacterStats myStats;
    private Animator animator;
    private CinemachineImpulseSource impulseSource;

    [Header("Duration info")] 
    [SerializeField] private float waitDuration = 2f;
    [SerializeField] private float startDuration = 1f;
    [SerializeField] private float idleDuration = 2f;
    [SerializeField] private float endDuration = 1f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        smallHitbox.enabled = false; 
        largeHitbox.enabled = false;
    }

    public void SetupPhase2Spell(CharacterStats _stats)
    {
        myStats = _stats;
        StartCoroutine(SpellSequence());
    }

    private IEnumerator SpellSequence()
    {
        animator.SetTrigger("Wait");
        yield return new WaitForSeconds(waitDuration);

        animator.SetTrigger("Start");
        yield return new WaitForSeconds(startDuration);

        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(idleDuration);

        animator.SetTrigger("End");
        yield return new WaitForSeconds(endDuration);

    }

    public void ActivateSmallHitbox()
    {
        smallHitbox.enabled = true;
    }
    public void ActivateLargeHitbox()
    {
        smallHitbox.enabled = false;
        largeHitbox.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Entity>().SetupKnockBackDir(transform);
            if(!myStats.isInvincible)
            {
                myStats.DoCustomDamage(other.GetComponent<CharacterStats>(), 70);
                other.GetComponent<Player>().Stagger();
                GenerateImpulse();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Entity>().SetupKnockBackDir(transform);
            if(!myStats.isInvincible)
            {
                myStats.DoCustomDamage(other.GetComponent<CharacterStats>(), 70);
                other.GetComponent<Player>().Stagger();
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);

    public void DisableHitBox()
    {
        largeHitbox.enabled = false;
    }

    public void GenerateImpulse() => impulseSource?.GenerateImpulse();

    //spell sfx
    private void MagicStartSFX() => AudioManager.instance.PlaySFXNoPitch(23, null);
    private void MagicLoopSFX() => AudioManager.instance.PlaySFXNoPitch(25, null);
    private void MagicEndSFX() => AudioManager.instance.PlaySFXNoPitch(26, null);


}
