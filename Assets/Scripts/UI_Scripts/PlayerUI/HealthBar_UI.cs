using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private RectTransform myTransform => GetComponent<RectTransform>();
    private CharacterStats myStats => GetComponentInParent<CharacterStats>();
    [SerializeField] private Slider topSlider; //health slider
    [SerializeField] private Slider bottomSlider; //damage slider
    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 0.5f;
    private float damagedHealthShrinkTimer;
    [SerializeField] private float shrinkSpeed;

    private void Start()
    {
        topSlider = transform.Find("TopSlider").GetComponent<Slider>();
        bottomSlider = transform.Find("BottomSlider").GetComponent<Slider>();
        


        UpdateHealthUI();
        bottomSlider.value = topSlider.value;
    }

    private void OnEnable() 
    {
        entity.onFlipped += FlipUI; //When the onFlipped happens we add an call the FlipUI function
        myStats.onHealthChanged += UpdateHealthUI;
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

    private void FlipUI()
    {
        if (myTransform == null)
        {
            Debug.LogWarning("HealthBar_UI: myTransform is NULL! Skipping FlipUI.");
            return;
        }
        myTransform.Rotate(0, 180, 0);
    } 

    private void OnDisable()
    {
        if(entity != null)
            entity.onFlipped -= FlipUI;

        if(myStats!= null)
            myStats.onHealthChanged -= UpdateHealthUI; 
    } 
    
}
