using UnityEngine;

public class DeathBringerPhase2State : EnemyState
{
    private Enemy_DeathBringer enemy;
    private float phase2SpellDuration = 6f; 
    public DeathBringerPhase2State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = phase2SpellDuration;
        enemy.CastSpell2();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
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
