using UnityEngine;

public class PlayerRestState : PlayerState
{
    public PlayerRestState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.zeroVelocity();
        
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 1f);
    }
    
    
}
