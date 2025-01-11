using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;
    protected Player player;

    protected virtual void Start() {
        player = PlayerManager.instance.player;

        Invoke(nameof(CheckUnlock), .1f);
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        Debug.Log($"Skill - Cooldown Timer: {cooldownTimer}, Cooldown: {cooldown}");
        if (player.isDead)
            return false;
        
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            Debug.Log($"kill used. Cooldown Timer reset to {cooldown}");
            return true;
        }

        Debug.Log("Skill is on cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        //Do some skill specific stuff
    }

    protected virtual void CheckUnlock()
    {

    }

    //Function to find the closest enemy
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
