using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar_UI : MonoBehaviour
{
    private RectTransform myTransform;
    private PlayerStats myStats;
    [SerializeField] private Slider topSlider; //health slider
    [SerializeField] private Slider bottomSlider; //damage slider
    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 0.5f;
    private float damagedHealthShrinkTimer;
    [SerializeField] private float shrinkSpeed;

    private void Start()
    {
        myStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        topSlider = transform.Find("TopSlider").GetComponent<Slider>();
        bottomSlider = transform.Find("BottomSlider").GetComponent<Slider>();
        
        
        myStats.onHealthChanged += UpdateHealthUI;
        UpdateHealthUI();

        bottomSlider.value = topSlider.value;
    }

    private void Update() 
    {
        ShrinkDamageBar();
    }

    private void UpdateHealthUI()
    {
        // Alex's Udemy code
        // slider.maxValue = myStats.GetMaxHealthValue();
        // slider.value = myStats.currentHealth;;

        //My code
        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
        topSlider.maxValue = myStats.GetMaxHealthValue();
        bottomSlider.maxValue = myStats.GetMaxHealthValue();

        topSlider.value = myStats.currentHealth;

        //Update the bottomSlider when topSlider is increase
        if (bottomSlider.value < topSlider.value)
            bottomSlider.value = topSlider.value;
    }

    private void ShrinkDamageBar()
    {
        damagedHealthShrinkTimer -= Time.deltaTime;

        if(damagedHealthShrinkTimer < 0)
        {
            if(bottomSlider.value > topSlider.value)
            {
                bottomSlider.value -= shrinkSpeed * Time.deltaTime;
            }
        }
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        myStats.onHealthChanged -= UpdateHealthUI; 
    } 
}
