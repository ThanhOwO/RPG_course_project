using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(CapsuleCollider2D))]

public class Npc : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    public NpcStateMachine stateMachine {get; private set;}
    public string lastAnimBoolName  {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new NpcStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public virtual RaycastHit2D IsPlayerDetected() 
    {
        RaycastHit2D playerDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 13, whatIsPlayer);
        RaycastHit2D wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 13, whatIsGround);

        if(wallDetected)
        {
            if(wallDetected.distance < playerDetected.distance)
                return default(RaycastHit2D);
            
        }
        return playerDetected;
    } 

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = Color.green;
        // Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + FacingDir, transform.position.y));
        //Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + agroDistance, transform.position.y));
    }

    public virtual void FacePlayer(Transform playerTransform)
    {
        if (playerTransform == null) return;

        float direction = playerTransform.position.x - transform.position.x;

        if ((direction > 0 && !FacingRight) || (direction < 0 && FacingRight))
        {
            Flip();
        }
    }
}
