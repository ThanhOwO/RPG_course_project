using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Option : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainMenu";
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(sceneName);
    }
}
