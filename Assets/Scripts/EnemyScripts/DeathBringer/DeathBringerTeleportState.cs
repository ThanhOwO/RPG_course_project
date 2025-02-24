using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{
    private Enemy_DeathBringer enemy;
    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.stats.MakeInvincible(true);

        //Teleport sfx
        AudioManager.instance.PlaySFX(20, null);
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled)
        {
            if(enemy.stats.currentHealth > enemy.stats.maxHealth.GetValue() * 0.5f)
            {
                if(enemy.CanDoSpellCast())
                    stateMachine.ChangeState(enemy.spellCastState);
                else
                    stateMachine.ChangeState(enemy.battleState);
            }
            else
            {
                int rand = Random.Range(0, 100);
                if(rand < 70 && enemy.CanDoPhase2SpellCast())
                {
                    stateMachine.ChangeState(enemy.phase2State);
                }
                else if(enemy.CanDoSpellCast())
                {
                    stateMachine.ChangeState(enemy.spellCastState);
                }
                else
                {
                    stateMachine.ChangeState(enemy.battleState);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.stats.MakeInvincible(false);
    }
}
