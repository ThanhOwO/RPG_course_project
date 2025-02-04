using UnityEngine;

public class SamuraiGroundState : NpcState
{
    protected Npc_Samurai npc;
    public SamuraiGroundState(Npc _npcBase, NpcStateMachine _stateMachine, string _animBoolName, Npc_Samurai _npc) : base(_npcBase, _stateMachine, _animBoolName)
    {
        this.npc = _npc;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
