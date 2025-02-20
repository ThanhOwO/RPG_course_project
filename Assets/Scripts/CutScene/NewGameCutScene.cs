using UnityEngine;
using UnityEngine.Playables;

public class NewGameCutScene : MonoBehaviour
{
    public Player player;
    public GameObject fakePlayer;
    public PlayableDirector timeline;

    private void Start()
    {
        fakePlayer.SetActive(true);
        player.GetComponentInChildren<SpriteRenderer>().enabled = false;
        PlayerManager.instance.player.isCutScene = true;
        timeline.Play();
        timeline.stopped += OnCutsceneEnd;
    }

    private void OnCutsceneEnd(PlayableDirector obj)
    {
        player.GetComponentInChildren<SpriteRenderer>().enabled = true;
        PlayerManager.instance.player.isCutScene = false;
        fakePlayer.SetActive(false);

        gameObject.SetActive(false);
    }
}
