using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrystalSkill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] private UI_SkillTreeSlot unlockCloneCrystalButton;
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked {get; private set;}

    [Header("Explosive Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMoveCrystalButton;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool canMoveToEnemy;

    [Header("Multiple Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMultipleCrystalButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCloneCrystal);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMoveCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMoveCrystal);
        unlockMultipleCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleCrystal);
    }

    #region Unlock skill

    protected override void CheckUnlock()
    {
        UnlockMultipleCrystal();
        UnlockMoveCrystal();
        UnlockExplosiveCrystal();
        UnlockCrystal();
        UnlockCloneCrystal();
    }
    private void UnlockCrystal()
    {
        if(unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }

    private void UnlockCloneCrystal()
    {
        if(unlockCloneCrystalButton.unlocked)
            cloneInsteadOfCrystal = true;
    }

    private void UnlockExplosiveCrystal()
    {
        if(unlockExplosiveButton.unlocked)
            canExplode = true;
    }

    private void UnlockMoveCrystal()
    {
        if(unlockMoveCrystalButton.unlocked)
            canMoveToEnemy = true;
    }

    private void UnlockMultipleCrystal()
    {
        if(unlockMultipleCrystalButton.unlocked)
            canUseMultiStacks = true;
    }
    #endregion

    public override bool CanUseSkill()
    {
        if(currentCrystal != null)
            UseSkill();

        return base.CanUseSkill();
    }
    
    public override void UseSkill()
    {
        base.UseSkill();

        if(canUseMultiCrystal())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if(canMoveToEnemy)
                return;
            
            Vector2 playerPos = player.transform.position;

            player.transform.position = currentCrystal.transform.position;

            currentCrystal.transform.position = playerPos;
            
            if(cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }


        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private bool canUseMultiCrystal()
    {
        if(canUseMultiStacks)
        {
            if(crystalLeft.Count > 0)
            {
                if(crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);
                
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if(crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }

        return false;
    }

    private void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for(int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if(cooldownTimer > 0)
            return;
        
        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
