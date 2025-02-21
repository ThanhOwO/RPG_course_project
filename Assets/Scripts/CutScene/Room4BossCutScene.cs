using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

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

    private void Start()
    {
        StartCoroutine(ShowCinematicBars());
        uiInGame.alpha = 0;
        fakePlayer.SetActive(true);
        player.gameObject.SetActive(false);
        PlayerManager.instance.player.isCutScene = true;
        timeline.Play();
        timeline.stopped += OnCutsceneEnd;
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
