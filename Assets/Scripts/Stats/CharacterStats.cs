using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat strenght;
    public Stat damage;
    public Stat maxHealth;
    [SerializeField] private int currentHealth; 
    public virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamge(CharacterStats _targetStats)
    {
        int totalDamage = damage.GetValue() + strenght.GetValue();

        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        Debug.Log(_damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // Add death logic here
        Debug.Log("Dead");
    }
}
