using UnityEngine;

public abstract class UI_Tooltip : MonoBehaviour
{
    [SerializeField] protected RectTransform tooltipRectTransform;
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Vector2 offset = new Vector2(10f, -10f);

    protected virtual void Awake()
    {
        if (tooltipRectTransform == null)
            tooltipRectTransform = GetComponent<RectTransform>();

        DisableRaycastTarget();
    }

    protected virtual void Update() 
    {
        if (tooltipRectTransform.gameObject.activeSelf)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 tooltipPosition = mousePosition + offset;

            // Make sure the tooltip doesn't go beyond the screen
            Vector2 screenBounds = new Vector2(Screen.width, Screen.height);
            Vector2 tooltipSize = tooltipRectTransform.sizeDelta * canvas.scaleFactor;

            if (tooltipPosition.x + tooltipSize.x > screenBounds.x) // Check the right edge
                tooltipPosition.x = mousePosition.x - tooltipSize.x + 120f;
        
            if (tooltipPosition.y - tooltipSize.y < 0) // Check the below edge
                tooltipPosition.y = tooltipSize.y;

            tooltipRectTransform.position = tooltipPosition;
        }
    }

    public abstract void ShowTooltip(params object[] args);
    public virtual void HideTooltip() => gameObject.SetActive(false);

    private void DisableRaycastTarget()
    {
        // Disable Raycast Target for all elements in tooltip
        foreach (var graphic in GetComponentsInChildren<UnityEngine.UI.Graphic>())
        {
            graphic.raycastTarget = false;
        }
    }
}
