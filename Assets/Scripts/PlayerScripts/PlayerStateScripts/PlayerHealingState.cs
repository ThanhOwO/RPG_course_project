using UnityEngine;

public class PlayerHealingState : PlayerState
{
    public PlayerHealingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isHealing = true;
        player.zeroVelocity();
        stateTimer = 1.2f;

        //Healing sfx
        AudioManager.instance.PlaySFX(27, null);
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        player.isHealing = false;
    }
}
