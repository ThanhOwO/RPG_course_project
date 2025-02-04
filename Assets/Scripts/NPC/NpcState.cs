using UnityEngine;

public class NpcState
{
    protected NpcStateMachine stateMachine;
    protected Rigidbody2D rb;
    protected Npc npcBase;
    private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    public NpcState(Npc _npcBase, NpcStateMachine _stateMachine, string _animBoolName)
    {
        this.npcBase = _npcBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = npcBase.rb;
        npcBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    
    public virtual void Exit()
    {
        npcBase.anim.SetBool(animBoolName, false);
        npcBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
