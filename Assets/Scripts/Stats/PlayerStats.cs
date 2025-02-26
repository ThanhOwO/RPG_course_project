using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        //player get hit sfx
        if(!isInvincible)
        {
            AudioManager.instance.PlaySFX(14, null);
            AudioManager.instance.PlaySFX(15, null);
        }
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GameManager.instance.lastDeathAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;
        //GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        if(isDead) 
            return;

        if(_damage > GetMaxHealthValue() * .3f)
        {           
            player.fx.ScreenShake(player.fx.shakeHighDmg);
        }

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if(currentArmor != null)
            currentArmor.Effect(player.transform);
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDamage(CharacterStats _targetStats, float _multiplier)
    {
        if(CanDogdeAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockBackDir(transform);
        int totalDamage = damage.GetValue() + strength.GetValue();
        
        if(_multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);

        if(canCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage); //Physical damage to target
        //DoMagicalDamage(_targetStats); //Magical damage to target
    }

}
