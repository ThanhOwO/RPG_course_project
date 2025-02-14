using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Option : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainMenu";
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI sfxText;
    [SerializeField] UI_FadeScreen fadeScreen;
    private Selectable[] menuElements;
    private int selectedIndex = 0;

    private void Start()
    {
        menuElements = new Selectable[] {sfxSlider, musicSlider, exitButton};
        
        HighlightElement(selectedIndex);
    }

    private void Update()
    {
        NavigateMenu();
    }

    private void NavigateMenu()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + menuElements.Length) % menuElements.Length;
            HighlightElement(selectedIndex);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % menuElements.Length;
            HighlightElement(selectedIndex);
        }

        if (menuElements[selectedIndex] is Slider slider)
        {
            float step = 0.001f;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                slider.value = Mathf.Max(slider.minValue, slider.value - step);
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                slider.value = Mathf.Min(slider.maxValue, slider.value + step);

            slider.value = Mathf.Round(slider.value * 1000f) / 1000f;
            UpdateVolumeText();
        }

        if (menuElements[selectedIndex] is Button && Input.GetKeyDown(KeyCode.Space))
            exitButton.onClick.Invoke();
    }

    private void HighlightElement(int index)
    {
        EventSystem.current.SetSelectedGameObject(menuElements[index].gameObject);

        foreach (var element in menuElements)
        {
            if (element is Slider slider)
            {
                SetTextColor(slider, Color.white);
            }
            else if (element is Button button)
            {
                SetButtonAlpha(button, 100);
            }
        }

        if (menuElements[index] is Slider selectedSlider)
        {
            SetTextColor(selectedSlider, Color.yellow);
        }
        else if (menuElements[index] is Button selectedButton)
        {
            SetButtonAlpha(selectedButton, 255);
        }
    }

    private void UpdateVolumeText()
    {
        musicText.text = Mathf.RoundToInt(musicSlider.value * 10).ToString();
        sfxText.text = Mathf.RoundToInt(sfxSlider.value * 10).ToString();
    }

    private void SetTextColor(Slider slider, Color color)
    {
        TextMeshProUGUI[] texts = slider.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            text.color = color;
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
    
}
