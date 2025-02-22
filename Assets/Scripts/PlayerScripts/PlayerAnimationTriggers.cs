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
        AudioManager.instance.PlaySFX(2, null); //atk sound effect
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
                    HitStopFX.instance.StopTime(0.1f, 0.05f);
                    
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
}
