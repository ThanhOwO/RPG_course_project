using System.Collections;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;
    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private bool canDuplicateClone;
    private int facingDir = 1;
    private float chanceToDuplicate;

    [Space]
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float closestEnemyCheckRadius = 25;
    [SerializeField] private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        StartCoroutine(FaceClosestTarget());
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1,1,1,sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            if(sr.color.a <= 0)
                Destroy(gameObject);
        }
        
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, bool _canDuplicate, float _chanceToDuplicate, Player _player, float _atkMult)
    {
        if(_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1,3));
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        attackMultiplier = _atkMult;
        player = _player;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;

    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockBackDir(transform);

                //player.stats.DoDamge(hit.GetComponent<CharacterStats>());

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDamage(enemyStats, attackMultiplier);

                if(player.skill.clone.canApplyOnHitEffect)
                {
                    Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(hit.transform);       
                }

                if(canDuplicateClone)
                {
                    if(Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private IEnumerator FaceClosestTarget()
    {
        yield return null; //Wait for the next frame to make sure the closest enemy is found

        FindClosestEnemy();

        if(closestEnemy!= null)
        {
            if(transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0,180,0);
            }
        }
    }

    private void FindClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, closestEnemyCheckRadius, whatIsEnemy);
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
            if(distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = hit.transform;
            }
        }
    }
}
