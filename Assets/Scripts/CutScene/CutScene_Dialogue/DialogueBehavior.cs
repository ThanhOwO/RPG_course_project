using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueBehavior : PlayableBehaviour
{
    public string text;
    private TextMeshProUGUI dialogueText;
    private bool isPlaying = false;
    private AudioSource audioSource;
    public AudioClip[] typingSounds;

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
            if (CoroutineRunner.Instance.TryGetComponent(out AudioSource source))
                audioSource = source;
            else
                audioSource = CoroutineRunner.Instance.gameObject.AddComponent<AudioSource>();

            CoroutineRunner.Instance.StartCoroutine(TypeText(text, charDelay));
            isPlaying = true;
        }
    }

    IEnumerator TypeText(string sentence, float delayPerChar)
    {
        for (int i = 0; i < sentence.Length; i++)
        {
            dialogueText.text += sentence[i];

            // play blip sound for each 2 characters
            if (i % 2 == 0 && typingSounds.Length > 0 && audioSource != null)
            {
                audioSource.clip = typingSounds[Random.Range(0, typingSounds.Length)];
                audioSource.Play();
            }

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
