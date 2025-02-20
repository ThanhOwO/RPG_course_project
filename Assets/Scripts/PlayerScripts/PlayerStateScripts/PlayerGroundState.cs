using UnityEngine;

public class PlayerGroundState : PlayerState
{
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float lastPositionUpdateTime;

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
        if(DialogueController.isTalking || UI.isInputBlocked || player.isCutScene) return;
        
        base.Update();
        GetInput();

        //Get the current player position each 3 seconds
        if (Time.time - lastPositionUpdateTime >= 3f)
        {
            player.playerLastestPosition = player.transform.position;
            lastPositionUpdateTime = Time.time;
        }

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

    private void GetInput()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked)
        {
            if (player.skill.blackhole.cooldownTimer > 0)
            {
                player.fx.CreatePopUpText("Cooldown!", Color.white);
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
        
        if(!player.IsGroundDetected() && !player.IsOnOneWayPlatform() && coyoteTimeCounter <= 0)
            stateMachine.ChangeState(player.airState);

        if(Input.GetKeyDown(KeyCode.S) && (player.IsGroundDetected() || player.IsOnOneWayPlatform()))
            stateMachine.ChangeState(player.crouchState);

        //Coyote jump
        if (!player.IsGroundDetected() && !player.IsOnOneWayPlatform())
            coyoteTimeCounter -= Time.deltaTime;
        else
            coyoteTimeCounter = coyoteTime;

        if(Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0)
            stateMachine.ChangeState(player.jumpState);
    }

}
