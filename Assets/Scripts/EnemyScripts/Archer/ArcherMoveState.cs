using UnityEngine;

public class ArcherMoveState : ArcherGroundedState
{
    public ArcherMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if(enemy.moveType == EnemyMoveType.AlwaysMove || (enemy.moveType == EnemyMoveType.MoveOnBattle && enemy.IsPlayerDetected()))
        {
            enemy.setVelocity(enemy.moveSpeed * enemy.FacingDir, rb.linearVelocityY);

            if(enemy.IsWallDetected() || !enemy.IsGroundDetected())
            {
                stateMachine.ChangeState(enemy.idleState);
                enemy.Flip();
            }
        } 
        else
            stateMachine.ChangeState(enemy.idleState);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
