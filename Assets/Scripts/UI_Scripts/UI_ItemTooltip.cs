using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private RectTransform tooltipRectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector2 offset = new Vector2(10f, -10f);
    private ItemData_Equipment currentItem;

    private void Awake()
    {
        if (tooltipRectTransform == null)
            tooltipRectTransform = GetComponent<RectTransform>();

        DisableRaycastTarget();
    }

    private void Update()
    {
        // Update position only when tooltip is visible
        if (currentItem != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 tooltipPosition = mousePosition + offset;

            // Make sure the tooltip doesn't go beyond the screen
            Vector2 screenBounds = new Vector2(Screen.width, Screen.height);
            Vector2 tooltipSize = tooltipRectTransform.sizeDelta * canvas.scaleFactor;

            if (tooltipPosition.x + tooltipSize.x > screenBounds.x) // Check the right edge
                tooltipPosition.x = screenBounds.x - tooltipSize.x;
            if (tooltipPosition.y - tooltipSize.y < 0) // Check the below edge
                tooltipPosition.y = tooltipSize.y;

            tooltipRectTransform.position = tooltipPosition;
        }
    }

    public void ShowTooltip(ItemData_Equipment _item)
    {
        if(_item == null)
            return;
        
        currentItem = _item;
        
        itemNameText.text = _item.itemName;
        itemTypeText.text = _item.itemType.ToString();
        itemDescription.text = _item.GetDescription();

        gameObject.SetActive(true);

    }

    public void HideTooltip()
    {
        currentItem = null;
        gameObject.SetActive(false);
    }

    private void DisableRaycastTarget()
    {
        // Disable Raycast Target for all elements in tooltip
        foreach (var graphic in GetComponentsInChildren<UnityEngine.UI.Graphic>())
        {
            graphic.raycastTarget = false;
        }
    }
}
