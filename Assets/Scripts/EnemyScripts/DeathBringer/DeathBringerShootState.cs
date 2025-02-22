using UnityEngine;

public class DeathBringerShootState : EnemyState
{
    private Enemy_DeathBringer enemy;
    private float shootDuration = 10f;
    public DeathBringerShootState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = shootDuration;
        enemy.bulletSpawnerLeft.gameObject.SetActive(true);
        enemy.bulletSpawnerRight.gameObject.SetActive(true);
        enemy.Invoke("CastBulletSpell", 0.5f);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.teleportState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.bulletSpawnerLeft.gameObject.SetActive(false);
        enemy.bulletSpawnerRight.gameObject.SetActive(false);
        enemy.lastTimeShootSpell = Time.time;
    }
}
