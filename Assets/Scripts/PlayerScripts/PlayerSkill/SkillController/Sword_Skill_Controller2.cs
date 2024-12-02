using UnityEngine;

public class Sword_Skill_Controller2 : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 50f;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool canRotate = true;
    private bool isReturing;

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

    public void ReturnSword()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        isReturing = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.linearVelocity;
        
        if(isReturing)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, player.transform.position) < 1)
                player.ClearTheSword();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        anim.SetBool("Rotation", false);

        canRotate = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }
}
