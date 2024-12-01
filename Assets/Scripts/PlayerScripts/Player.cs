using System.Collections;
using UnityEngine;

public class Player : Entity
{

    public SkillManager skill { get; private set;} 

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
    #endregion

    public bool isBusy {get; private set;}

    [Header("Attack movement")]
    public Vector2[] atkMovement;
    public float parryDuration;

    [Header("Move info")]
    public float moveSpeed = 7.0f;
    public float jumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {get; private set;}


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
    }

    protected override void Start() 
    {
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
    }

    protected override void Update() 
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();    
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
        if(IsWallDetected())
            return;

        if(Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if(dashDir == 0)
                dashDir = FacingDir;
            stateMachine.ChangeState(dashState);
        }
    }

}
