using UnityEngine;

[CreateAssetMenu(fileName = "Small thunder striker effect", menuName = "Data/Item effect/Small thunder strike")]
public class ThunderStrike_ItemEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike, 0.3f);
    }
}
