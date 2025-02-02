using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(CapsuleCollider2D))]

[RequireComponent(typeof(EnemyStats))]

[RequireComponent(typeof(EntityFx))]

[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned info")]
    public float stunDuration = 2;
    protected bool canBeStunned;
    public bool isStaggered;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed = 2;
    public float idleTime = 2;
    public float battleTime = 7;
    private float defaultMoveSpeed;

    [Header("Attack info")]
    public float agroDistance = 2;
    public float attackDistance = 2;
    public float attackHeight = 2f;
    public float attackCooldown;
    public float minAttackCooldown = 1;
    public float maxAttackCooldown = 2;
    [HideInInspector] public float lastTimeAttack;

    public EnemyStateMachine stateMachine {get; private set;}
    public EntityFx fx {get; private set;}
    public string lastAnimBoolName  {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();
        fx = GetComponent<EntityFx>();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke(nameof(ReturnDefaultSpeed), _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            anim.speed = 1;
            moveSpeed = defaultMoveSpeed;
        }
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));

    protected virtual IEnumerator FreezeTimerCoroutine(float _second)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_second);
        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if(canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }
    public virtual RaycastHit2D IsPlayerDetected() 
    {
        RaycastHit2D playerDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 13, whatIsPlayer);
        RaycastHit2D wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 13, whatIsGround);

        if(wallDetected)
        {
            if(wallDetected.distance < playerDetected.distance)
                return default(RaycastHit2D);
            
        }
        return playerDetected;
    } 
    //My player detection for boss
    public virtual RaycastHit2D IsPlayerDetected2() => Physics2D.BoxCast(wallCheck.position, new Vector2(attackDistance, attackHeight), 0, Vector2.right * FacingDir, attackDistance, whatIsPlayer);
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual void AnimationSpecialAttackTrigger()
    {

    }
    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * FacingDir, transform.position.y));
        Gizmos.DrawWireCube(
            new Vector3(transform.position.x + attackDistance * FacingDir, transform.position.y),
            new Vector3(attackDistance, attackHeight, 1)
        );
        //Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + agroDistance, transform.position.y));
    }

    public override void Stagger()
    {
        base.Stagger();
        isStaggered = true;
    }

}
