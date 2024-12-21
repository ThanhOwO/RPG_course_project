using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        player.setVelocity(xInput * player.moveSpeed, rb.linearVelocityY);
        if(xInput == 0)
        {
            stateMachine.ChangeState(player.brakeState);
        }

        if (xInput == player.FacingDir && player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
