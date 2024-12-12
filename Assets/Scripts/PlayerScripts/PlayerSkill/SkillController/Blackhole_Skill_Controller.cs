using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;
    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotKeys = true;
    private bool cloneAttackReleased;
    private float amountOfAttack = 5;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;
    private bool playerCanDisappear = true;
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();
    public bool playerCanExitState {get; private set;}
    private HashSet<Collider2D> enemiesWithHotKey = new();

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttack, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttack = _amountOfAttack;
        blackholeTimer = _blackholeDuration;
        cloneAttackCooldown = _cloneAttackCooldown;
    }

    private void Update()
    {
        cloneAttackTimer = -Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if(targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHoleSkill();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if(canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if(transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        DestroyHotKey();
        cloneAttackReleased = true;
        canCreateHotKeys = false;

        if(playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.makeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttack > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;


            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            if(targets.Count > 0)
            {
                int RandomIndex = Random.Range(0, targets.Count);
                SkillManager.instance.clone.CreateClone(targets[RandomIndex], new Vector3(xOffset, 0));
                amountOfAttack--;
            }

            if (amountOfAttack <= 0)
            {
                cloneAttackReleased = false;
                Invoke("FinishBlackHoleSkill", 1f);
            }
        }
    }

    private void FinishBlackHoleSkill()
    {
        DestroyHotKey();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;

        PlayerManager.instance.player.playerIsInBlackHole = false;
    }

    private void DestroyHotKey()
    {
        if(createdHotKey.Count <= 0)
            return;
        
        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy) && canCreateHotKeys)
        {
            if(!enemiesWithHotKey.Contains(collision)) {
                enemy.FreezeTime(true);
                CreateHotKey(collision);
                enemiesWithHotKey.Add(collision);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>()!= null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }

    private void CreateHotKey(Collider2D collision)
    {
        if(keyCodeList.Count <= 0 )
        {
            Debug.Log("No more hotkeys available");
            return;
        }
        
        if(!canCreateHotKeys)
            return;

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);
            
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_Hotkey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_Hotkey_Controller>();
        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
