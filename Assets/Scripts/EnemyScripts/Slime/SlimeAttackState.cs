using UnityEngine;

public class SlimeAttackState : EnemyState
{
    protected Enemy_Slime enemy;

    public SlimeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        enemy.zeroVelocity();

        if(triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttack = Time.time;
    }
}
