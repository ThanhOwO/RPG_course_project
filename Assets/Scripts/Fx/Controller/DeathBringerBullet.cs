using UnityEngine;

public class DeathBringerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector2 direction;
    private CharacterStats myStats;
    [SerializeField] private GameObject hitFxPrefab;

    public void Setup(Vector2 _direction, CharacterStats _myStats)
    {
        direction = _direction.normalized;
        myStats = _myStats;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool hit = false;

        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            hit = true;
        
        if(other.GetComponent<Player>())
        {
            other.GetComponent<Entity>().SetupKnockBackDir(transform);
            if(!myStats.isInvincible)
            {
                myStats.DoDamge(other.GetComponent<CharacterStats>());
                other.GetComponent<Player>().Stagger();
            }
            hit = true;
        }
        
        if(hit)
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            if(hitFxPrefab != null)
            {
                GameObject hitFx = Instantiate(hitFxPrefab, hitPoint, Quaternion.identity);
                Destroy(hitFx, 0.5f);
            }
            Destroy(gameObject);
        }
    }
}
