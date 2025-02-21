using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private Transform player;
    private Enemy_DeathBringer enemy;

    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();
        if(Vector2.Distance(player.transform.position, enemy.transform.position) < 15 && !enemy.bossFightBegun)
        {
            enemy.StartShowBossHealth();
        }

        if(stateTimer < 0 && enemy.bossFightBegun)
            stateMachine.ChangeState(enemy.battleState);

    }

    public override void Exit()
    {
        base.Exit();
    }
}
