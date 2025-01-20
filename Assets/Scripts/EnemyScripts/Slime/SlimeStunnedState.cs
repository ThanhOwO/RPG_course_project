using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    protected Enemy_Slime enemy;

    public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        
        //enemy.fx.InvokeRepeating("ColorBlink",0,.1f);

        stateTimer = enemy.stunDuration;
    }
    public override void Update()
    {
        base.Update();

        if(rb.linearVelocity.y < .1f && enemy.IsGroundDetected())
        {
            enemy.anim.SetTrigger("StunFold");
            enemy.stats.MakeInvincible(true);
        }

        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        //enemy.fx.Invoke("CancelColorChange",0);
        enemy.stats.MakeInvincible(false);
    }
}
