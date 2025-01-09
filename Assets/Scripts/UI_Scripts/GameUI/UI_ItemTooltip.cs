using TMPro;
using UnityEngine;

public class UI_ItemTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    private ItemData_Equipment currentItem;

    public override void ShowTooltip(params object[] args)
    {
        if(args.Length == 0 || args[0] is not ItemData_Equipment _item)
            return;

        currentItem = _item;

        itemNameText.text = _item.itemName;
        itemTypeText.text = _item.itemType.ToString();
        itemDescription.text = _item.GetDescription();

        gameObject.SetActive(true);
    }

    public override void HideTooltip()
    {
        currentItem = null;
        base.HideTooltip();
    }

}
