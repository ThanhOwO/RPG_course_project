using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    protected Enemy_Skeleton enemy;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        
        enemy.fx.InvokeRepeating("ColorBlink",0,.1f);

        stateTimer = enemy.stunDuration;
        rb.linearVelocity = new Vector2(-enemy.FacingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }
    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelBlink",0);
    }

}