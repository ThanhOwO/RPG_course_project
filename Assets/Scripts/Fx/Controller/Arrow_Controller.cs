using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;
    private Animator anim;

    void Start() {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update() 
    {
        if(canMove)
        {
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocityY);
            anim.SetBool("Spin", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            other.GetComponent<CharacterStats>()?.TakeDamage(damage);
            StuckInto(other);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            StuckInto(other);

    }

    public void FlipArrow()
    {
        if(flipped)
            return;

        xVelocity = xVelocity * -1;
        flipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }

    private void StuckInto(Collider2D other)
    {
        anim.SetBool("Spin", false);
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = other.transform;
        Destroy(gameObject, 10f);
    }
}
