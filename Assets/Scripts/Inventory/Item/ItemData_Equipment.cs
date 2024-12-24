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
}
