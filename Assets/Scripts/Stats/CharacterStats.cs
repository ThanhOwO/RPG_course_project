using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength; //1 point => increase damamge by 1 and crit dmg by 1% 
    public Stat agility; //1 point => increase evasion by 1 and crit rate by 0.5
    public Stat intelligence; //1 point => increase magic by 1 and magic res by 3
    public Stat vitality; //1 point => increase health by 5

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance; // default value = 5%
    public Stat critPower; // default value = 150%

    [SerializeField] private int currentHealth; 
    public virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamge(CharacterStats _targetStats)
    {
        if(CanDogdeAttack(_targetStats))
            return;
        
        int totalDamage = damage.GetValue() + strength.GetValue();

        if(canCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        Debug.Log(_damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // Add death logic here
        Debug.Log("Dead");
    }

    private bool CanDogdeAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if(Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    
    private bool canCrit()
    {
        float totalCritChance = critChance.GetValue() + (agility.GetValue() * 0.5f);

        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }

        return false;
    }

    private int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    
}
