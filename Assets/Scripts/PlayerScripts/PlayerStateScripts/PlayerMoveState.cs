using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    private float moveTimer;
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        moveTimer = 0f;
        AudioManager.instance.PlaySFX(14, null);
    }
    public override void Update()
    {
        base.Update();

        player.setVelocity(xInput * player.moveSpeed, rb.linearVelocityY);

        if (xInput != 0)
            moveTimer += Time.deltaTime;

        if(xInput == 0 && player.IsGroundDetected())
        {
            if(moveTimer >= 0.5f)
                stateMachine.ChangeState(player.brakeState);
            else
                stateMachine.ChangeState(player.idleState);
        }

        if (xInput == player.FacingDir && player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(14);

    }
}
