using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float cloneAttackCooldown;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackHolePrefab);
        
        Blackhole_Skill_Controller newBlackHoleScript = newBlackhole.GetComponent<Blackhole_Skill_Controller>();

        newBlackHoleScript.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, cloneAttackCooldown);
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
