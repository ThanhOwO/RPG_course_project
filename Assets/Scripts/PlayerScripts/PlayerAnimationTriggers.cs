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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                if(player.primaryAttack.IsLastComboAtk())
                    hit.GetComponent<Enemy>().Stagger();
                player.stats.DoDamge(_target); 

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
