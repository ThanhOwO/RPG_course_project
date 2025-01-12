using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        if(SaveManager.instance.HasSaveData() == false)
            continueButton.SetActive(false);
    }
    
    public void ContinueGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        //Application.Quit();
        Debug.Log("ExitGame");
    }
}
