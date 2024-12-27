using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private RectTransform myTransform;
    private CharacterStats myStats;
    [SerializeField] private Slider topSlider; //health slider
    [SerializeField] private Slider bottomSlider; //damage slider
    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 0.5f;
    private float damagedHealthShrinkTimer;
    [SerializeField] private float shrinkSpeed;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        topSlider = transform.Find("TopSlider").GetComponent<Slider>();
        bottomSlider = transform.Find("BottomSlider").GetComponent<Slider>();
        myStats = GetComponentInParent<CharacterStats>();
        
        entity.onFlipped += FlipUI; //When the onFlipped happens we add an call the FlipUI function
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
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI; 
    } 
    
}
