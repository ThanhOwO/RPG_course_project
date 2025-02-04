using UnityEngine;

public class Npc_Samurai : Npc, IInteractable
{

    #region States
    public SamuraiIdleState idleState { get; private set;}

    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new SamuraiIdleState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void Interact()
    {
        Transform player = PlayerManager.instance.player.transform;
        if(player != null)
        {
            FacePlayer(player);
        }
        Debug.Log("Hello, I'm Yukanasi the samurai. How can I help you?");
    }
}
