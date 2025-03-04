using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(0, null); //atk sound effect
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if(_target != null)
                    player.stats.DoDamge(_target); 

                if(player.primaryAttack.IsLastComboAtk() && !_target.isDead)
                    hit.GetComponent<Enemy>().Stagger();

                if(!_target.isInvincible)
                {
                    HitStopFX.instance.StopTime(0.05f, 0.05f);
                    AudioManager.instance.PlaySFX(9, null); //hit sound effect
                }

                //inventory get weapon call item effect 
                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);       
            }
        }
    }
    private void ThrowSword()
    {
        if(!player.sword)
        {
            SkillManager.instance.sword.CreateSword();
            return;
        }
    }

    #region healing regions
    private void Healing()
    {
        if (player.flaskSpritePrefab != null)
        {
            GameObject flask = Instantiate(player.flaskSpritePrefab, player.flaskSpawnPoint.position, Quaternion.identity);
            Destroy(flask, 0.7f);
        }
        Inventory.instance.UseFlask();
    }

    private void EmptyFlask()
    {
        if (player.emptyPotionPrefab != null)
        {
            GameObject emptyPotion = Instantiate(player.emptyPotionPrefab, player.transform.position, Quaternion.identity);

            Rigidbody2D rb = emptyPotion.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float forceX = Random.Range(-1f, 1f);
                float forceY = Random.Range(0f, -1f);
                rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
            }
            Destroy(emptyPotion, 3f);
        }
    }
    #endregion

    #region SFX regions
    private void PlayFootstep()
    {
        AudioManager.instance.PlaySFX(4, null);
    }
    private void PlayGrabSfx()
    {
        AudioManager.instance.PlaySFX(17, null);
    }
    #endregion

}
