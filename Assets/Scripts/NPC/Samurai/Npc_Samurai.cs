using UnityEngine;

public class Npc_Samurai : Npc, ITalkable
{

    [SerializeField] private DialogueText dialogueText;
    [SerializeField] private DialogueController dialogueController;
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

    public override void Interact()
    {
        Player player = PlayerManager.instance.player;
        if(player.transform != null)
        {
            player.stateMachine.ChangeState(player.idleState);
            FacePlayer(player.transform);
        }
        
        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        dialogueController.DisplayNextParagraph(dialogueText);
    }
}
