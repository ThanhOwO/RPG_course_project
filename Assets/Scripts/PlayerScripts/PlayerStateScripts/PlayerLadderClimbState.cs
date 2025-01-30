using UnityEngine;

public class PlayerLadderClimbState : PlayerState
{
    private float climbSpeed = 5f;

    public PlayerLadderClimbState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            player.anim.speed = 1;
            player.rb.linearVelocity = new Vector2(0, verticalInput * climbSpeed);
        }
        else
        {
            player.rb.linearVelocity = new Vector2(0, 0);
            player.anim.speed = 0;
        }

        player.anim.SetFloat("ladderSpeed", verticalInput);

        if (Input.GetButtonDown("Jump") || !player.IsTouchingLadder)
            stateMachine.ChangeState(player.jumpState);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            stateMachine.ChangeState(player.airState);
        
        if(player.IsGroundDetected() && verticalInput <= 0.1f)
            stateMachine.ChangeState(player.idleState);

    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = 3.5f;
        player.isClimbing = false;
        player.anim.speed = 1;
    }
}
