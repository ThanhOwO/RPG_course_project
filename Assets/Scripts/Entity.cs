using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim {get; private set;}
    public Rigidbody2D rb {get; private set;}
    public EntityFx fx {get; private set;}
    public SpriteRenderer sr;
    public CharacterStats stats {get; private set;}
    public CapsuleCollider2D cd {get; private set;}
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public int knockbackDir {get; private set;}

    public int FacingDir {get; private set;} = 1;
    protected bool FacingRight = true;
    public System.Action onFlipped;
    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFx>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }
    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {

    }
    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }
    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }
    public virtual void SetupKnockBackDir(Transform _damageDirection)
    {
        if(_damageDirection.position.x > transform.position.x)
            knockbackDir = -1;
        else if(_damageDirection.position.x < transform.position.x)
            knockbackDir = 1;
    }
    public virtual void DamageImpact() => StartCoroutine("HitKnockBack");

    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;
        rb.linearVelocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos() {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion
    #region Flip
    public void Flip()
    {
        FacingDir *= -1;
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);

        if(onFlipped != null)
            onFlipped(); // onFlipped action happens when Flip function is called
    }
    public void FlipController(float _x)
    {
        if(_x > 0 && !FacingRight)
            Flip();
        else if(_x < 0 && FacingRight)
            Flip();
    }
    #endregion
    #region Velocity
    public void setVelocity(float _xVelocity, float _yVelocity)
    {
        if(isKnocked)
            return;
        
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    public void zeroVelocity()
    {
        if(isKnocked)
            return;
        
        rb.linearVelocity = new Vector2(0, 0);
    } 
        
    #endregion



    public virtual void Die()
    {

    }

}
