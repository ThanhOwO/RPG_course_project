using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDir;
    private bool flippedOnce;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.battleTime;
        flippedOnce = false;
        player = PlayerManager.instance.player.transform;

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance)
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

        float verticalDistance = Mathf.Abs(enemy.transform.position.y - player.position.y);

        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if(enemy.IsPlayerDetected().distance < enemy.attackDistance && verticalDistance < 2)
            {
                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            } 
        }else
        {
            if(flippedOnce == false)
            {
                flippedOnce = true;
                enemy.Flip();
            }

            if(stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.agroDistance || verticalDistance > 2f)
                stateMachine.ChangeState(enemy.idleState);
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
