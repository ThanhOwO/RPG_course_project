using UnityEngine;

public class DeathBringerDeathState : EnemyState
{
    protected Enemy_DeathBringer enemy;
    public DeathBringerDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.enabled = false;
        enemy.HideBossHealth();

    }

    public override void Update()
    {
        base.Update();

        enemy.zeroVelocity();

    }
}
