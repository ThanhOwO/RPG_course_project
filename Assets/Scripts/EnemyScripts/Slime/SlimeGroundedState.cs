using UnityEngine;

public class SlimeGroundedState : EnemyState
{
    protected Enemy_Slime enemy;
    protected Transform player;
    public SlimeGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();
        float horizontalDistance = Mathf.Abs(enemy.transform.position.x - player.position.x);
        float verticalDistance = Mathf.Abs(enemy.transform.position.y - player.position.y);

        // Check if player is within agro distance horizontally and within a reasonable vertical range
        if ((horizontalDistance < enemy.agroDistance && verticalDistance < 2f) || enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

}
