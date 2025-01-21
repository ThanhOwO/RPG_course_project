using UnityEngine;

public class ArcherStunnedState : EnemyState
{
    private Enemy_Archer enemy;
    public ArcherStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
