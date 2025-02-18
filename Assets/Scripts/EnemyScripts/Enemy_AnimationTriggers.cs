using UnityEngine;

public class Enemy_AnimationTriggers : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                if(!_target.isInvincible)
                {
                    enemy.stats.DoDamge(_target);
                    hit.GetComponent<Player>().Stagger();
                }
            }
        }
    }
    private void SpecialAttackTrigger()
    {
        enemy.AnimationSpecialAttackTrigger();
    }
    protected void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    protected void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
