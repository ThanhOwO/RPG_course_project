using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private float crystalExistTimer;
    private bool canExplode;
    private float moveSpeed;
    private bool canMove; 
    private bool canGrow;
    private float growSpeed = 5;

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        moveSpeed = _moveSpeed;
        canMove = _canMove;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if ( crystalExistTimer < 0 )
        {
            FinishCrystal();
        }

        if(canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3,3), growSpeed * Time.deltaTime);
    }

    public void SelfDestroy() => Destroy(gameObject);

    public void FinishCrystal()
    {
        if(canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x * cd.radius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

}
