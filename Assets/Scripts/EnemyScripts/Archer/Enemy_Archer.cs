using System.Collections;
using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Archer specific info")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;
    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float safeDistance; //if player too close => jump backward
    [HideInInspector] public float lastTimeJumped;

    [Header("Additional collision check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;
    
    #region States
    public ArcherIdleState idleState {get; private set;}
    public ArcherMoveState moveState {get; private set;}
    public ArcherBattleState battleState {get; private set;}
    public ArcherAttackState attackState {get; private set;}
    public ArcherStunnedState stunnedState {get; private set;}
    public ArcherDeathState deathState {get; private set;}
    public ArcherJumpState jumpState {get; private set;}

    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        stunnedState = new ArcherStunnedState(this, stateMachine, "Stun", this);
        deathState = new ArcherDeathState(this, stateMachine, "Die", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
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

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);
        newArrow.GetComponent<Arrow_Controller>().SetupArrow(arrowSpeed * FacingDir, stats);
        
    }

    public bool GroundBehindCheck() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, whatIsGround);
    public bool WallBehindCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDir, wallCheckDistance + 2, whatIsGround);


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
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
