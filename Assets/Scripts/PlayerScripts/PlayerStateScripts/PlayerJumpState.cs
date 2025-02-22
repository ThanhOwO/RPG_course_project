using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private float jumpTime = 0.2f;
    private float jumpTimeCounter;
    private bool isJumping;

    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        isJumping = true;
        jumpTimeCounter = jumpTime;
        player.setVelocity(xInput * player.moveSpeed, player.minJumpForce);
    }
    public override void Update()
    {
        base.Update();

        if(rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.airState);
        
        player.setVelocity(xInput * player.moveSpeed, rb.linearVelocityY);
        
        if (player.IsWallDetected() && !player.IsGroundDetected()) 
        { 
            stateMachine.ChangeState(player.wallSlide); 
            return; 
        }

        if (Input.GetKey(KeyCode.Space) && jumpTimeCounter > 0 && isJumping)
        {
            player.setVelocity(xInput * player.moveSpeed, player.minJumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
            isJumping = false;

        if (Input.GetKeyDown(KeyCode.S))
            player.isCrouchBuffered = true;
    }
    public override void Exit()
    {
        base.Exit();
    }
}
