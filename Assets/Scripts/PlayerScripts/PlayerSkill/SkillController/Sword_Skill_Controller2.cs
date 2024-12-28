using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller2 : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool canRotate = true;
    private bool isReturning;
    private float freezeTimeDuration;
    private float returnSpeed = 50f;

    [Header("Pierce info")]
    private float pierceAmount;

    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTargets;
    private int targetIndex;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;
    private float spinDirection;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;
        freezeTimeDuration = _freezeTimeDuration;

        if(pierceAmount <= 0)
            anim.SetBool("Rotation", true);
        
        spinDirection = Mathf.Clamp(rb.linearVelocityX, -1, 1);

        Invoke("DestroyMe", 7f);
    }

    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        bounceSpeed = _bounceSpeed;
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;
        enemyTargets = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.linearVelocity;
        
        if(isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
        
    }

    private void BounceLogic()
    {
        //Make the sword boucing between enemy count in the list.
        if(isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);
            
            if(Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTargets[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                bounceAmount--;

                if(bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                
                if(targetIndex >= enemyTargets.Count)
                    targetIndex = 0;
            }
        }
    }

    private void SpinLogic()
    {
        if(isSpinning)
        {
            if(Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if(wasStopped)
            {
                spinTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if(spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer = -Time.deltaTime;

                if(hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach(var hit in colliders)
                    {
                        if(hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(isReturning)
            return;

        //Like the if statement below, if there are any collision to enemy, damaged them.
        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }
        
        SetupTargetsForBounce(collision);
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoDamge(enemy.GetComponent<CharacterStats>());
        enemy.FreezeTimeFor(freezeTimeDuration);

        ItemData_Equipment equipedAccessory = Inventory.instance.GetEquipment(EquipmentType.Accessory);

        if(equipedAccessory != null)
            equipedAccessory.Effect(transform);
    }

    private void StuckInto(Collider2D collision)
    {
        if(pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if(isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        //Make return for isBouncing here to make sure those code below won't work.
        if(isBouncing && enemyTargets.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        //Check if the collision with enemy not null, create a list to add them in the list
        if(collision.GetComponent<Enemy>() != null)
        {
            if(isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach(var hit in colliders)
                {
                    if(hit.GetComponent<Enemy>() != null)
                        enemyTargets.Add(hit.transform);
                }
            }
        }
    }
}
