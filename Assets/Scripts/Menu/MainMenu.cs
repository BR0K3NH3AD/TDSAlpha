using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace TDS.Scripts.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private TextMeshProUGUI _volumeValueText;
        [SerializeField] private AudioSource menuMusicSource;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private TextMeshProUGUI _musicToggleText;

        private const string VolumePrefKey = "Volume";
        private const string MusicMutedPrefKey = "MusicMuted";
        private bool isMusicMuted = false;

        private void Start()
        {
            _settingsPanel.SetActive(false);
            LoadVolumeSettings();
            LoadMusicSettings();

            if (menuMusicSource != null)
            {
                menuMusicSource.Play();
                menuMusicSource.mute = isMusicMuted;
            }

            _volumeSlider.onValueChanged.AddListener(SetVolume);
            musicToggle.onValueChanged.AddListener(delegate { ToggleMusic(musicToggle.isOn); });
        }

        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void OpenSettings()
        {
            _settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            _settingsPanel.SetActive(false);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
            if (_volumeValueText != null)
            {
                _volumeValueText.text = Mathf.RoundToInt(volume * 100).ToString();
            }
            PlayerPrefs.SetFloat(VolumePrefKey, volume);
            PlayerPrefs.Save();
        }

        private void LoadVolumeSettings()
        {
            float volume = PlayerPrefs.HasKey(VolumePrefKey) ? PlayerPrefs.GetFloat(VolumePrefKey) : 1f;
            _volumeSlider.value = volume;
            AudioListener.volume = volume;
            if (_volumeValueText != null)
            {
                _volumeValueText.text = Mathf.RoundToInt(volume * 100).ToString();
            }
        }

        public void ToggleMusic(bool isOn)
        {
            isMusicMuted = !isOn;
            menuMusicSource.mute = isMusicMuted;
            _musicToggleText.text = isMusicMuted ? "Enable Music" : "Disable Music";

            PlayerPrefs.SetInt(MusicMutedPrefKey, isMusicMuted ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadMusicSettings()
        {
            if (PlayerPrefs.HasKey(MusicMutedPrefKey))
            {
                isMusicMuted = PlayerPrefs.GetInt(MusicMutedPrefKey) == 1;
                musicToggle.isOn = !isMusicMuted;
                _musicToggleText.text = isMusicMuted ? "Enable Music" : "Disable Music";
            }
            else
            {
                musicToggle.isOn = true;
                _musicToggleText.text = "Disable Music";
            }
        }
    }
}
