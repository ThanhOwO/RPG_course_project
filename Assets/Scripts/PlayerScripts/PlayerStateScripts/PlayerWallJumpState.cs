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
            stateMachine.ChangeState(player.idleState);

        if(player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        if(xInput != 0 && !player.IsWallDetected())
            player.setVelocity(player.moveSpeed * .8f * xInput, rb.linearVelocityY);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
