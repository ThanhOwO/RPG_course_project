using UnityEngine;

public class PlayerGroundState : PlayerState
{
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        coyoteTimeCounter = coyoteTime;
    }
    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked)
        {
            if (player.skill.blackhole.cooldownTimer > 0)
            {
                return;
            }
            stateMachine.ChangeState(player.blackHoleState);
        }
            
        if(Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
            stateMachine.ChangeState(player.aimSwordState);
            
        if(Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked)
        {
            if (player.skill.parry.cooldownTimer > 0)
            {
                return;
            }
            stateMachine.ChangeState(player.parryState);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);
        
        if(!player.IsGroundDetected() && coyoteTimeCounter <= 0)
            stateMachine.ChangeState(player.airState);

        if(Input.GetKeyDown(KeyCode.S) && player.IsGroundDetected())
            stateMachine.ChangeState(player.crouchState);
        
        //Coyote jump
        if (!player.IsGroundDetected())
            coyoteTimeCounter -= Time.deltaTime;
        else
            coyoteTimeCounter = coyoteTime;

        if(Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0)
            stateMachine.ChangeState(player.jumpState);

    }
    public override void Exit()
    {
        base.Exit();
    }
    private bool HasNoSword()
    {
        if(!player.sword)
        {
            return true;
        }
        
        player.sword.GetComponent<Sword_Skill_Controller2>().ReturnSword();
        return false;
    }
}
