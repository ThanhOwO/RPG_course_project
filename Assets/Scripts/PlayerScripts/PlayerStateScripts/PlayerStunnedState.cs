using UnityEngine;

public class PlayerStunnedState : PlayerState
{
    public PlayerStunnedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.stunDuration;
    }

    public override void Update()
    {
        base.Update();
        rb.linearVelocity = new Vector2(-player.FacingDir * player.stunDirection.x, player.stunDirection.y);
        if(stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        player.canBeStunned = false;
    }
}
