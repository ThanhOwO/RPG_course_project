using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player drop")]
    [SerializeField] private float chanceToDropEquipment;
    [SerializeField] private float chanceToDropMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;
        //List of items to unequip
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToDrop = new List<InventoryItem>();

        //for each item we gonna check if should be dropped
        //Equipment drop
        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            //check if we should drop this item
            if (Random.Range(0, 100) <= chanceToDropEquipment)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for(int i = 0; i < itemsToUnequip.Count; i++)
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);

        //Material drop
        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToDropMaterials)
            {
                DropItem(item.data);
                materialsToDrop.Add(item);
            }
        }

        for(int i = 0; i < materialsToDrop.Count; i++)
            inventory.RemoveItem(materialsToDrop[i].data);
        
        inventory.UpdateSlotUI();

    }
}
