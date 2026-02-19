using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);

        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(value);
    }

}
