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
    private CharacterStats myStats;
    private int facingDir = 1;
    private new Transform transform;

    void Start() {
        anim = GetComponentInChildren<Animator>();
        transform = GetComponent<Transform>();
    }

    private void Update() 
    {
        if(canMove)
        {
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocityY);
            anim.SetBool("Spin", true);
        }

        if(facingDir == 1 && rb.linearVelocityX < 0)
        {
            facingDir = -1;
            transform.Rotate(0, 180, 0);
        } 
    }

    public void SetupArrow(float _speed, CharacterStats _myStats)
    {
        xVelocity = _speed;
        myStats = _myStats;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            myStats.DoDamge(other.GetComponent<CharacterStats>());
            StuckInto(other);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            StuckInto(other);

    }

    public void ParriedArrow()
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
