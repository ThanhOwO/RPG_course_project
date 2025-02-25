using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(parameter, 1f);
        slider.value = savedVolume;
        SliderValue(savedVolume);
    }
    
    public void SliderValue(float _value) => audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multiplier);

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(parameter, slider.value);
        PlayerPrefs.Save();
    }
}
