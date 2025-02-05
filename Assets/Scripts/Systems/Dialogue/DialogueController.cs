using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    [SerializeField] private float typeSpeed = 10f;

    private Queue<string> paragraphs = new Queue<string>();
    private bool conversationEnded;
    private string p;
    private bool isTyping;
    private const float MAX_TYPE_TIME = 0.1f;
    private const string HTML_ALPHA = "<color=#00000000>";
    private Coroutine typeDialogueCorotine;
    public static bool isTalking;

    public void DisplayNextParagraph(DialogueText dialogueText)
    {
        if(paragraphs.Count == 0)
        {
            if(!conversationEnded)
                StartConversation(dialogueText);
            else if (conversationEnded && !isTyping)
            {
                EndConversation();
                return;
            }
        }

        //if there is somethin in the queue
        if(!isTyping)
        {
            p = paragraphs.Dequeue();
            typeDialogueCorotine = StartCoroutine(TypeDialogueText(p));
        }
        else
        {
            FinishParagraphEarly();
        }

        //Update conversation end bool
        if(paragraphs.Count == 0)
            conversationEnded = true;

    }

    private void StartConversation(DialogueText dialogueText)
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);

        isTalking = true;
        npcName.text = dialogueText.speakerName;

        for(int i = 0; i < dialogueText.paragraphs.Length; i++)
            paragraphs.Enqueue(dialogueText.paragraphs[i]);

    }

    private void EndConversation()
    {
        paragraphs.Clear();

        conversationEnded = false;
        isTalking = false;

        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        npcDialogueText.text = " ";

        string originalText = p;
        string displayedText = " ";
        int alphaIndex = 0;

        foreach (char c in p.ToCharArray())
        {
            alphaIndex++;
            npcDialogueText.text = originalText;
            displayedText = npcDialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            npcDialogueText.text = displayedText;
            
            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }

    private void FinishParagraphEarly()
    {
        StopCoroutine(typeDialogueCorotine);

        npcDialogueText.text = p;
        isTyping = false;

    }
}
