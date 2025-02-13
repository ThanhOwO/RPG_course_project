using System.Collections;
using UnityEngine;

public class Enemy_Shady : Enemy
{
    [Header("Shady specific info")]
    public float battleMoveSpeed;
    [SerializeField] private GameObject explosivePrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;

    #region States
    public ShadyMoveState moveState { get; private set; }
    public ShadyIdleState idleState { get; private set; }
    public ShadyDeathState deathState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyWaitingState waitingState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);
        battleState = new ShadyBattleState(this, stateMachine, "Run", this);
        deathState = new ShadyDeathState(this, stateMachine, "Explode", this);
        stunnedState = new ShadyStunnedState(this, stateMachine, "Stun", this);
        waitingState = new ShadyWaitingState(this, stateMachine, "Waiting", this);

    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }
    public override void Die()
    {
        base.Die();

        CloseCounterAttackWindow();
        DisableColliders();
        DisableRigidBody();
        StartCoroutine(CorpseRemainTime());
        stateMachine.ChangeState(deathState);
    }

    //Death animation remain time of skeleton
    private IEnumerator CorpseRemainTime()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        Color originalColor = sr.color;
 
        while (sr.color.a > 0)
        {
            // Reduce the alpha value over time
            float newAlpha = sr.color.a - (Time.deltaTime * 0.5f);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            isKnocked = false;
 
            yield return null;
        }
 
        // Destroy the game object after fading out
        //Destroy(gameObject);

        //Respawn
        gameObject.SetActive(false);
    }

    private void DisableColliders()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach(Collider2D collider in colliders)
            collider.enabled = false;
    }

    private void DisableRigidBody()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if(rb != null)
            rb.simulated = false;
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newExplosion = Instantiate(explosivePrefab, transform.position, Quaternion.identity);
        newExplosion.GetComponent<ShadyExplode_Controller>().SetupExplosive(stats, growSpeed, maxSize, attackCheckRadius);

        cd.enabled = false;
        rb.gravityScale = 0;
    }

    public override void Stagger() 
    {
        stateMachine.ChangeState(stunnedState);
    }

    public override void Respawn()
    {
        base.Respawn();
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        FacingDir = 1;
        FacingRight = true;

        Color originalColor = sr.color;
        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        cd.enabled = true;
        rb.simulated = true;

        stateMachine.Initialize(idleState);

        isStaggered = false;
        canBeStunned = false;
        stats.isDead = false;

        enemyStats.ResetHealth();

        healthBarUI.gameObject.SetActive(true);
        healthBarUI.ResetUIRotation(FacingDir);
        healthBarUI.UpdateHealthUI();

    }
}
