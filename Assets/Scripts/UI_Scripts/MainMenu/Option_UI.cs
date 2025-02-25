using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Option_UI : MonoBehaviour
{
    [SerializeField] private UI_VolumeSlider sfxSlider;
    [SerializeField] private UI_VolumeSlider musicSlider;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI sfxText;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;
    private MainMenu menu;
    
    private Selectable[] menuElements;
    private int selectedIndex = 0;

    private void Start()
    {
        menu = GetComponentInParent<MainMenu>();
        menuElements = new Selectable[] { sfxSlider.slider, musicSlider.slider, saveButton, closeButton };
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
            AudioManager.instance.PlayUISFX(0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % menuElements.Length;
            HighlightElement(selectedIndex);
            AudioManager.instance.PlayUISFX(0);
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
        musicText.text = Mathf.RoundToInt(musicSlider.slider.value * 10).ToString();
        sfxText.text = Mathf.RoundToInt(sfxSlider.slider.value * 10).ToString();
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

    public void SaveSettings()
    {
        sfxSlider.SaveVolume();
        musicSlider.SaveVolume();
        gameObject.SetActive(false);
        menu.isOptionOpen = false;
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
        menu.isOptionOpen = false;
    }
}
