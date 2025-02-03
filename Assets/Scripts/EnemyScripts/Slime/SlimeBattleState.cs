using UnityEngine;

public class SlimeBattleState : EnemyState
{
    private Transform player;
    private Enemy_Slime enemy;
    private int moveDir;
    private bool flippedOnce;
    public SlimeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        flippedOnce = false;
        player = PlayerManager.instance.player.transform;

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance)
        {
            if (CanAttack())
                enemy.stateMachine.ChangeState(enemy.attackState);
            else
                enemy.stateMachine.ChangeState(enemy.idleState);
        }

        if(player.GetComponent<Player>().isDead)
            stateMachine.ChangeState(enemy.idleState);
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

        if ((enemy.moveType == EnemyMoveType.AlwaysMove || enemy.moveType == EnemyMoveType.MoveOnBattle) && enemy.IsGroundDetected())
        {
            if (player.position.x > enemy.transform.position.x + .5f)
                moveDir = 1;
            else if (player.position.x < enemy.transform.position.x - .5f)
                moveDir = -1;

            enemy.setVelocity(enemy.moveSpeed * moveDir, rb.linearVelocityY);
        }
        else if (!enemy.IsGroundDetected() || enemy.moveType == EnemyMoveType.Idle)
            stateMachine.ChangeState(enemy.waitingState);
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
