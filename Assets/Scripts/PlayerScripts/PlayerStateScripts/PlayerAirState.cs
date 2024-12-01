using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if(player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        if(xInput != 0 && !player.IsWallDetected())
            player.setVelocity(player.moveSpeed * .8f * xInput, rb.linearVelocityY);

        if(player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        
    }
}
