using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Room4BossCutScene : MonoBehaviour
{
    public Player player;
    public GameObject fakePlayer;
    public PlayableDirector timeline;
    public CanvasGroup uiInGame;
    public Enemy_DeathBringer boss;

    [Header("Cinematic Bars")]
    public RectTransform cinematicBarTop;
    public RectTransform cinematicBarBottom;
    public float barTargetSize = 100f;
    public float transitionSpeed = 2f;

    [Header("Skip UI")]
    public CanvasGroup skipUI;
    public Image skipProgressBar;
    public TextMeshProUGUI skipText;
    private bool hasInteracted = false;

    [Header("Skip Settings")]
    public float holdTime = 3f;
    private float holdTimer = 0f;
    private bool canSkip = false;

    private void Start()
    {
        StartCoroutine(ShowCinematicBars());
        uiInGame.alpha = 0;
        fakePlayer.SetActive(true);
        player.gameObject.SetActive(false);
        PlayerManager.instance.player.isCutScene = true;
        timeline.Play();
        timeline.stopped += OnCutsceneEnd;
        canSkip = true;
        skipUI.alpha = 0;
        skipProgressBar.fillAmount = 0f;
    }

    private void Update()
    {
        if (!hasInteracted && Input.anyKeyDown)
        {
            skipUI.alpha = 1;
            hasInteracted = true;
        }

        if (canSkip && hasInteracted && Input.GetKey(KeyCode.E))
        {
            holdTimer += Time.deltaTime;
            skipProgressBar.fillAmount = holdTimer / holdTime;

            if (holdTimer >= holdTime)
            {
                SkipCutscene();
            }
        }
        else
        {
            holdTimer = 0f;
            skipProgressBar.fillAmount = 0f;
        }
    }

    private void SkipCutscene()
    {
        if (!canSkip) return;

        timeline.time = timeline.duration;
        timeline.Evaluate();
        OnCutsceneEnd(timeline);
        canSkip = false;
    }

    private void OnCutsceneEnd(PlayableDirector obj)
    {
        StartCoroutine(EndCutscene());
        PlayerManager.instance.player.isCutScene = false;
        player.transform.position = fakePlayer.transform.position;
        fakePlayer.SetActive(false);
        player.gameObject.SetActive(true);
        player.stateMachine.ChangeState(player.idleState);
        boss.skipAppearState = true;
        boss.gameObject.SetActive(true);
        uiInGame.alpha = 1;
        canSkip = false;
        skipUI.alpha = 0;
    }

    private IEnumerator ShowCinematicBars()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            cinematicBarTop.sizeDelta = new Vector2(cinematicBarTop.sizeDelta.x, Mathf.Lerp(0, barTargetSize, t));
            cinematicBarBottom.sizeDelta = new Vector2(cinematicBarBottom.sizeDelta.x, Mathf.Lerp(0, barTargetSize, t));
            yield return null;
        }
    }

    private IEnumerator HideCinematicBars()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            cinematicBarTop.sizeDelta = new Vector2(cinematicBarTop.sizeDelta.x, Mathf.Lerp(barTargetSize, 0, t));
            cinematicBarBottom.sizeDelta = new Vector2(cinematicBarBottom.sizeDelta.x, Mathf.Lerp(barTargetSize, 0, t));
            yield return null;
        }
    }

    private IEnumerator EndCutscene()
    {
        yield return StartCoroutine(HideCinematicBars());
        gameObject.SetActive(false);
    }
}
