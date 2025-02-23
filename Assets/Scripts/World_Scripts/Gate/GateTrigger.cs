using System.Collections;
using Cinemachine;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public GameObject gate;
    [SerializeField] private GameObject cutScene;
    private bool cutscenePlayed = false;
    private const string CutsceneKey = "BossCutscenePlayed";
    private float defaultGateDelay = 0.2f;
    private float cutsceneGateDelay = 1.5f;
    public Enemy_DeathBringer boss;
    public CanvasGroup victoryCanvasGroup;
    [SerializeField] private ParticleSystem dustCloudPrefab;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        victoryCanvasGroup.alpha = 0;
        impulseSource = GetComponentInParent<CinemachineImpulseSource>();
    }

    private IEnumerator Start()
    {
        yield return null;
        if (PlayerPrefs.GetInt(CutsceneKey, 0) == 1)
            cutscenePlayed = true;

        if (SaveManager.instance.IsGateOpened())
        {
            gameObject.SetActive(false);
        }

        if (boss != null)
            boss.OnBossDeath += OpenGate;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            float gateDelay = cutscenePlayed ? defaultGateDelay : cutsceneGateDelay;

            StartCoroutine(CloseGate(gateDelay));

            if (!cutscenePlayed)
            {
                cutscenePlayed = true;
                PlayerPrefs.SetInt(CutsceneKey, 1);
                PlayerPrefs.Save();

                cutScene.SetActive(true);
            }
            else
                boss.gameObject.SetActive(true);
        }
    }

    private IEnumerator CloseGate(float delay)
    {
        Rigidbody2D gateRb = gate.GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(delay);
        gateRb.bodyType = RigidbodyType2D.Dynamic;
        impulseSource.GenerateImpulse();
        dustCloudPrefab.Play();
        Destroy(dustCloudPrefab.gameObject, dustCloudPrefab.main.duration + dustCloudPrefab.main.startLifetime.constant);

    }

    private void OpenGate()
    {
        if (gate != null)
        {
            Rigidbody2D gateRb = gate.GetComponent<Rigidbody2D>();
            if (gateRb != null)
                gateRb.bodyType = RigidbodyType2D.Kinematic;
            StartCoroutine(MoveGateUp());
        }
        SaveManager.instance.SetGateOpened(true);
        StartCoroutine(ShowVictoryUI());
    }

    private IEnumerator MoveGateUp()
    {
        float moveSpeed = 2f;
        float targetHeight = gate.transform.position.y + 3f;
        while (gate.transform.position.y < targetHeight)
        {
            gate.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ShowVictoryUI()
    {
        yield return new WaitForSeconds(1f);
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            victoryCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        victoryCanvasGroup.alpha = 1;

        yield return new WaitForSeconds(3f);
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            victoryCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        victoryCanvasGroup.alpha = 0;
        
        //Disable gate trigger
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (boss != null)
            boss.OnBossDeath -= OpenGate;
    }

    [ContextMenu("Reset Cutscene Key")]
    public void ResetCutsceneKey()
    {
        PlayerPrefs.DeleteKey(CutsceneKey);
        PlayerPrefs.Save();
    }
}
