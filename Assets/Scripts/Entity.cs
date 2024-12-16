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
    [SerializeField] protected Vector2 knockbackDirection;
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

    public int FacingDir {get; private set;} = 1;
    protected bool FacingRight = true;
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
    public virtual void DamageEffect()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack");
        Debug.Log(gameObject.name + "  was damaged!");
    }
    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;
        rb.linearVelocity = new Vector2(knockbackDirection.x * -FacingDir, knockbackDirection.y);
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

    public void makeTransparent(bool _transparent)
    {
        if(_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    public virtual void Die()
    {

    }

}
