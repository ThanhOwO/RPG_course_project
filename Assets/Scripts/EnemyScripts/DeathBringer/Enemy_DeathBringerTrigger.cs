using UnityEngine;

public class Enemy_DeathBringerTrigger : Enemy_AnimationTriggers
{
    private Enemy_DeathBringer enemyDeathBringer => GetComponentInParent<Enemy_DeathBringer>();

    private void Relocate() => enemyDeathBringer.FindPositon();
    
    private void MakeInvisible() => enemyDeathBringer.fx.makeTransparent(true);
    private void MakeVisible() => enemyDeathBringer.fx.makeTransparent(false);

    private void AttackSFX() => AudioManager.instance.PlaySFX(18, null);
    private void MoveSFX() => AudioManager.instance.PlaySFX(19, null);

}
