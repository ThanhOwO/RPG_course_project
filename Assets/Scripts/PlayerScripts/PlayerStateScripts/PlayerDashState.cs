using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CloneOnDash();

        stateTimer = player.dashDuration;

        player.stats.MakeInvincible(true);
    }
    public override void Update()
    {
        base.Update();

        if(player.isDead)
            return;

        if(!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        player.setVelocity(player.dashSpeed * player.dashDir, 0);

        if(stateTimer < 0 && player.IsGroundDetected())
            stateMachine.ChangeState(player.brakeState);
        else if(stateTimer < 0 && !player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();

        player.skill.dash.CloneOnArrival();
        player.setVelocity(0, rb.linearVelocityY);
        player.stats.MakeInvincible(false);

    }
}
