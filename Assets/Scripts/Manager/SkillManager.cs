using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public DashSkill dash {get; private set;}
    public CloneSkill clone {get; private set;}
    public SwordSkill sword {get; private set;}
    public Blackhole_Skill blackhole {get; private set;}

    private void Awake() {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() 
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        sword = GetComponent<SwordSkill>();
        blackhole = GetComponent<Blackhole_Skill>();
    }
}
