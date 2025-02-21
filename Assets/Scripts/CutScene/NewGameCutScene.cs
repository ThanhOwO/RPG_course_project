using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class NewGameCutScene : MonoBehaviour
{
    public Player player;
    public GameObject fakePlayer;
    public PlayableDirector timeline;
    public CanvasGroup uiInGame;

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
        player.GetComponentInChildren<SpriteRenderer>().enabled = false;
        PlayerManager.instance.player.isCutScene = true;
        timeline.Play();
        timeline.stopped += OnCutsceneEnd;
    }

    private void OnCutsceneEnd(PlayableDirector obj)
    {
        player.GetComponentInChildren<SpriteRenderer>().enabled = true;
        StartCoroutine(EndCutscene());
        PlayerManager.instance.player.isCutScene = false;
        fakePlayer.SetActive(false);
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
