using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
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

    public void LoadData(GameData _data)
    {
       this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }

}
