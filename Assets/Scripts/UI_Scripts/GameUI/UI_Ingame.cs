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

    private SkillManager skills;
    
    void Start()
    {
        skills = SkillManager.instance;

    }

    void Update()
    {
        UpdateSoulsUI();

        if(Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
            SetCooldown(dashImage);
        if(Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
            SetCooldown(parryImage);
        if(Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
            SetCooldown(crystalImage);
        if(Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
            SetCooldown(blackholeImage);
        if(Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldown(flaskImage);
        
        CheckCooldown(dashImage, skills.dash.cooldown);
        CheckCooldown(parryImage, skills.parry.cooldown);
        CheckCooldown(crystalImage, skills.crystal.cooldown);
        CheckCooldown(blackholeImage, skills.blackhole.cooldown);
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
}
