using UnityEngine;

public class PlayerParryState : PlayerState
{
    private bool canCreateClone;
    public PlayerParryState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (player.skill.parry.CanUseSkill())
            player.skill.parry.cooldownTimer = player.skill.parry.cooldown;
    
        canCreateClone = true;
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
            if(hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                ParrySuccessful();
            }
            
            if(hit.GetComponent<Enemy>() != null)
            {   
                if(hit.GetComponent<Enemy>().CanBeStunned())
                {
                    ParrySuccessful();

                    if (player.skill.parry.CanUseSkill())
                        player.skill.parry.UseSkill(); //restore health on successful parry

                    if(canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parry.MakeMirageOnParry(hit.transform);
                    }
                }
                
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
