using UnityEngine;

public class SlimeIdleState : SlimeGroundedState
{
    public SlimeIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            if (enemy.moveType == EnemyMoveType.AlwaysMove)
            {
                stateMachine.ChangeState(enemy.moveState);
            }
            else if (enemy.moveType == EnemyMoveType.MoveOnBattle && enemy.IsPlayerDetected())
            {
                stateMachine.ChangeState(enemy.battleState);
            }
            else if(enemy.moveType == EnemyMoveType.Idle)
            {
                stateMachine.ChangeState(enemy.waitingState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        //AudioManager.instance.PlaySFX(24,enemy.transform);
    }
}
