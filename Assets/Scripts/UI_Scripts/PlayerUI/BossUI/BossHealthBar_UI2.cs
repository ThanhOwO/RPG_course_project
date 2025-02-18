using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar_UI2 : MonoBehaviour
{
    private CharacterStats myStats;
    [SerializeField] private Image topFillImage; // Health bar
    [SerializeField] private Image bottomFillImage; // Damage delay bar
    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 1f;
    private float damagedHealthShrinkTimer;
    [SerializeField] private float shrinkSpeed;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        ShrinkDamageBar();
    }

    public void SetBoss(CharacterStats bossStats)
    {
        // Unsubscribe from the previous boss if there was one
        if (myStats != null)
        {
            myStats.onHealthChanged -= UpdateHealthUI;
        }

        myStats = bossStats;
        myStats.onHealthChanged += UpdateHealthUI;

        gameObject.SetActive(true);

        UpdateHealthUI();
        bottomFillImage.fillAmount = topFillImage.fillAmount;
    }

    private void UpdateHealthUI()
    {
        if (myStats == null || myStats.GetMaxHealthValue() <= 0)
        {
            Debug.LogWarning("Invalid health values detected.");
            return;
        }

        Debug.Log($"Boss Current Health: {myStats.currentHealth} / {myStats.GetMaxHealthValue()}");

        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;

        float healthPercent = Mathf.Clamp01((float)myStats.currentHealth / myStats.GetMaxHealthValue());
        Debug.Log($"Health Percent Calculated: {healthPercent}");

        topFillImage.fillAmount = healthPercent;

        if (bottomFillImage.fillAmount < topFillImage.fillAmount)
            bottomFillImage.fillAmount = topFillImage.fillAmount;
    }

    private void ShrinkDamageBar()
    {
        if (bottomFillImage.fillAmount > topFillImage.fillAmount)
        {
            if (damagedHealthShrinkTimer > 0)
                damagedHealthShrinkTimer -= Time.deltaTime;
            else
                bottomFillImage.fillAmount = Mathf.Lerp(bottomFillImage.fillAmount, topFillImage.fillAmount, shrinkSpeed * Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        if (myStats != null)
            myStats.onHealthChanged -= UpdateHealthUI;
    }

    public void HideBossHealthBar()
    {
        gameObject.SetActive(false);
        if (myStats != null)
        {
            myStats.onHealthChanged -= UpdateHealthUI;
            myStats = null;
        }
    }
}
