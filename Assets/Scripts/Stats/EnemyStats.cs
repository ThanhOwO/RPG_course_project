using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    public override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        enemy.DamageEffect();
    }
}
