using System.Collections;
using UnityEngine;

public class Player : Entity
{

    public SkillManager skill { get; private set;}
    public GameObject sword { get; private set;}

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
    #endregion

    public bool isBusy {get; private set;}

    [Header("Attack movement")]
    public Vector2[] atkMovement;
    public float parryDuration;

    [Header("Move info")]
    public float moveSpeed = 7.0f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    private float defaultDashSpeed;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {get; private set;}
    public bool playerIsInBlackHole;
    public bool isDead;

    [Header("Stunned info")]
    public float stunDuration;
    public bool canBeStunned;

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
    }

    protected override void Start() 
    {
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update() 
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();

        if(Input.GetKeyDown(KeyCode.F))
            skill.crystal.CanUseSkill();
    }
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
       moveSpeed = moveSpeed * (1 - _slowPercentage);
       jumpForce = jumpForce * (1 - _slowPercentage);
       dashSpeed = dashSpeed * (1 - _slowPercentage);
       anim.speed = anim.speed * (1 - _slowPercentage);

       Invoke(nameof(ReturnDefaultSpeed), _slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }
    public void CatchTheSword()
    {
        if(!playerIsInBlackHole)
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
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
    private void CheckForDashInput()
    {
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

}
