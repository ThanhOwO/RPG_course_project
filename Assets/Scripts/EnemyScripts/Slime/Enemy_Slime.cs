using System.Collections;
using UnityEngine;

public enum SlimeType { big, medium, small }

public class Enemy_Slime : Enemy
{
    [Header("Slime specification")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimeToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;


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

        if(slimeType == SlimeType.small)
            return;

        CreateSlimes(slimeToCreate, slimePrefab);
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

    private void CreateSlimes(int _amount, GameObject _slimePrefab)
    {
        for(int i = 0; i < _amount; i++)
        {
            GameObject newSlime = Instantiate(slimePrefab, transform.position, Quaternion.identity);
            newSlime.GetComponent<Enemy_Slime>().SetupSlimes(FacingDir);
        }
    }

    public void SetupSlimes(int _facingDir)
    {
        if(_facingDir != FacingDir)
            Flip();
            
        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xVelocity * -FacingDir, yVelocity);
        Invoke(nameof(CancelKnockback), 1.5f);
    }

    private void CancelKnockback() => isKnocked = false;
}
