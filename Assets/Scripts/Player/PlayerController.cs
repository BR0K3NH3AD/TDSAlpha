using UnityEngine;

namespace TDS.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private int _playerMaxHealth = 100;
        [SerializeField] private int _enemyDamage;
        public int _currentPlayerHealth;

        public PlayerUI playerUI;

        private void Start()
        {
            _currentPlayerHealth = _playerMaxHealth;
            playerUI.SetMaxHealth(_playerMaxHealth);
        }

        public void TakeDamagePlayer(int damage)
        {
            _currentPlayerHealth -= damage;
            playerUI.SetHealth(_currentPlayerHealth);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                if (_currentPlayerHealth != 0)
                {
                    TakeDamagePlayer(_enemyDamage);
                }
                else
                {
                    Time.timeScale = 0f;
                }
            }
        }
    }
}
