using UnityEngine;
using UnityEngine.UI;


public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked {get; private set;}

    [Header("Mirage dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool mirageDodgeUnlocked {get; private set;}

    protected override void Start()
    {
        base.Start();
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);

    }

    private void UnlockDodge()
    {
        if(unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if(unlockMirageDodgeButton.unlocked)
            mirageDodgeUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if(mirageDodgeUnlocked)
        {
            var enemyTransform = FindClosestEnemy(player.transform);
            SkillManager.instance.clone.CreateClone(enemyTransform, new Vector3((enemyTransform.position.x - player.transform.position.x) * .3f, + player.transform.position.y - enemyTransform.position.y));
        }
    }
}
