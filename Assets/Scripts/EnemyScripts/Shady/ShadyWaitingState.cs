using UnityEngine;

public class ShadyWaitingState : EnemyState
{
    protected Enemy_Shady enemy;
    public ShadyWaitingState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 1f;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0 && !enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.moveState);
        
        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance)
            stateMachine.ChangeState(enemy.deathState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
