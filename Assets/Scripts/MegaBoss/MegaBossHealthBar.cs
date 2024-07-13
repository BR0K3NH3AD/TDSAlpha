using UnityEngine;
using UnityEngine.UI;

namespace TDS.Scripts.LastMegaBoss
{
    public class MegaBossHealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject _megaBossUI;
        [SerializeField] private Slider _megaBossHealthSlider;

        private void Start()
        {
            MegaBoss.OnMegaBossSpawned += ActivateHealthBar;
        }
        private void ActivateHealthBar()
        {
            _megaBossUI.gameObject.SetActive(true); 
        }

        public void SetMaxHealth(int maxHealth)
        {
            _megaBossHealthSlider.maxValue = maxHealth;
            _megaBossHealthSlider.value = maxHealth;
        }

        public void SetHealth(int health)
        {
            _megaBossHealthSlider.value = health;
        }

        private void OnDestroy()
        {
            MegaBoss.OnMegaBossSpawned -= ActivateHealthBar;
        }
    }
}
