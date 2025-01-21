using UnityEngine;

public class FireExplode_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(CharacterStats _targetStats)
    {
        targetStats = _targetStats;
    }

    void Update()
    {
        Invoke(nameof(SelfDestroy), .5f);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject, .3f);
    }


}
