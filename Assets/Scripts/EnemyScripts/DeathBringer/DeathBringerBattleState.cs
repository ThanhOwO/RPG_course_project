using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    private Transform player;
    private Enemy_DeathBringer enemy;
    private int moveDir;
    private bool flippedOnce;
    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.battleTime;
        flippedOnce = false;
        player = PlayerManager.instance.player.transform;

        if (enemy.IsPlayerDetected2() && enemy.IsPlayerDetected2().distance < enemy.attackDistance)
        {
            if (CanAttack())
            {
                enemy.stateMachine.ChangeState(enemy.attackState);
            }
            else
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
            }
        }

        if(player.GetComponent<Player>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }
    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected2())
        {
            stateTimer = enemy.battleTime;
            if(enemy.IsPlayerDetected2().distance < enemy.attackDistance)
            {
                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
                else
                    stateMachine.ChangeState(enemy.idleState);
            } 
        }
        else
        {
            if(flippedOnce == false)
            {
                flippedOnce = true;
                enemy.Flip();
            }
        }

        if(player.position.x > enemy.transform.position.x + .5f)
            moveDir = 1;
        else if(player.position.x < enemy.transform.position.x -.5f)
            moveDir = -1;

        enemy.setVelocity(enemy.moveSpeed * moveDir, rb.linearVelocityY);

    }
    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttack + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttack = Time.time;
            return true;
        }
        return false;
    }

}
