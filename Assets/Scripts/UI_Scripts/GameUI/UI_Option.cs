using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Option : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainMenu";
    [SerializeField] UI_FadeScreen fadeScreen;
    public void ExitToMainMenu()
    {
        StartCoroutine(LoadScreenWithFadeEffect(1.5f));
    }

    IEnumerator LoadScreenWithFadeEffect(float _delay)
    {
        Time.timeScale = 1;
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }
}
