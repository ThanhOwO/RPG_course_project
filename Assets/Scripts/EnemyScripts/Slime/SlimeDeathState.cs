using UnityEngine;

public class SlimeDeathState : EnemyState
{
    protected Enemy_Slime enemy;

    public SlimeDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();

        enemy.enabled = false;
        // Alex's death code from Udemy
        // enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        // enemy.anim.speed = 0;
        // enemy.cd.enabled = false;

        // stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();

        enemy.zeroVelocity();
        // Alex's death code from Udemy
        // if(stateTimer > 0)
        // {
        //     rb.linearVelocity = new Vector2(0, 10);
        // }
    }
}