using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    [TextArea(5, 10)]
    public string text;
    public AudioClip[] typingSounds;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        ScriptPlayable<DialogueBehavior> playable = ScriptPlayable<DialogueBehavior>.Create(graph);
        DialogueBehavior behaviour = playable.GetBehaviour();
        behaviour.text = text;
        behaviour.typingSounds = typingSounds;
        return playable;
    }
}
