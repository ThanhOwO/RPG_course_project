using System.Collections;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    public bool bossFightBegun;
    [SerializeField] private BossHealthBar_UI bossHealthBar;

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

    #region States
    public DeathBringerIdleState idleState { get; set; }
    public DeathBringerMoveState moveState { get; set; }
    public DeathBringerAttackState attackState { get; set; }
    public DeathBringerBattleState battleState { get; set; }
    public DeathBringerSpellCastState spellCastState { get; set; }
    public DeathBringerTeleportState teleportState { get; set; }
    public DeathBringerDeathState deathState { get; set; }

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
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
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

    public override void Die()
    {
        base.Die();
        StartCoroutine(DestroyObject());
        stateMachine.ChangeState(deathState);
    }

    //Death animation remain time
    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void FindPositon()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

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

    public bool CanTeleport()
    {
        if(Random.Range(0, 100) < chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }
        else
        {
            return false;
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
        yield return new WaitForSeconds(1f);
        bossFightBegun = true;
    }
}
