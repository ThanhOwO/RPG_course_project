using UnityEngine;

public class SkeletonWaitingState : EnemyState
{
    protected Enemy_Skeleton enemy;

    public SkeletonWaitingState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 2f;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0 && !enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.idleState);
        
        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance)
            stateMachine.ChangeState(enemy.attackState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
