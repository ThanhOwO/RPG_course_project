using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager
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
    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Database")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;

    [HideInInspector] public int maxFlaskInInventory = 6;
    [HideInInspector] public int flaskStorage = 0;

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

    #region Item Management
    public void EquipItem(ItemData _item)
    {
        if (!(_item is ItemData_Equipment newEquipment)) return;

        // Nếu đã có item trong slot, chỉ cần thay thế
        if (equipmentDictionary.TryGetValue(newEquipment, out InventoryItem oldEquipment))
        {
            UnequipItem(oldEquipment.data as ItemData_Equipment);
        }

        // Thêm item vào equipment slot nhưng không xóa khỏi inventory
        InventoryItem newItem = new InventoryItem(newEquipment);
        equipment.Add(newItem);
        equipmentDictionary[newEquipment] = newItem;
        newEquipment.AddModifiers();

        UpdateSlotUI();
    }
    public void UnequipItem(ItemData_Equipment _oldEquipment)
    {
        if (equipmentDictionary.TryGetValue(_oldEquipment, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(_oldEquipment);
            _oldEquipment.RemoveModifiers();
        }

        UpdateSlotUI();
    }
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            if (_item is ItemData_Equipment equipmentItem && equipmentItem.equipmentType == EquipmentType.Flask)
            {
                AddFlaskToInventory(equipmentItem);
            }
            else if (CanAddItem())
            {
                AddToInventory(_item);
            }
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }

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

        UpdateStatUI();
    }
    public void UpdateStatUI()
    {
        for(int i = 0; i < statSlot.Length; i++) //Update info of stats in char UI
        {
            statSlot[i].UpdateStatValueUI();
        }
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
    
    #endregion
    
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        //Check if all required materials are available with the required quantity
        foreach(var requiredItem in _requiredMaterials)
        {
            if(stashDictionary.TryGetValue(requiredItem.data, out InventoryItem stashItem))
            {
                if(stashItem.stackSize < requiredItem.stackSize)
                {
                    Debug.Log("Not enough materials");
                    return false;
                }
            }
            else
            {
                Debug.Log("Material not found");
                return false;
            }
        }

        //If all materials are available remove them from stash

        foreach(var requiredItem in _requiredMaterials)
        {
            for(int i = 0; i < requiredItem.stackSize; i++)
            {
                RemoveItem(requiredItem.data);
            }
        }

        AddItem(_itemToCraft);
        Debug.Log("Crafted " + _itemToCraft.itemName);
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
        foreach(ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }

        if(loadedItems.Count > 0)
        {
            foreach(InventoryItem item in loadedItems)
            {
                for(int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }

        for(int i = 0; i < startingEquipment.Count; i++)
        {
            if(startingEquipment[i] != null)
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
    
    #region  Flask management
    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask != null && inventoryDictionary.TryGetValue(currentFlask, out InventoryItem flaskItem))
        {
            bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;
            if (canUseFlask && flaskItem.stackSize > 0)
            {
                flaskItem.stackSize--;
                flaskCooldown = currentFlask.itemCooldown;
                currentFlask.Effect(null);
                lastTimeUsedFlask = Time.time;

                UpdateSlotUI();
            }
        }
    }
    private void AddFlaskToInventory(ItemData_Equipment flaskItem)
    {
        int currentFlaskCount = inventoryDictionary.ContainsKey(flaskItem) ? inventoryDictionary[flaskItem].stackSize : 0;
        int totalFlask = currentFlaskCount + 1;

        if (totalFlask > maxFlaskInInventory)
        {
            flaskStorage += (totalFlask - maxFlaskInInventory);
            totalFlask = maxFlaskInInventory;
        }

        if (inventoryDictionary.ContainsKey(flaskItem))
        {
            inventoryDictionary[flaskItem].stackSize = totalFlask;
        }
        else
        {
            InventoryItem newItem = new InventoryItem(flaskItem);
            newItem.stackSize = totalFlask;
            inventory.Add(newItem);
            inventoryDictionary.Add(flaskItem, newItem);
        }
    }
    public void RefillFlask()
    {
        ItemData_Equipment flaskItem = null;

        // Find flask in Database
        foreach (var item in itemDataBase)
        {
            if (item is ItemData_Equipment equipmentItem && equipmentItem.equipmentType == EquipmentType.Flask)
            {
                flaskItem = equipmentItem;
                break;
            }
        }

        if (flaskItem == null || flaskStorage <= 0)
            return; // No Flask or Storage empty, no refill required

        // If Flask is not in inventory, add it again with stackSize = 0
        if (!inventoryDictionary.ContainsKey(flaskItem))
        {
            InventoryItem newFlask = new InventoryItem(flaskItem);
            newFlask.stackSize = 0;
            inventory.Add(newFlask);
            inventoryDictionary.Add(flaskItem, newFlask);
        }

        //Refill flask
        int currentFlask = inventoryDictionary[flaskItem].stackSize;
        int refillAmount = Mathf.Min(flaskStorage, maxFlaskInInventory - currentFlask);

        inventoryDictionary[flaskItem].stackSize += refillAmount;
        flaskStorage -= refillAmount;

        UpdateSlotUI();
    }

    #endregion
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
    
    #region Save and Load

    public void LoadData(GameData _data)
    {
        flaskStorage = _data.flaskStorage;
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach(var item in itemDataBase)
            {
                if(item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemID in _data.equipmentID)
        {
            foreach(var item in itemDataBase)
            {
                if(item != null && loadedItemID == item.itemID)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentID.Clear();
        _data.flaskStorage = flaskStorage;

        foreach(KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentID.Add(pair.Key.itemID);
        }
    }

    #endregion

#if UNITY_EDITOR
    [ContextMenu("Fill up item database")]
    private void FillItemDatabase() => itemDataBase = new List<ItemData>(GetItemDataBase());

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] {"Assets/Scripts/Inventory/Item/All_Item/"});

        foreach(string SOname in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOname);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDatabase.Add(itemData);
        }

        return itemDatabase;
    }
#endif

}
