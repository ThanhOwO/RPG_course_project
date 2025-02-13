using System.Collections;
using UnityEngine;

public class Player : Entity
{

    public SkillManager skill { get; private set;}
    public GameObject sword { get; private set;}
    public PlayerStats playerStats { get; private set;}
    public PlayerFX fx { get; private set;}
    private CapsuleCollider2D playerCollider;

    #region States
    public PlayerStateMachine stateMachine {get; private set;}
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState {get; private set;}
    public PlayerJumpState jumpState {get; private set;}
    public PlayerAirState airState {get; private set;}
    public PlayerDashState dashState {get; private set;}
    public PlayerWallSlideState wallSlide {get; private set;}
    public PlayerWallJumpState wallJump {get; private set;}
    public PlayerPrimaryAttack primaryAttack {get; private set;}
    public PlayerParryState parryState {get; private set;}
    public PlayerAimSwordState aimSwordState {get; private set;}
    public PlayerCatchSwordState catchswordState {get; private set;}
    public PlayerBlackHoleState blackHoleState {get; private set;}
    public PlayerDeathState deathState {get; private set;}
    public PlayerBrakeState brakeState {get; private set;}
    public PlayerStunnedState stunnedState {get; private set;}
    public PlayerCrouchState crouchState {get; private set;}
    public PlayerLedgeGrabState ledgeGrabState {get; private set;}
    public PlayerLadderClimbState ladderClimbState {get; private set;}
    public PlayerRestState restState {get; private set;}
    #endregion

    public bool isBusy {get; private set;}

    [Header("Attack movement")]
    public Vector2[] atkMovement;
    public float parryDuration;

    [Header("Move info")]
    public float moveSpeed = 7.0f;
    public float maxJumpForce;
    public float minJumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultMaxJumpForce;
    private float defaultMinJumpForce;
    private float defaultDashSpeed;
    public bool IsTouchingLadder { get; private set; }
    public bool isClimbing;

    [Header("Ledge info")]
    public bool isGrabbingLedge = false;
    [SerializeField] protected Transform ledgeCheck;
    public LayerMask whatIsLedge;
    public float grabRange = 0.5f;
    public Vector2 offset2;
    public Vector2 offset1;

    [Header("One_way platform info")]
    public Transform oneWayCheck;
    public float oneWayCheckDistance = 0.1f;
    public LayerMask whatIsOneWayPlatform;
    [SerializeField] private Vector2 overlapSize = new Vector2(0.3f, 1f);
    [SerializeField] private Vector2 overlapOffset = Vector2.zero;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {get; private set;}

    [Header("Interaction info")]
    public bool playerIsInBlackHole;
    public bool isDead;

    [Header("Stunned info")]
    public float stunDuration;
    public bool canBeStunned;
    [HideInInspector] public Vector2 playerLastestPosition;

    protected override void Awake() 
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump  = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");
        parryState  = new PlayerParryState(this, stateMachine, "Parry");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchswordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");
        deathState = new PlayerDeathState(this, stateMachine, "Die");
        brakeState = new PlayerBrakeState(this, stateMachine, "Brake");
        stunnedState = new PlayerStunnedState(this, stateMachine, "Stun");
        crouchState = new PlayerCrouchState(this, stateMachine, "Crouch");
        ledgeGrabState = new PlayerLedgeGrabState(this, stateMachine, "Grab");
        ladderClimbState = new PlayerLadderClimbState(this, stateMachine, "LadderClimb");
        restState = new PlayerRestState(this, stateMachine, "Rest");
    }

    protected override void Start() 
    {
        base.Start();
        skill = SkillManager.instance;
        playerStats = GetComponent<PlayerStats>();
        stateMachine.Initialize(idleState);
        fx = GetComponent<PlayerFX>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        
        defaultMoveSpeed = moveSpeed;
        defaultMaxJumpForce = maxJumpForce;
        defaultMinJumpForce = minJumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update() 
    {
        if(Time.timeScale == 0 || DialogueController.isTalking || UI.isInputBlocked)
            return;
            
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
        CheckForLedge();
        CheckForLadderClimb();

        if(Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
            skill.crystal.CanUseSkill();
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UseFlask();
    }
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
       moveSpeed = moveSpeed * (1 - _slowPercentage);
       maxJumpForce = maxJumpForce * (1 - _slowPercentage);
       minJumpForce = minJumpForce * (1 - _slowPercentage);
       dashSpeed = dashSpeed * (1 - _slowPercentage);
       anim.speed = anim.speed * (1 - _slowPercentage);

       Invoke(nameof(ReturnDefaultSpeed), _slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        maxJumpForce = defaultMaxJumpForce;
        minJumpForce = defaultMinJumpForce;
        dashSpeed = defaultDashSpeed;
    }
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    public void CatchTheSword()
    {
        if(isDead)
            return;
            
        if(!playerIsInBlackHole || !stats.isDead)
            stateMachine.ChangeState(catchswordState);
        
        if (sword != null)
        {
            Destroy(sword);
        }
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        UI.isInputBlocked = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
        UI.isInputBlocked = false;
    }
    private void CheckForDashInput()
    {   
        if(skill.dash.dashUnlocked == false)
            return;

        if(IsWallDetected() || playerIsInBlackHole || canBeStunned)
            return;
        
        if(Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if(dashDir == 0)
                dashDir = FacingDir;
            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        DisableColliders();
        DisableRigidBody();
        isDead = true;
        stateMachine.ChangeState(deathState);
    }
    public override void Stagger()
    {
        base.Stagger();
        canBeStunned = true;
        stateMachine.ChangeState(stunnedState);
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

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }

    #region Ledge & ladder grab regions
    private bool IsCollidingWithOneWayPlatformAbove()
    {
        if (playerCollider == null)
            return false;
        // Calculate the center and size of the overlap area
        Vector2 overlapCenter = (Vector2)playerCollider.bounds.center + overlapOffset;
        // Check for overlapping colliders within the extended area
        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(overlapCenter, overlapSize, 0, whatIsOneWayPlatform);
        // If any overlapping colliders are found, return true
        return overlappingColliders.Length > 0;
    }

    public bool IsLedgeDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDir, grabRange, whatIsLedge);
        bool isCollidingWithOneWayPlatformAbove = IsCollidingWithOneWayPlatformAbove();
        return hit.collider != null && !isCollidingWithOneWayPlatformAbove;
    }

    private void CheckForLedge()
    {
        if (IsLedgeDetected() && !isGrabbingLedge && !isDead)
        {
            isGrabbingLedge = true;
            stateMachine.ChangeState(ledgeGrabState);
        }
    }

    private void CheckForLadderClimb()
    {
        if(IsTouchingLadder && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f && !isDead)
        {
            isClimbing = true;
            stateMachine.ChangeState(ladderClimbState);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
            IsTouchingLadder = true;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
            IsTouchingLadder = false;
    }
    #endregion

    public bool IsOnOneWayPlatform() => Physics2D.Raycast(oneWayCheck.position, Vector2.down, oneWayCheckDistance, whatIsOneWayPlatform);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawLine(oneWayCheck.position, new Vector3(oneWayCheck.position.x, oneWayCheck.position.y - oneWayCheckDistance));

        if (ledgeCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + Vector3.right * FacingDir * grabRange);
        }

        //Visualize the overlap of player with one way platform
        // if (playerCollider != null)
        // {
        //     Vector2 overlapCenter = (Vector2)playerCollider.bounds.center + overlapOffset;
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireCube(overlapCenter, overlapSize); // Overlap area visualization
        // }
    }

}
