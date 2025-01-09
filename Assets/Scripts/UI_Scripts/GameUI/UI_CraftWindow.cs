using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners();

        for(int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for(int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            if(_data.craftingMaterials.Count > materialImage.Length)
                Debug.LogWarning("More materials than available slot in craft window");

            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;
            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;

        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.name;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftingMaterials)) ;
    }
    
}
