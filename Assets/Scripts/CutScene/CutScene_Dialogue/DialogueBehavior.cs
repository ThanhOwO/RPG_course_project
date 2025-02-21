using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueBehavior : PlayableBehaviour
{
    [TextArea(5, 10)]
    public string text;
    private TextMeshProUGUI dialogueText;
    private bool isPlaying = false;

    public override void OnPlayableCreate(Playable playable)
    {
        // Called when the playable is created, before ProcessFrame
        isPlaying = false;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (isPlaying) return;

        dialogueText = playerData as TextMeshProUGUI;
        if (dialogueText != null)
        {
            dialogueText.text = "";
            double clipDuration = playable.GetDuration();
            // Calculate delay per character based on clip duration and text length
            float charDelay = (float)(clipDuration * 0.5f / text.Length); // 50% of clip duration
            CoroutineRunner.Instance.StartCoroutine(TypeText(text, charDelay));
            isPlaying = true;
        }
    }

    IEnumerator TypeText(string sentence, float delayPerChar)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(delayPerChar);
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
        isPlaying = false;
    }
}
