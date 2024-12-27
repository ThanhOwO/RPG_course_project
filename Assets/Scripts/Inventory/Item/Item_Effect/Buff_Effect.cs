using UnityEngine;


public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicRes,
    fireDmg,
    iceDmg,
    lightningDmg,
    poisonDmg
    // add more stats as needed
}

[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        if(buffType == StatType.strength)
            return stats.strength;
        else if(buffType == StatType.agility)
            return stats.agility;
        else if(buffType == StatType.intelligence)
            return stats.intelligence;
        else if(buffType == StatType.vitality)
            return stats.vitality;
        else if(buffType == StatType.damage)
            return stats.damage;
        else if(buffType == StatType.critChance)
            return stats.critChance;
        else if(buffType == StatType.critPower)
            return stats.critPower;
        else if(buffType == StatType.maxHealth)
            return stats.maxHealth;
        else if(buffType == StatType.armor)
            return stats.armor;
        else if(buffType == StatType.evasion)
            return stats.evasion;
        else if(buffType == StatType.magicRes)
            return stats.magicRes;
        else if(buffType == StatType.fireDmg)
            return stats.fireDmg;
        else if(buffType == StatType.iceDmg)
            return stats.iceDmg;
        else if(buffType == StatType.lightningDmg)
            return stats.lightningDmg;
        else if(buffType == StatType.poisonDmg)
            return stats.poisonDmg;
        
        return null;
    }
}
