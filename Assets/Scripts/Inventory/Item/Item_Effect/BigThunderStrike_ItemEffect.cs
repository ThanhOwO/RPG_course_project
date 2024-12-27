using UnityEngine;

[CreateAssetMenu(fileName = "Big thunder striker effect", menuName = "Data/Item effect/Big thunder strike")]
public class BigThunderStrike_ItemEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike, 0.3f);
    }
}
