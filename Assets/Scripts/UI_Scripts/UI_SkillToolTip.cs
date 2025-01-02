using TMPro;
using UnityEngine;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private RectTransform tooltipRectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector2 offset = new Vector2(10f, -10f);

    private void Awake()
    {
        if (tooltipRectTransform == null)
            tooltipRectTransform = GetComponent<RectTransform>();

        DisableRaycastTarget();
    }

    private void Update()
    {
        // Update position only when tooltip is visible
        if (skillText != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 tooltipPosition = mousePosition + offset;

            // Make sure the tooltip doesn't go beyond the screen
            Vector2 screenBounds = new Vector2(Screen.width, Screen.height);
            Vector2 tooltipSize = tooltipRectTransform.sizeDelta * canvas.scaleFactor;

            if (tooltipPosition.x + tooltipSize.x > screenBounds.x) // Check the right edge
                tooltipPosition.x = mousePosition.x - tooltipSize.x + 70f;
        
            if (tooltipPosition.y - tooltipSize.y < 0) // Check the below edge
                tooltipPosition.y = tooltipSize.y;

            tooltipRectTransform.position = tooltipPosition;
        }
    }
    
    public void ShowTooltip(string _skillDescription, string _skillName)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescription;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        skillText.text = "";
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
