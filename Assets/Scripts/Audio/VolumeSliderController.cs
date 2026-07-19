using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string audioParam;
    [SerializeField] private float multiplier;

    private void Awake() => multiplier = 25;

    public void SetupSlider()
    {
        slider.onValueChanged.AddListener(ChangeSliderValue);
        slider.minValue = 0.001f;
        slider.value = PlayerPrefs.GetFloat(audioParam, slider.value);
    }

    private void OnDisable() 
        => PlayerPrefs.SetFloat(audioParam, slider.value);

    private void ChangeSliderValue(float value)
        => audioMixer.SetFloat(audioParam, Mathf.Log10(value) * multiplier);
}
