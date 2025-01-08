using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

    private void Awake() {

        if (instance != null && instance != this) 
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool HaveEnoughSkillPoint(int _amount)
    {
        if (_amount > currency)
        {
            Debug.Log("Not enough skill point");
            return false;
        }

        currency -= _amount;
        return true; 
    }

    public int GetCurrentCurrency() => currency;
}
