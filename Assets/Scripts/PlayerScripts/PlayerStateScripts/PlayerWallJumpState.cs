using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = 1f;
        player.setVelocity(5 * -player.FacingDir, player.wallJumpForce);
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer < 0)
            stateMachine.ChangeState(player.airState);
        if(player.IsGroundDetected() || player.IsOnOneWayPlatform())
        {
            stateMachine.ChangeState(player.idleState);
            player.fx.LandingDust();
        }

        if(player.IsWallSlideDetected())
            stateMachine.ChangeState(player.wallSlide);

    }
    public override void Exit()
    {
        base.Exit();
    }
}
