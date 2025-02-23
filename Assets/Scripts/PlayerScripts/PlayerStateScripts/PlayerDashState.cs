using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CloneOnDash();

        stateTimer = player.dashDuration;

        player.stats.MakeInvincible(true);
        rb.gravityScale = 0;
    }
    public override void Update()
    {
        base.Update();

        if(player.isDead)
            return;

        player.setVelocity(player.dashSpeed * player.dashDir, 0);

        if(Input.GetButtonDown("Jump") && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);

        if(!player.IsGroundDetected() && player.IsWallSlideDetected())
            stateMachine.ChangeState(player.wallSlide);

        if(stateTimer < 0 && (player.IsGroundDetected() || player.IsOnOneWayPlatform()))
            stateMachine.ChangeState(player.brakeState);
        else if(stateTimer < 0 && (!player.IsGroundDetected() || !player.IsOnOneWayPlatform()))
            stateMachine.ChangeState(player.airState);
        
        player.fx.CreateAfterImage();
    }
    public override void Exit()
    {
        base.Exit();

        player.skill.dash.CloneOnArrival();
        player.setVelocity(0, rb.linearVelocityY);
        player.stats.MakeInvincible(false);
        rb.gravityScale = player.defaultGravityScale;

    }
}
