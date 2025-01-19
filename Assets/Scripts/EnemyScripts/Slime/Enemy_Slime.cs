using System.Collections;
using UnityEngine;

public class Enemy_Slime : Enemy
{
    #region States
    public SlimeIdleState idleState { get; private set;}
    public SlimeMoveState moveState { get; private set;}
    public SlimeBattleState battleState { get; private set;}
    public SlimeAttackState attackState { get; private set;}
    public SlimeStunnedState stunnedState { get; private set;}
    public SlimeDeathState deathState { get; private set;}
    public SlimeStaggerState staggerState { get; private set;}
    #endregion

    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(-1);

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stun", this);
        deathState = new SlimeDeathState(this, stateMachine, "Die", this);
        staggerState = new SlimeStaggerState(this, stateMachine, "Stagger", this);

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
        yield return new WaitForSeconds(5f);

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
        Destroy(gameObject);
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
        stateMachine.ChangeState(staggerState);
    }
}
