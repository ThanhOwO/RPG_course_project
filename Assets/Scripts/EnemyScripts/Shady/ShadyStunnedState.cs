using UnityEngine;

public class ShadyStunnedState : EnemyState
{
    protected Enemy_Shady enemy;
    public ShadyStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        
        //enemy.fx.InvokeRepeating("ColorBlink",0,.1f);

        stateTimer = enemy.stunDuration;
    }
    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        enemy.isStaggered = false;
        //enemy.fx.Invoke("CancelColorChange",0);
    }
}
