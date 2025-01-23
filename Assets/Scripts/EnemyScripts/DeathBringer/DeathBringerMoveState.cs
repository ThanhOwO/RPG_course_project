using UnityEngine;

public class DeathBringerMoveState : EnemyState
{
    protected Enemy_DeathBringer enemy;
    public DeathBringerMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        enemy.setVelocity(enemy.moveSpeed * enemy.FacingDir, rb.linearVelocityY);

        if(enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
            enemy.Flip();
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
