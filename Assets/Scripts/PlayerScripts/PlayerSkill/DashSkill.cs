using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked {get; private set;}
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;

    [Header("Clone on dash")]
    public bool cloneOnDashUnlocked {get; private set;}
    [SerializeField] private UI_SkillTreeSlot cloneDashUnlockButton;


    [Header("Clone on arrival")]
    public bool cloneOnArrivalUnlocked {get; private set;}
    [SerializeField] private UI_SkillTreeSlot cloneArrivalUnlockButton;

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }


    public override void UseSkill()
    {
        base.UseSkill();
    }

    private void UnlockDash()
    {
        if(dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if(cloneDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        if(cloneArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }

    public void CloneOnDash()
    {
        if(cloneOnDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

    public void CloneOnArrival()
    {
        if(cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
}
