using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        // Carica il volume salvato se esiste, altrimenti 0.5
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        // Applica subito al MusicManager
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(slider.value);

        // Aggiungi listener allo slider
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(value);
            // Salva il volume
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }
}
