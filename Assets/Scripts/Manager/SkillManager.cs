using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public DashSkill dash {get; private set;}
    public CloneSkill clone {get; private set;}
    public SwordSkill sword {get; private set;}
    public Blackhole_Skill blackhole {get; private set;}
    public CrystalSkill crystal {get; private set;}
    public ParrySkill parry {get; private set;}
    public DodgeSkill dodge {get; private set;}

    private void Awake() {

        if (instance != null && instance != this) 
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start() 
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        sword = GetComponent<SwordSkill>();
        blackhole = GetComponent<Blackhole_Skill>();
        crystal = GetComponent<CrystalSkill>();
        parry = GetComponent<ParrySkill>();
        dodge = GetComponent<DodgeSkill>();
    }
}
