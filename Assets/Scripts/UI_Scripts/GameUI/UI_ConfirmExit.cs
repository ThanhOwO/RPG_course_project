using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ConfirmExit : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainMenu";
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    private Selectable[] menuElements;
    private int selectedIndex = 1;
    private UI_Option uiOption;

    private void Start()
    {
        uiOption = GetComponentInParent<UI_Option>();
        menuElements = new Selectable[] { yesButton, noButton };
        HighlightElement(selectedIndex);
    }

    private void Update()
    {
        NavigateMenu();
    }

    private void NavigateMenu()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            selectedIndex = (selectedIndex - 1 + menuElements.Length) % menuElements.Length;
            HighlightElement(selectedIndex);
            AudioManager.instance.PlayUISFX(0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            selectedIndex = (selectedIndex + 1) % menuElements.Length;
            HighlightElement(selectedIndex);
            AudioManager.instance.PlayUISFX(0);
        }

        if (menuElements[selectedIndex] is Button && Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PlayUISFX(1);
            menuElements[selectedIndex].GetComponent<Button>().onClick.Invoke();
        }
    }

    private void HighlightElement(int index)
    {
        EventSystem.current.SetSelectedGameObject(menuElements[index].gameObject);

        foreach (var element in menuElements)
        {
            if (element is Button button)
            {
                SetButtonAlpha(button, 100);
            }
        }

        if (menuElements[index] is Button selectedButton)
        {
            SetButtonAlpha(selectedButton, 255);
        }
    }

    private void SetButtonAlpha(Button button, byte alpha)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            Color32 color = buttonImage.color;
            color.a = alpha;
            buttonImage.color = color;
        }
    }

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

    public void NoOption()
    {
        gameObject.SetActive(false);
        uiOption.isConfirmOpen = false;
    }
}
