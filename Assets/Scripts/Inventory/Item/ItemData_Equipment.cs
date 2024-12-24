using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory,
    Flask,
    
    //add more equipment types here
}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Major stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive stats")]
    public int damage;
    public int critChance;
    public int critPower;

    [Header("Defensive stats")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicRes;

    [Header("Magic stats")]
    public int fireDmg;
    public int iceDmg;
    public int lightningDmg;
    public int poisonDmg;

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicRes.AddModifier(magicRes);

        playerStats.fireDmg.AddModifier(fireDmg);
        playerStats.iceDmg.AddModifier(iceDmg);
        playerStats.lightningDmg.AddModifier(lightningDmg);
        playerStats.poisonDmg.AddModifier(poisonDmg);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicRes.RemoveModifier(magicRes);

        playerStats.fireDmg.RemoveModifier(fireDmg);
        playerStats.iceDmg.RemoveModifier(iceDmg);
        playerStats.lightningDmg.RemoveModifier(lightningDmg);
        playerStats.poisonDmg.RemoveModifier(poisonDmg);

    }
}
