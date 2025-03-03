using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    [Header("Skill layouts")]
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;

    [Header("Flask Info")]
    [SerializeField] private TextMeshProUGUI flaskCountText;

    [Header("Equipment Slots")]
    [SerializeField] private Image weaponSlot;
    [SerializeField] private Image armorSlot;
    [SerializeField] private Image accessorySlot;
    [SerializeField] private Image flaskSlot;

    [Header("Default Equipment Sprites")]
    [SerializeField] private Sprite defaultWeaponSprite;
    [SerializeField] private Sprite defaultArmorSprite;
    [SerializeField] private Sprite defaultAccessorySprite;
    [SerializeField] private Sprite defaultFlaskSprite;

    private SkillManager skills;
    
    void Start()
    {
        skills = SkillManager.instance;

    }

    void Update()
    {
        UpdateSoulsUI();
        UpdateFlaskUI();
        UpdateEquipmentUI();

        // if(Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
        //     SetCooldown(dashImage);
        // if(Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
        //     SetCooldown(parryImage);
        // if(Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
        //     SetCooldown(crystalImage);
        // if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
        //     SetCooldown(blackholeImage);
        if(Input.GetKeyDown(KeyCode.R) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldown(flaskImage);
        
        // CheckCooldown(dashImage, skills.dash.cooldown);
        // CheckCooldown(parryImage, skills.parry.cooldown);
        // CheckCooldown(crystalImage, skills.crystal.cooldown);
        // CheckCooldown(blackholeImage, skills.blackhole.cooldown);
        CheckCooldown(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void SetCooldown(Image _image)
    {
        if(_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldown(Image _image, float _cooldown)
    {
        if(_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

    private void UpdateSoulsUI()
    {
        if(soulsAmount < PlayerManager.instance.GetCurrentCurrency())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.GetCurrentCurrency();

        currentSouls.text = ((int)soulsAmount).ToString("N0");
    }

    private void UpdateFlaskUI()
    {
        ItemData_Equipment flask = Inventory.instance.GetEquipment(EquipmentType.Flask);
        if (flask != null && Inventory.instance.inventoryDictionary.TryGetValue(flask, out InventoryItem flaskItem))
        {
            flaskCountText.text = flaskItem.stackSize.ToString();
        }
        else
        {
            flaskCountText.text = "";
        }
    }

    public void UpdateEquipmentUI()
    {
        weaponSlot.sprite = GetEquipmentSprite(EquipmentType.Weapon);
        armorSlot.sprite = GetEquipmentSprite(EquipmentType.Armor);
        accessorySlot.sprite = GetEquipmentSprite(EquipmentType.Accessory);
        flaskSlot.sprite = GetEquipmentSprite(EquipmentType.Flask);
    }

    private Sprite GetEquipmentSprite(EquipmentType type)
    {
        ItemData_Equipment equippedItem = Inventory.instance.GetEquipment(type);
        
        if (equippedItem != null)
            return equippedItem.icon;

        // Nếu không có item equip, trả về sprite mặc định
        return type switch
        {
            EquipmentType.Weapon => defaultWeaponSprite,
            EquipmentType.Armor => defaultArmorSprite,
            EquipmentType.Accessory => defaultAccessorySprite,
            EquipmentType.Flask => defaultFlaskSprite,
            _ => null
        };
    }

}
