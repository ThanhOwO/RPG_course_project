using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [Header("Clone attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAtkUnlockButton;
    [SerializeField] private float cloneMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggressive clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneMultiplier;
    public bool canApplyOnHitEffect { get; private set; }


    [Header("Multiple clone")]
    [SerializeField] private UI_SkillTreeSlot multipleCloneUnlockButton;
    [SerializeField] private float multipleCloneMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAtkUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
    }

    #region Unlock region

    protected override void CheckUnlock()
    {
        UnlockAggressiveClone();
        UnlockCloneAttack();
        UnlockMultipleClone();
        UnlockCrystalInsteadOfClone();
    }

    private void UnlockCloneAttack()
    {
        if(cloneAtkUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneMultiplier;
        }
    }

    private void UnlockAggressiveClone()
    {
        if(aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneMultiplier;
        }
    }

    private void UnlockMultipleClone()
    {
        if(multipleCloneUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multipleCloneMultiplier;
        }
    }

    private void UnlockCrystalInsteadOfClone()
    {
        if(crystalInsteadUnlockButton.unlocked)
            crystalInsteadOfClone = true;
    }

    #endregion

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if(crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(2 * player.FacingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
            CreateClone(_transform, _offset);
    }
}
