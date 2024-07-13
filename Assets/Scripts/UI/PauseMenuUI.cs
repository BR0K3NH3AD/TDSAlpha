using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TDS.Scripts.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private TextMeshProUGUI _musicToggleText;
        [SerializeField] private TextMeshProUGUI _volumeValueText;
        [SerializeField] private List<AudioSource> gameAudioSources;

        private const string VolumePrefKey = "Volume";
        private const string MusicMutedPrefKey = "MusicMuted";
        private bool isPaused = false;
        private bool isMusicMuted = false;

        private void Start()
        {
            pauseMenuUI.SetActive(false);
            LoadVolumeSettings();
            LoadMusicSettings();

            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
            musicToggle.onValueChanged.AddListener(delegate { ToggleMusic(musicToggle.isOn); });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1.0f;
            isPaused = false;
        }

        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
            foreach (var source in gameAudioSources)
            {
                source.volume = volume;
            }
            if (_volumeValueText != null)
            {
                _volumeValueText.text = Mathf.RoundToInt(volume * 100).ToString();
            }
            PlayerPrefs.SetFloat(VolumePrefKey, volume);
            PlayerPrefs.Save();
        }

        public void ToggleMusic(bool isOn)
        {
            isMusicMuted = !isOn;
            foreach (var source in gameAudioSources)
            {
                source.mute = isMusicMuted;
            }
            _musicToggleText.text = isMusicMuted ? "Enable Music" : "Disable Music";

            PlayerPrefs.SetInt(MusicMutedPrefKey, isMusicMuted ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadVolumeSettings()
        {
            float volume = PlayerPrefs.HasKey(VolumePrefKey) ? PlayerPrefs.GetFloat(VolumePrefKey) : 1f;
            volumeSlider.value = volume;
            AudioListener.volume = volume;
            foreach (var source in gameAudioSources)
            {
                source.volume = volume;
            }
            if (_volumeValueText != null)
            {
                _volumeValueText.text = Mathf.RoundToInt(volume * 100).ToString();
            }
        }

        private void LoadMusicSettings()
        {
            if (PlayerPrefs.HasKey(MusicMutedPrefKey))
            {
                isMusicMuted = PlayerPrefs.GetInt(MusicMutedPrefKey) == 1;
                musicToggle.isOn = !isMusicMuted;
                _musicToggleText.text = isMusicMuted ? "Enable Music" : "Disable Music";
                foreach (var source in gameAudioSources)
                {
                    source.mute = isMusicMuted;
                }
            }
            else
            {
                musicToggle.isOn = true;
                _musicToggleText.text = "Disable Music";
            }
        }

        public void QuitGame()
        {
            Application.Quit();
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
        }
    }

}

