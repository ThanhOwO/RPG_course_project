using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private float jumpTime = 0.2f;
    private float jumpTimeCounter;
    private bool isJumping;
    private float jumpAcceleration = 10f;

    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        isJumping = true;
        jumpTimeCounter = jumpTime;
        player.setVelocity(xInput * player.moveSpeed, player.jumpForce);
        //jump sfx
        AudioManager.instance.PlaySFX(6, null);
    }
    public override void Update()
    {
        base.Update();

        if(rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.airState);
        
        player.setVelocity(xInput * player.moveSpeed, rb.linearVelocityY);
        
        if (player.IsWallSlideDetected() && !player.IsGroundDetected()) 
        { 
            stateMachine.ChangeState(player.wallSlide); 
            return; 
        }

        if (Input.GetKey(KeyCode.Space) && jumpTimeCounter > 0 && isJumping)
        {
            float newJumpForce = player.jumpForce + (jumpAcceleration * (jumpTime - jumpTimeCounter));
            player.setVelocity(xInput * player.moveSpeed, newJumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
            isJumping = false;

        if (Input.GetKey(KeyCode.S))
            player.isCrouchBuffered = true;
        else
            player.isCrouchBuffered = false;
    }
    public override void Exit()
    {
        base.Exit();
    }
}
