using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f,1f)]
    [SerializeField] private float percentageModifier = 0.4f; //Each level will be increased by ..%
    private ItemDrop dropSystem;
    public Stat soulsDropAmount;

    protected override void Start()
    {
        //Lvl apply must be called first
        soulsDropAmount.SetDefaultValue(10);
        ApplyLevelModifier();

        base.Start();

        enemy = GetComponent<Enemy>();
        dropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifier()
    {
        //Enemy basic level modifier stats
        Modify(damage);

        Modify(maxHealth);
        Modify(armor);
        Modify(magicRes);
        Modify(evasion);

        Modify(fireDmg);
        Modify(iceDmg);
        Modify(lightningDmg);
        Modify(poisonDmg);
        Modify(soulsDropAmount);
    }

    //Increase enemy power by level and percentage
    private void Modify(Stat _stat)
    {
        float modifier = 0;
        for(int i = 0; i < level; i++)
        {
            modifier += _stat.GetValue() * percentageModifier;
        }
        
        _stat.AddModifier(Mathf.RoundToInt(modifier));
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        if(_damage > 0)
        {
            Color textColor = isCriticalHit ? Color.yellow : Color.gray;
            enemy.fx.CreatePopUpText(_damage.ToString(), textColor);
            isCriticalHit = false;    
        }
    }

    protected override void Die()
    {
        base.Die();
        
        enemy.Die();
        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        dropSystem.GenerateDrop();

    }

    public void ResetHealth()
    {
        currentHealth = maxHealth.GetValue();
    }
    
}
