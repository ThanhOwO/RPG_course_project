using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    public string text;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        ScriptPlayable<DialogueBehavior> playable = ScriptPlayable<DialogueBehavior>.Create(graph);
        DialogueBehavior behaviour = playable.GetBehaviour();
        behaviour.text = text;
        return playable;
    }
}
