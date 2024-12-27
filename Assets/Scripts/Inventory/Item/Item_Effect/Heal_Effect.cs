using UnityEngine;

[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float healPercent;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        //player stats
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        //How much to heal
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        //heal
        playerStats.IncreaseHealthBy(healAmount);
    }
}
