using UnityEngine;

public class ShadyDeathState : EnemyState
{
    private Enemy_Shady enemy;
    public ShadyDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.stats.KillEntity();
        enemy.enabled = false;
    }

    public override void Update()
    {
        base.Update();
        enemy.zeroVelocity();

    }
}
