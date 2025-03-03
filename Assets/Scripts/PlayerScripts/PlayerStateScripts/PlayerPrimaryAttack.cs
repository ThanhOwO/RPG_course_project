using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 0.3f;

    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        player.isAttacking = true;
        xInput = 0;

        if(comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;
        player.anim.SetInteger("comboCounter", comboCounter);

        float atkDirection = GetAttackDirection();

        player.setVelocity(player.atkMovement[comboCounter].x * atkDirection, player.atkMovement[comboCounter].y);

        stateTimer = .1f;
    }
    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            player.zeroVelocity();

        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);

        if(!player.IsGroundDetected() && !player.IsOnOneWayPlatform())
            stateMachine.ChangeState(player.airState);
    }
    public override void Exit()
    {
        base.Exit();

        //player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
        player.isAttacking = false;
    }

    private float GetAttackDirection()
    {
        float atkDirection = player.FacingDir;
        if (xInput != 0)
            atkDirection = xInput;
        return atkDirection;
    }

    public bool IsLastComboAtk()
    {
        if(comboCounter == 2)
            return true;
        else
            return false;
    }

}
