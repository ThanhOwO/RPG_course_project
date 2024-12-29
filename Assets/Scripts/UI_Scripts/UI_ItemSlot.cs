using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if(item != null)
        {
            itemImage.sprite = item.data.icon;

            if(item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    //Clear the slot when the item is removed
    public void ClearSlot()
    {
        item = null;
        itemImage.color = Color.clear;
        itemImage.sprite = null;
        itemText.text = "";
    }

    //Click on the item slot to equip the equipment item
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(item == null)
            return;
            
        //Hold Ctrl + click to remove items
        if(Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        //Click to equip the equipment item
        if(item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }
    }
}
