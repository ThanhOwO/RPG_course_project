using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<ItemData> startingEquipment;
    public List<InventoryItem> inventory; //Main Inventory 
    public Dictionary<ItemData, InventoryItem> inventoryDictionary; //used for searching inventory
    public List<InventoryItem> stash; //Material Inventory
    public Dictionary<ItemData, InventoryItem> stashDictionary;
    public List<InventoryItem> equipment; //Equipment Inventory
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Item Cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;
    private float flaskCooldown;
    private float armorCooldown;

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>(); //value of items in inventory
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>(); //contain key and value of item
        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItem();
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        //Check each items in equipment dictionary
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if(item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }

        if(oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }
        
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);
        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment _oldEquipment)
    {
        if(equipmentDictionary.TryGetValue(_oldEquipment, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(_oldEquipment);
            _oldEquipment.RemoveModifiers();
        }
    }

    public void UpdateSlotUI()
    {
        for(int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].ClearSlot();
        }
        for(int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].ClearSlot();
        }


        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
        for(int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if(item.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }
        for(int i = 0; i < statSlot.Length; i++) //Update info of stats in char UI
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if(_item.itemType == ItemType.Equipment && CanAddItem())
            AddToInventory(_item);
        else if(_item.itemType == ItemType.Material)
            AddToStash(_item);
        
        UpdateSlotUI();
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if(value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if(stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if(stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }

        UpdateSlotUI();
    }

    public void AddToInventory(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void AddToStash(ItemData _item)
    {
        if(stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsUsed = new List<InventoryItem>();

        for(int i = 0; i < _requiredMaterials.Count; i++)
        {
            if(stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                //add this to used material
                if(stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("Not enough materials");
                    return false;
                }
                else
                {
                    materialsUsed.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("Not enough materials");
                return false;
            }
        }

        for(int i = 0; i < materialsUsed.Count; i++)
        {
            RemoveItem(materialsUsed[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("Crafted " + _itemToCraft.name);

        return true;
    }

    public bool CanAddItem()
    {
        if(inventory.Count >= inventoryItemSlot.Length)
            return false;
        
        return true;
    }

    private void AddStartingItem()
    {
        for(int i = 0; i < startingEquipment.Count; i++)
        {
            AddItem(startingEquipment[i]);
        }
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public List<InventoryItem> GetInventoryList() => inventory;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipmentItem = null;

        //Check each items in equipment dictionary
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if(item.Key.equipmentType == _type)
                equipmentItem = item.Key;
        }

        return equipmentItem;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);
        
        if(currentFlask != null)
        {
            bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

            if(canUseFlask)
            {
                InventoryItem tempItem = null;
                InventoryItem equipItem = equipmentSlot[3].item;

                //Function that calculates the consumed flask
                if(inventoryDictionary.TryGetValue(currentFlask, out InventoryItem value))
                {
                    List<InventoryItem> itemToCheck = GetInventoryList();
                    for( int i = 0; i < itemToCheck.Count ; i++)
                    {
                        if(itemToCheck[i] == value)
                        {
                            tempItem = itemToCheck[i];
                        }
                    }

                    if(tempItem != null)
                    {
                        if(tempItem.stackSize > 1)
                        {
                            tempItem.stackSize--;
                            UpdateSlotUI();
                        }
                        else
                        {
                            inventoryDictionary.Remove(currentFlask);
                            inventory.Remove(tempItem);
                            UpdateSlotUI();
                        }
                    }
                }
                else
                {
                    equipment.Remove(equipItem);
                    equipmentDictionary.Remove(currentFlask);
                    equipmentSlot[3].ClearSlot();
                    inventoryDictionary.Remove(currentFlask);
                }
                
                flaskCooldown = currentFlask.itemCooldown;
                currentFlask.Effect(null);
                lastTimeUsedFlask = Time.time;
            }
        }
    }

    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);

        if(Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        return false;
    }

}
