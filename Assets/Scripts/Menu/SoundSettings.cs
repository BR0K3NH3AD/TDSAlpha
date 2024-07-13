using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public Slider volumeSlider; 

    private const string VolumePrefKey = "Volume"; 

    private void Start()
    {
        LoadVolumeSettings();

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;

        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();
    }

    private void LoadVolumeSettings()
    {
        float volume = PlayerPrefs.HasKey(VolumePrefKey) ? PlayerPrefs.GetFloat(VolumePrefKey) : 1f;
        volumeSlider.value = volume;
        AudioListener.volume = volume;
    }
}
