using UnityEngine;

public class PlayerParryState : PlayerState
{
    public PlayerParryState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.parryDuration;
        player.anim.SetBool("ParrySuccessful", false);
    }
    public override void Update()
    {
        base.Update();

        player.zeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {   
                if(hit.GetComponent<Enemy>().CanBeStunned())
                    ParrySuccessful();
            }
        }

        if(stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);

    }
    public override void Exit()
    {
        base.Exit();
    }
    private void ParrySuccessful()
    {
        stateTimer = 10;
        player.anim.SetBool("ParrySuccessful", true);
    }
}
