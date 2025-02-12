using System.Collections;
using UnityEngine;

public class PlayerCrouchState : PlayerState
{
    private CapsuleCollider2D playerCollider;
    private Vector2 defaultColliderSize;
    private Vector2 crouchColliderSize = new Vector2(0.8117779f, 1.421077f); // Change this value to fit the player collider size
    private Vector2 defaultColliderOffset;
    private Vector2 crouchColliderOffset = new Vector2(-0.05477181f, -0.6321046f); // Change this value to fit the player collider offset
    private Collider2D platformCollider;

    public PlayerCrouchState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        playerCollider = player.GetComponent<CapsuleCollider2D>();
        defaultColliderSize = playerCollider.size;
        defaultColliderOffset = playerCollider.offset;
    }

    public override void Enter()
    {
        base.Enter();
        playerCollider.size = crouchColliderSize;
        playerCollider.offset = crouchColliderOffset;
        platformCollider = Physics2D.OverlapCircle(player.oneWayCheck.position, 0.1f, player.whatIsOneWayPlatform);
    }

    public override void Update()
    {
        base.Update();
        player.zeroVelocity();
        
        if (Input.GetKeyUp(KeyCode.S) && (player.IsGroundDetected() || player.IsOnOneWayPlatform()))
            stateMachine.ChangeState(player.idleState);
        
        if(player.IsOnOneWayPlatform() && Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.airState);
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), platformCollider, true);
            player.StartCoroutine(ReenableCollision(platformCollider));
        }

        if (Input.GetKeyDown(KeyCode.A) && player.FacingDir == 1)
        {
            player.Flip();
        }
        else if (Input.GetKeyDown(KeyCode.D) && player.FacingDir == -1)
        {
            player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
        playerCollider.size = defaultColliderSize;
        playerCollider.offset = defaultColliderOffset;
    }
    
    private IEnumerator ReenableCollision(Collider2D platformCollider)
    {
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), platformCollider, false);
    }
}
