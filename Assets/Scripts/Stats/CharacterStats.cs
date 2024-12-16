using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat damage;
    public Stat maxHealth;
    [SerializeField] private int currentHealth; 
    void Start()
    {
        currentHealth = maxHealth.GetValue();

        damage.AddModifier(12);
    }

    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Add death logic here
        Debug.Log("Dead");
    }
}
