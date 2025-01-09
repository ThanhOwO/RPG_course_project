using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null)
            return;

        ui.itemTooltip.ShowTooltip(item.data as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item == null)
            return;
            
        ui.itemTooltip.HideTooltip();
    }

}
