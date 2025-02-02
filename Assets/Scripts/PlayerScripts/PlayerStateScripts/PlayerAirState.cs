using UnityEngine;

public class PlayerAirState : PlayerState
{
    private float fallGravityScale = 3.5f;
    private float maxFallGravityScale = 8f;
    private float fallAcceleration = 1.5f;

    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        fallGravityScale = 3.5f;
    }
    public override void Update()
    {
        base.Update();

        if(player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        if(xInput != 0 && !player.IsWallDetected())
            player.setVelocity(player.moveSpeed * .8f * xInput, rb.linearVelocityY);

        if(rb.linearVelocityY < 0 && !player.IsWallDetected())
        {
            fallGravityScale += fallAcceleration * Time.deltaTime;
            fallGravityScale = Mathf.Min(fallGravityScale, maxFallGravityScale);
            rb.gravityScale = fallGravityScale;
        }

        if(player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = 3.5f;
    }
}
