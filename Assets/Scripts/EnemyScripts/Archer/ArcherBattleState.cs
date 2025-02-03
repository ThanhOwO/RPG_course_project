using UnityEngine;

public class ArcherBattleState : EnemyState
{
    private Transform player;
    private Enemy_Archer enemy;
    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance && enemy.IsPlayerDetected().distance > enemy.safeDistance)
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

            if(enemy.IsPlayerDetected().distance < enemy.safeDistance)
            {
                if(CanJump())
                    stateMachine.ChangeState(enemy.jumpState);
            }
            
            if(enemy.IsPlayerDetected().distance < enemy.attackDistance && verticalDistance < 2)
            {
                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            } 
        }else
        {
            if(stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.agroDistance || verticalDistance > 2f)
                stateMachine.ChangeState(enemy.idleState);
        }

        BattleStateFlipControl();
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

    private bool CanJump()
    {
        if(enemy.GroundBehindCheck() == false || enemy.WallBehindCheck() == true)
            return false;

        if(Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }

        return false;
    }

    private void BattleStateFlipControl()
    {
        if(player.position.x > enemy.transform.position.x && enemy.FacingDir == -1)
            enemy.Flip();
        else if(player.position.x < enemy.transform.position.x && enemy.FacingDir == 1)
            enemy.Flip();

    }
}
