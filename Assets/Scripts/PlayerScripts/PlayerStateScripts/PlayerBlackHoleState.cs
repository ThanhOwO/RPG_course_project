using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    public float flyTime = .4f;
    public bool skillUsed;
    private float defaultGravity;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
    public override void Enter()
    {
        base.Enter();

        defaultGravity = rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }
    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
            rb.linearVelocity = new Vector2(0, 15);

        if(stateTimer < 0)
        {
            rb.linearVelocity = new Vector2(0, -.1f);

            if(!skillUsed)
            {
                if(player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }
        }
        //We exit state in black hole skills controller when all the attacks are over;
    }
    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;
        player.makeTransparent(false);
    }
}
