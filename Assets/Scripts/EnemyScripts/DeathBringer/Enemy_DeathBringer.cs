using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    public bool bossFightBegun;
    [SerializeField] private BossHealthBar_UI bossHealthBar;
    [HideInInspector] public bool skipAppearState = false;
    public event Action OnBossDeath;

    [Header("Teleport Details")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25;
    
    [Header("Spell Cast Details")]
    [SerializeField] private GameObject spellPrefab;
    public float lastTimeCast;
    [SerializeField] private float spellStateCooldown;
    public int amountOfSpells;
    public float spellCooldown;
    [SerializeField] private Vector2 spellOffset;

    [Header("Phase 2 spell cast details")]
    [SerializeField] private GameObject spellPrefab2;
    [SerializeField] private GameObject spellPrefab3;
    [SerializeField] private float spell2StateCooldown;
    public float lastTimeCast2;
    [SerializeField] private List<Transform> spawnPositions;
    private List<GameObject> phase2Spells = new List<GameObject>();

    [Header("Bullet Spell Details")]
    public BulletSpawnerController bulletSpawnerLeft;
    public BulletSpawnerController bulletSpawnerRight;
    [SerializeField] private int bulletNumberLeft = 7;
    [SerializeField] private int bulletNumberRight = 5;
    public float lastTimeShootSpell;
    public float shootSpellCooldown = 10f;

    #region States
    public DeathBringerIdleState idleState { get; set; }
    public DeathBringerMoveState moveState { get; set; }
    public DeathBringerAttackState attackState { get; set; }
    public DeathBringerBattleState battleState { get; set; }
    public DeathBringerSpellCastState spellCastState { get; set; }
    public DeathBringerTeleportState teleportState { get; set; }
    public DeathBringerDeathState deathState { get; set; }
    public DeathBringerPhase2State phase2State { get; set; }
    public DeathBringerShootState shootState { get; set; }
    public DeathBringerAppearState appearState { get; set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(-1);

        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        moveState = new DeathBringerMoveState(this, stateMachine, "Move", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        spellCastState = new DeathBringerSpellCastState(this, stateMachine, "SpellCast", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
        deathState = new DeathBringerDeathState(this, stateMachine, "Die", this);
        phase2State = new DeathBringerPhase2State(this, stateMachine, "Phase2Cast", this);
        shootState = new DeathBringerShootState(this, stateMachine, "Bullet", this);
        appearState = new DeathBringerAppearState(this, stateMachine, "Appear", this);
    }

    protected override void Start()
    {
        base.Start();
        if(skipAppearState)
            stateMachine.Initialize(idleState);
        else
            stateMachine.Initialize(appearState);
        InitializeSpells();
    }
    public override void Die()
    {
        base.Die();
        StartCoroutine(DestroyObject());
        stateMachine.ChangeState(deathState);
        OnBossDeath?.Invoke();
    }

    //Death animation remain time
    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void FindPositon()
    {
        float x = UnityEngine.Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = UnityEngine.Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));
        if (SomethingIsAround() || !GroundBelow())
        {
            Debug.Log("looking for new position");
        }
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);

        Gizmos.color = Color.red;
        Vector2 boxCenter = (Vector2)wallCheck.position + Vector2.right * FacingDir * (attackDistance / 2);
        Vector2 boxSize = new Vector2(attackDistance, attackHeight);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    #region Spell casting

    public void CastBulletSpell()
    {
        StartCoroutine(bulletSpawnerLeft.SpawnBulletsFromLeftWithDelay(bulletNumberLeft, 1f, stats));
        StartCoroutine(bulletSpawnerRight.SpawnBulletsFromRightWithDelay(bulletNumberRight, 1.7f, stats));
    }
    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;
        float xOffset = 0;

        if(player.rb.linearVelocityX != 0)
            xOffset = player.FacingDir * spellOffset.x;

        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + spellOffset.y);
        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
        newSpell.GetComponent<DeathBringerSpell_Controller>().SetupSpell(stats);
    }

    private void InitializeSpells()
    {
        phase2Spells.Add(spellPrefab2);
        phase2Spells.Add(spellPrefab3);
    }

    public void CastSpell2()
    {
        if (spawnPositions.Count == 0)
        {
            Debug.LogError("No spawn positions assigned!");
            return;
        }

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            GameObject prefabToSpawn = phase2Spells[i % phase2Spells.Count];
            GameObject spawnedSpell = Instantiate(prefabToSpawn, spawnPositions[i].position, Quaternion.identity);

            if (prefabToSpawn == spellPrefab3)
                spawnedSpell.transform.Rotate(0f, 0f, 180f);

            spawnedSpell.GetComponent<DeathBringerPhase2Spell>().SetupPhase2Spell(stats);
        }
    }


    public bool CanDoSpellCast()
    {
        if(Time.time >= lastTimeCast + spellStateCooldown)
        {
            return true;
        }

        return false;
    }

    public bool CanDoPhase2SpellCast()
    {
        if(Time.time >= lastTimeCast2 + spell2StateCooldown)
        {
            return true;
        }

        return false;
    }

    public bool CanTeleport()
    {
        if(UnityEngine.Random.Range(0, 100) < chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Health UI
    public void StartShowBossHealth()
    {
        bossHealthBar.SetBoss(enemyStats);
        StartCoroutine(delayBossStart());
    }

    public void HideBossHealth()
    {
        bossFightBegun = false;
        bossHealthBar.HideBossHealthBar();
    }

    private IEnumerator delayBossStart()
    {
        yield return new WaitForSeconds(0.5f);
        bossFightBegun = true;
    }
    #endregion


}
