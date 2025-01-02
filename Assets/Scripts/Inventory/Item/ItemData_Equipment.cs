using System.Collections.Generic;
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

    [Header("Unique effect")]
    public float itemCooldown;
    public ItemEffect[] itemEffect;
    [TextArea]
    public string itemEffectDescription;


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

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;

    public void Effect(Transform _enemyPosition)
    {
        foreach(var item in itemEffect)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

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

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Critical Chance");
        AddItemDescription(critPower, "Critical Power");

        AddItemDescription(maxHealth, "Max Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicRes, "Magic Resistance");

        AddItemDescription(fireDmg, "Fire Damage");
        AddItemDescription(iceDmg, "Ice Damage");
        AddItemDescription(lightningDmg, "Lightning Damage");
        AddItemDescription(poisonDmg, "Poison Damage");

        if(descriptionLength < 5)
        {
            for(int i = 0; i < descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        if(itemEffectDescription.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine(itemEffectDescription);
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if(_value != 0)
        {
            if(sb.Length > 0)
                sb.AppendLine();
            if(_value > 0)
                sb.Append("+" + _value + " " + _name);

            descriptionLength++;
        }
    }
}
