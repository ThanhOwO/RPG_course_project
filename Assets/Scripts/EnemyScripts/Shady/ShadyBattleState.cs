using UnityEngine;

public class ShadyBattleState : EnemyState
{
    protected Transform player;
    protected Enemy_Shady enemy;
    private int moveDir;
    private float defaultSpeed;
    private bool flippedOnce;
    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        flippedOnce = false;
        defaultSpeed = enemy.moveSpeed;
        enemy.moveSpeed = enemy.battleMoveSpeed;

        player = PlayerManager.instance.player.transform;

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
                stateMachine.ChangeState(enemy.deathState);
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
        enemy.moveSpeed = defaultSpeed;
    }
}
