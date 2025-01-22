using UnityEngine;

public class ShadyBattleState : EnemyState
{
    protected Transform player;
    protected Enemy_Shady enemy;
    private int moveDir;
    private float defaultSpeed;
    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        defaultSpeed = enemy.moveSpeed;
        enemy.moveSpeed = enemy.battleMoveSpeed;

        player = PlayerManager.instance.player.transform;

        if(player.GetComponent<Player>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }
    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
                stateMachine.ChangeState(enemy.deathState);
        }else
        {
            if(stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
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
        enemy.moveSpeed = defaultSpeed;
    }
}
