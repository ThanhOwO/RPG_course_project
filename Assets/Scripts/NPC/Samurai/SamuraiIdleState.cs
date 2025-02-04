using UnityEngine;

public class SamuraiIdleState : SamuraiGroundState
{
    public SamuraiIdleState(Npc _npcBase, NpcStateMachine _stateMachine, string _animBoolName, Npc_Samurai _npc) : base(_npcBase, _stateMachine, _animBoolName, _npc)
    {
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
