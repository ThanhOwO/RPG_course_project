using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.setVelocity(xInput * player.moveSpeed, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();

        if(rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.airState);
        
        player.setVelocity(xInput * player.moveSpeed, rb.linearVelocityY);
        
        if (player.IsWallDetected() && !player.IsGroundDetected()) 
        { 
            stateMachine.ChangeState(player.wallSlide); 
            return; 
        }
    }
    public override void Exit()
    {
        base.Exit();

    }
}
