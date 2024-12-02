using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller2 : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 50f;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool canRotate = true;
    private bool isReturning;

    [Header("Bounce info")]
    [SerializeField]private float bounceSpeed;
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> enemyTargets;
    private int targetIndex;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;

        anim.SetBool("Rotation", true);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
        enemyTargets = new List<Transform>();
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
    }

    private void BounceLogic()
    {
        //Make the sword boucing between enemy count in the list.
        if(isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);
            
            if(Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f)
            {
                targetIndex++;
                amountOfBounce--;

                if(amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                
                if(targetIndex >= enemyTargets.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(isReturning)
            return;

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
        
        StuckInto(collision);
        
    }

    private void StuckInto(Collider2D collision)
    {

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
}
