using UnityEngine;

public class ArcherJumpState : EnemyState
{
    protected Enemy_Archer enemy;
    public ArcherJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = new Vector2(enemy.jumpVelocity.x * -enemy.FacingDir, enemy.jumpVelocity.y);
        
    }
    public override void Update()
    {
        base.Update();

        enemy.anim.SetFloat("yVelocity", rb.linearVelocity.y);

        if(enemy.rb.linearVelocity.y < 0 && enemy.IsGroundDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }

    }
    public override void Exit()
    {
        base.Exit();
    }
}
