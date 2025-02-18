using UnityEngine;

public class PlayerStunnedState : PlayerState
{
    public PlayerStunnedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.canBeStunned = true;
        stateTimer = player.stunDuration;
        player.zeroVelocity();
        player.stats.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime; 
        if(stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
        if(player.isDead)
            stateMachine.ChangeState(player.deathState);
    }

    public override void Exit()
    {
        base.Exit();
        player.canBeStunned = false;
        player.stats.MakeInvincible(false);
    }
}
