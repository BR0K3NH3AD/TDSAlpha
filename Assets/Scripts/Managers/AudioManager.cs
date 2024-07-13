using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TDS.Scripts.Managers
{
    public class AudioManager : MonoBehaviour
    {
        private const string VolumePrefKey = "Volume";
        private AudioSource[] audioSources;

        private void Start()
        {
            audioSources = FindObjectsOfType<AudioSource>();
            LoadVolumeSettings();
        }

        private void LoadVolumeSettings()
        {
            float volume = PlayerPrefs.HasKey(VolumePrefKey) ? PlayerPrefs.GetFloat(VolumePrefKey) : 1f;

            foreach (var source in audioSources)
            {
                source.volume = volume;
            }
        }

        public void UpdateVolume(float volume)
        {
            foreach (var source in audioSources)
            {
                source.volume = volume;
            }
        }
    }

}
