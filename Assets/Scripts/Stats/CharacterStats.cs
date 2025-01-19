using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicRes,
    fireDmg,
    iceDmg,
    lightningDmg,
    poisonDmg
    // add more stats as needed
}

public class CharacterStats : MonoBehaviour
{
    private EntityFx fx;
    private HealthBar_UI healthBar;

    [Header("Major stats")]
    public Stat strength; //1 point => increase damamge by 1 and crit dmg by 1% 
    public Stat agility; //1 point => increase evasion by 1 and crit rate by 0.5
    public Stat intelligence; //1 point => increase magic by 1 and magic res by 3
    public Stat vitality; //1 point => increase health by 5

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicRes;

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance; // default value = 5%
    public Stat critPower; // default value = 150%

    [Header("Magic stats")]
    public Stat fireDmg;
    public Stat iceDmg;
    public Stat lightningDmg;
    public Stat poisonDmg;

    public bool isIgnited; //Deals small damage over a short period of time and explodes for a large amount of damage (1300%) in last seconds of ignite duration
    public bool isChilled; //Apply slow effect and reduce armor by ..%
    public bool isShocked; //Apply stun effect and reduce accuracy by ..%
    public bool isPoisoned; //Deals small poison damage over an average period of time


    [SerializeField] private float igniteDuration = 5;
    [SerializeField] private float chillDuration = 4;
    [SerializeField] private float shockDuration = 4;
    [SerializeField] private float poisonDuration = 8;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;
    private float poisonedTimer;
    private float igniteDamageCooldown = .5f;
    private float igniteDamageTimer;
    private float poisonDamageCooldown = .1f;
    private float poisonDamageTimer;

    private int igniteDamage;
    private int igniteExplosiveDamage;
    private int poisonDamage;
    private int shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    [SerializeField] private GameObject igniteExplosivePrefab;

    public int currentHealth; 
    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    public bool isStagger { get; private set; }
    private bool isVulnerable;
    public bool isInvincible { get; private set; }

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFx>();
        healthBar = GetComponentInChildren<HealthBar_UI>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        poisonedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;
        poisonDamageTimer -= Time.deltaTime;

        //Timer logic for each element
        if(ignitedTimer <= 0)
        {
            if(isIgnited)
            {
                TakeDamage(igniteExplosiveDamage);
                IgniteExplosionEffect();
            }
            isIgnited = false;
        }
        if(chilledTimer <= 0)
            isChilled = false;
        if(shockedTimer <= 0)
            isShocked = false;
        if(poisonedTimer <= 0)
            isPoisoned = false;
        
        ApplyIgniteAndPoisonDamage();
    }

    public virtual void DoDamge(CharacterStats _targetStats)
    {
        bool critHit = false;

        if(CanDogdeAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockBackDir(transform);
        
        int totalDamage = damage.GetValue() + strength.GetValue();

        if(canCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
            critHit = true;
        }

        fx.CreateHitFX(_targetStats.transform, critHit);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage); //Physical damage to target
        DoMagicalDamage(_targetStats); //Magical damage to target
    }
    
    public virtual void TakeDamage(int _damage)
    {
        if(isInvincible)
            return;
            
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
            Die();
        
    }

    #region Magical damage and Aliments
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDmg.GetValue();
        int _iceDamage = iceDmg.GetValue();
        int _lightningDamage = lightningDmg.GetValue();
        int _poisonDamage = poisonDmg.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicalDamage = CheckTargetMagicRes(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        if(Mathf.Max(_fireDamage, _iceDamage, _lightningDamage, _poisonDamage) <= 0)
            return;

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage, _poisonDamage);
        
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock, bool _poison)
    {
        bool canApplyIgnite = !isChilled && !isShocked && !isPoisoned;
        bool canApplyChill = !isIgnited && !isShocked && !isPoisoned;
        bool canApplyShock = !isIgnited && !isChilled && !isPoisoned;
        bool canApplyPoison = !isIgnited && !isChilled && !isShocked;
        
        if(_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = igniteDuration;

            fx.IgniteFxFor(igniteDuration);
        }
        
        if(_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = chillDuration;
            
            float slowPercentage = .4f; //Slow the character speed by 40%
            GetComponent<Entity>().SlowEntityBy(slowPercentage, chillDuration);
            fx.ChillFxFor(chillDuration);
        }

        if(_shock && canApplyShock)
        {
            if(!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if(GetComponent<Player>() != null)
                    return;
                
                HitTargetWithThunderStrike();
            }
        }

        if(_poison && canApplyPoison)
        {
            isPoisoned = _poison;
            poisonedTimer = poisonDuration;

            fx.PoisonFxFor(poisonDuration);
        }

    }

    private void HitTargetWithThunderStrike()
    {
        //Find closest target, only among the enemies
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
            
            if(closestEnemy == null)
                closestEnemy = transform;
        }

        //Instantiate thunderstrike
        //setup thunderstrike
        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ThunderStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    
    private void IgniteExplosionEffect()
    {
        if(igniteExplosivePrefab != null)
        {
            Instantiate(igniteExplosivePrefab, transform.position, Quaternion.identity);
        }
    }

    public void ApplyShock(bool _shock)
    {
        if(isShocked)
            return;

        isShocked = _shock;
        shockedTimer = shockDuration;
        fx.ShockFxFor(shockDuration);
    }

    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage, int _poisonDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage && _fireDamage > _poisonDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage && _iceDamage > _poisonDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage && _lightningDamage > _poisonDamage;
        bool canApplyPoison = _poisonDamage > _fireDamage && _poisonDamage > _iceDamage && _poisonDamage > _lightningDamage;

        //Handle if element values are same
        while(!canApplyIgnite && !canApplyChill && !canApplyShock && !canApplyPoison)
        {
            if(Random.value < .25f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.SetupIgniteDmg(_fireDamage);
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock, canApplyPoison);
                return;
            }
            if(Random.value < .25f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock, canApplyPoison);
                return;
            }
            if(Random.value < .25f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock, canApplyPoison);
                return;
            }
            if(Random.value < .25f && _poisonDamage > 0)
            {
                canApplyPoison = true;
                _targetStats.SetupPoisonDmg(Mathf.RoundToInt(_poisonDamage * .2f)); //Deal 20% damage of poison damage
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock, canApplyPoison);
                return;
            }
        }

        //Burn damage
        if(canApplyIgnite)
            _targetStats.SetupIgniteDmg(_fireDamage);
        //Poison damage
        if(canApplyPoison)
            _targetStats.SetupPoisonDmg(Mathf.RoundToInt(_poisonDamage * .2f)); //Deal 20% damage of poison damage
        //Shocl damage
        if(canApplyShock)
            _targetStats.SetupShockDmg(Mathf.RoundToInt(_lightningDamage * .2f)); //Deal 20% damage of shock damage
        
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock, canApplyPoison);
    }

    public void SetupIgniteDmg(int _damage)
    {
        igniteDamage = Mathf.RoundToInt(_damage * .2f); //Deal 20% burn damage of fire damage
        igniteExplosiveDamage = Mathf.RoundToInt(_damage * 13f); // 1300% of fire damage
    }  
    public void SetupPoisonDmg(int _damage) => poisonDamage = _damage; 
    public void SetupShockDmg(int _damage) => shockDamage = _damage;

    private void ApplyIgniteAndPoisonDamage()
    {
        if(igniteDamageTimer <= 0 && isIgnited) 
        {
            DecreaseHealthBy(igniteDamage);
            
            if(currentHealth <= 0 && !isDead)
                Die();
            
            igniteDamageTimer = igniteDamageCooldown;
        }

        if(poisonDamageTimer <= 0 && isPoisoned) 
        {
            DecreaseHealthBy(poisonDamage);
            if(currentHealth <= 0 && !isDead)
                Die();
            
            poisonDamageTimer = poisonDamageCooldown;
        }
    }
    #endregion
    
    #region Stats calculation
    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        if(currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();
        
        if(onHealthChanged != null)
            onHealthChanged();
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        if(isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;
        if(onHealthChanged != null)
            onHealthChanged();
    }
    
    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableForCoroutine(_duration));

    private IEnumerator VulnerableForCoroutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statModify)
    {
        //Start coroutine for stat change
        StartCoroutine(StartModCoroutine(_modifier, _duration, _statModify));
    }
    private IEnumerator StartModCoroutine(int _modifier, float _duration, Stat _statModify)
    {
        _statModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statModify.RemoveModifier(_modifier);
    }
    public virtual void OnEvasion()
    {
        //Override this method
    }
    protected bool CanDogdeAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if(isShocked)
        {
            totalEvasion += 20;
        }

        if(Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private int CheckTargetMagicRes(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicRes.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        //Make sure the damage not negative
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);

        return totalMagicalDamage;
    }
    
    protected bool canCrit()
    {
        float totalCritChance = critChance.GetValue() + (agility.GetValue() * 0.5f);

        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }

        return false;
    }

    protected int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion

    protected virtual void Die()
    {
        isDead = true;

        if(healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }
    }
    
    public Stat GetStat(StatType _statType)
    {
        if(_statType == StatType.strength)
            return strength; 
        else if(_statType == StatType.agility)
            return agility;
        else if(_statType == StatType.intelligence)
            return intelligence;
        else if(_statType == StatType.vitality)
            return vitality;
        else if(_statType == StatType.damage)
            return damage;
        else if(_statType == StatType.critChance)
            return critChance;
        else if(_statType == StatType.critPower)
            return critPower;
        else if(_statType == StatType.maxHealth)
            return maxHealth;
        else if(_statType == StatType.armor)
            return armor;
        else if(_statType == StatType.evasion)
            return evasion;
        else if(_statType == StatType.magicRes)
            return magicRes;
        else if(_statType == StatType.fireDmg)
            return fireDmg;
        else if(_statType == StatType.iceDmg)
            return iceDmg;
        else if(_statType == StatType.lightningDmg)
            return lightningDmg;
        else if(_statType == StatType.poisonDmg)
            return poisonDmg;
        
        return null;
    }

    public void KillEntity()
    {
        if(!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    public bool IsCriticalHit(int _damage)
    {
        float normalDamage = damage.GetValue() + strength.GetValue();
        float criticalDamageThreshold = normalDamage * (critPower.GetValue() * 0.01f);
        return _damage >= criticalDamageThreshold;
    }
}
