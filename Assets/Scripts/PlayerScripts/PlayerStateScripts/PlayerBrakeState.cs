using UnityEngine;

public class PlayerBrakeState : PlayerState
{
    private bool isBraking;
    public PlayerBrakeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isBraking = true;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0 || !isBraking)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        isBraking = false;
    }
}
