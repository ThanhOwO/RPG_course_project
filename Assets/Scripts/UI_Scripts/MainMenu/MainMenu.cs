using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] private Button[] buttons;
    private int selectedIndex = 0;

    private void Start()
    {
        bool hasSave = SaveManager.instance.HasSaveData();
    
        if (!hasSave)
        {
            continueButton.SetActive(false);
            List<Button> buttonList = new List<Button>(buttons);
            buttonList.Remove(continueButton.GetComponent<Button>());
            buttons = buttonList.ToArray();
            selectedIndex = 0;
        }
        else
            selectedIndex = 0;

        HighlightButton(selectedIndex);
    }
    private void Update()
    {
        NavigateButtons();
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadScreenWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadScreenWithFadeEffect(1.5f));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadScreenWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }

    private void NavigateButtons()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            selectedIndex = (selectedIndex + 1) % buttons.Length;
        
        HighlightButton(selectedIndex);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    private void HighlightButton(int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Image buttonImg = buttons[i].GetComponent<Image>();
            if (buttonImg != null)
            {
                buttonImg.color = (i == index) ? Color.green : Color.black;
            }
        }
    }
}
