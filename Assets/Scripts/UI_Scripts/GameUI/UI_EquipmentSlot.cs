using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate() 
    {
        gameObject.name = "Equipment slot -" + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;

        // Unequip item
        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
        ClearSlot();
    }

    public override void UpdateSlot(InventoryItem _newItem)
    {
        base.UpdateSlot(_newItem);

        if (item != null && item.data is ItemData_Equipment equipmentItem && equipmentItem.equipmentType == EquipmentType.Flask)
        {
            // Get the actual number of Flasks from Inventory
            if (Inventory.instance != null && Inventory.instance.inventoryDictionary.TryGetValue(item.data, out InventoryItem inventoryFlask))
            {
                itemText.text = inventoryFlask.stackSize.ToString();
            }
        }
    }

}
