using UnityEngine;
using UnityEngine.UI;

namespace TDS.Scripts.Player
{
    public class PlayerUI : MonoBehaviour
    {
        private Slider playerHealthSlider;

        private void Awake()
        {
            if (playerHealthSlider == null)
            {
                playerHealthSlider = GetComponent<Slider>();
                if (playerHealthSlider == null)
                {
                    Debug.LogError("PlayerUI: Slider component error");
                }
            }
        }

        public void SetSlider(Slider slider)
        {
            playerHealthSlider = slider;
        }

        public void SetMaxHealth(int health)
        {
            if (playerHealthSlider != null)
            {
                playerHealthSlider.maxValue = health;
                playerHealthSlider.value = health;
            }
            else
            {
                Debug.LogError("PlayerUI: Slider component error");
            }
        }

        public void SetHealth(int health)
        {
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = health;
            }
            else
            {
                Debug.LogError("PlayerUI: Slider component error");
            }
        }
    }
}
