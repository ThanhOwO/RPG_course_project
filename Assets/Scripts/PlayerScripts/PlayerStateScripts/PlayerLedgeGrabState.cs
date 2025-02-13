using UnityEngine;

public class PlayerLedgeGrabState : PlayerState
{

    private Vector2 climbEndPosition;
    public PlayerLedgeGrabState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.zeroVelocity();
        player.rb.gravityScale = 0;
        player.isGrabbingLedge = true;
    }

    public override void Update()
    {
        base.Update();
        if (!player.IsLedgeDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            LedgeClimbOver();
        }

        
        if (Input.GetKeyDown(KeyCode.S))
        {
            DropDownLedge();
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = 3.5f;
         player.isGrabbingLedge = false;
    }

    private void LedgeClimbOver()
    {
        Vector2 ledgePosition = player.transform.position;
        Vector2 offset = player.FacingDir == 1 ? player.offset2 : new Vector2(-player.offset2.x, player.offset2.y);
        climbEndPosition = ledgePosition + offset;
        player.transform.position = climbEndPosition;
        stateMachine.ChangeState(player.idleState);
    }

    private void DropDownLedge()
    {
        Vector2 ledgePosition = player.transform.position;
        Vector2 offset = player.FacingDir == 1 ? player.offset1 : new Vector2(-player.offset1.x, player.offset1.y);
        climbEndPosition = ledgePosition + offset;
        player.transform.position = climbEndPosition;
        stateMachine.ChangeState(player.airState);
    }
}
