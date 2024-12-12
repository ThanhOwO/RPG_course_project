using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    Blackhole_Skill_Controller currentBlackhole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        
        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, cloneAttackCooldown, blackholeDuration);

        player.playerIsInBlackHole = true;
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if(!currentBlackhole)
            return false;
        
        if(currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }
}
