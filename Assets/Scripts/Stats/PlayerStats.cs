using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    public override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
    }
}
