using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked {get; private set;}

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    public bool restoreUnlocked {get; private set;}   
    [Range(0f, 1f)]
    [SerializeField] private float restorePercentHP;

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryMirageUnlockButton;
    public  bool parryMirageUnlocked {get; private set;}

    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryMirage);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if(restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restorePercentHP);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    private void UnlockParry()
    {
        if(parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockParryRestore()
    {
        if(restoreUnlockButton.unlocked)
            restoreUnlocked = true;
    }

    private void UnlockParryMirage()
    {
        if(parryMirageUnlockButton.unlocked)
            parryMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if(parryMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryMirage();
        UnlockParryRestore();
    }
}
