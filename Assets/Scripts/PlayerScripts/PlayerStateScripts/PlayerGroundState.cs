using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.R))
            stateMachine.ChangeState(player.blackHoleState);
            
        if(Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimSwordState);
            
        if(Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked)
            stateMachine.ChangeState(player.parryState);

        if(Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);
        
        if(!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);

        if(Input.GetKeyDown(KeyCode.S) && player.IsGroundDetected())
            stateMachine.ChangeState(player.crouchState);
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
