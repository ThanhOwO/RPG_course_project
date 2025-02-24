using System.Collections;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        GameObject.Find("UICanvas").GetComponent<UI>().SwitchOnEndScreen();
        
        //Game over sfx
        var sfx = AudioManager.instance;
        sfx.StopBGM();
        sfx.PlaySFXNoPitch(12, null);
    }

    public override void Update()
    {
        base.Update();
        
        player.zeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
