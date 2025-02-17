using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar_UI : MonoBehaviour
{
    private CharacterStats myStats;
    [SerializeField] private Slider topSlider; //health slider
    [SerializeField] private Slider bottomSlider; //damage slider
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
        bottomSlider.value = topSlider.value;
    }

    private void UpdateHealthUI()
    {
        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
        topSlider.maxValue = myStats.GetMaxHealthValue();
        bottomSlider.maxValue = myStats.GetMaxHealthValue();

        topSlider.value = myStats.currentHealth;

        if (bottomSlider.value < topSlider.value)
            bottomSlider.value = topSlider.value;
    }

    private void ShrinkDamageBar()
    {
        if(bottomSlider.value > topSlider.value)
        {
            if(damagedHealthShrinkTimer > 0)
                damagedHealthShrinkTimer -= Time.deltaTime;
            else
                bottomSlider.value = Mathf.Lerp(bottomSlider.value, topSlider.value, shrinkSpeed * Time.deltaTime);
        }
    }

    private void OnDisable()
    {
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
