using UnityEngine;

public class DeathBringerPhase2State : EnemyState
{
    private Enemy_DeathBringer enemy;
    private float phase2SpellDuration = 6f; 
    private float timer;
    public DeathBringerPhase2State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        timer = phase2SpellDuration;
        enemy.CastSpell2();
    }

    public override void Update()
    {
        base.Update();
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            stateMachine.ChangeState(enemy.teleportState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast2 = Time.time;
    }
}
