using System.Collections;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState {get; private set;}
    public SkeletonMoveState moveState {get; private set;}
    public SkeletonBattleState battleState {get; private set;}
    public SkeletonAttackState attackState {get; private set;}
    public SkeletonStunnedState stunnedState {get; private set;}
    public SkeletonDeathState deathState {get; private set;}
    public SkeletonWaitingState waitingState {get; private set;}
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        deathState = new SkeletonDeathState(this, stateMachine, "Die", this);
        waitingState = new SkeletonWaitingState(this, stateMachine, "Waiting", this);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        base.Update();
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
        yield return new WaitForSeconds(2f);

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

    public override void Stagger() 
    {
        CloseCounterAttackWindow();
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

        enemyStats.ResetHealth();

        healthBarUI.gameObject.SetActive(true);
        healthBarUI.ResetUIRotation(FacingDir);
        healthBarUI.UpdateHealthUI();

    }

}
