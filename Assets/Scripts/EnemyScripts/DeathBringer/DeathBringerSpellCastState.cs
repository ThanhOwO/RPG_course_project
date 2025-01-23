using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    private int amountOfSpell;
    private float spellCastTimer;
    private Enemy_DeathBringer enemy;
    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpell = enemy.amountOfSpells;
        spellCastTimer = .5f;
    }

    public override void Update()
    {
        base.Update();

        spellCastTimer -= Time.deltaTime;

        if(CanCast())
            enemy.CastSpell(); 
        
        if(amountOfSpell <= 0)
            stateMachine.ChangeState(enemy.teleportState);
    }

    private bool CanCast()
    {
        if(amountOfSpell > 0 && spellCastTimer < 0)
        {
            amountOfSpell--;
            spellCastTimer = enemy.spellCooldown;
            return true;
        }
        return false;
    }

    override public void Exit()
    {
        base.Exit();
        enemy.lastTimeCast = Time.time;
    }
}
