using UnityEngine;

public class PlayerAirState : PlayerState
{
    private float fallGravityScale;
    private float maxFallGravityScale = 8f;
    private float fallAcceleration = 1.5f;

    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        fallGravityScale = player.defaultGravityScale;
    }
    public override void Update()
    {
        base.Update();

        if(player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        if (Mathf.Abs(xInput) > 0.3f && !player.IsWallDetected()) 
            player.setVelocity(player.moveSpeed * .8f * xInput, rb.linearVelocityY);
        else
            player.setVelocity(0, rb.linearVelocityY); //Keep falling straight down if input too small

        // Increase gravity as player falls, but keep it controlled
        if(rb.linearVelocityY < 0 && !player.IsWallDetected())
        {
            fallGravityScale += fallAcceleration * Time.deltaTime;
            fallGravityScale = Mathf.Min(fallGravityScale, maxFallGravityScale);
            rb.gravityScale = fallGravityScale;
        }

        if(player.IsGroundDetected() || player.IsOnOneWayPlatform())
        {
            if (player.isCrouchBuffered)
            {
                player.isCrouchBuffered = false;
                player.fx.LandingDust();
                stateMachine.ChangeState(player.crouchState);
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
                player.fx.LandingDust();
            }
        }

        if (Input.GetKey(KeyCode.S))
            player.isCrouchBuffered = true;
        else
            player.isCrouchBuffered = false;
    }
    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = player.defaultGravityScale;
    }
}
