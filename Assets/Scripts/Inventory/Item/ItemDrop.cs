using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int dropAmount;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();
    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        if(possibleDrop.Length == 0)
        {
            Debug.Log("Item pool is empty. No item to drop");
            return;
        }

        foreach(ItemData item in possibleDrop)
        {
            if(item != null && Random.Range(0, 100) < item.dropChance)
                dropList.Add(item);
        }

        for(int i = 0; i < dropAmount; i++)
        {
            if(dropList.Count > 0)
            {
                int randomIndex = Random.Range(0, dropList.Count);
                ItemData itemToDrop = dropList[randomIndex];

                DropItem(itemToDrop);
                dropList.Remove(itemToDrop);
            }
        }
    }

    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVeclocity = new Vector2(Random.Range(-4,4), Random.Range(15,20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVeclocity);
    }
}
