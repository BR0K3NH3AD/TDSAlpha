using System.Collections;
using TDS.Scripts.Enemy;
using TDS.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TDS.Scripts.UI
{
    public class PlayerHealth : MonoBehaviour
    {
        private int _playerMaxHealth = 100;
        private int _currentPlayerHealth;
        private Slider playerHealthSlider;
        private PlayerUI playerUI;
        private int enemyDamage;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private bool isDead = false;
        private Color originalColor;
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private int flashCount = 3;

        [SerializeField] private GameOverUI gameOverUI; 

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
            else
            {
                Debug.LogError("SpriteRenderer не найден на объекте");
            }
        }

        public void Initialize(Slider playerHealthSlider, PlayerUI playerUI, int maxHealth)
        {
            this.playerHealthSlider = playerHealthSlider;
            this.playerUI = playerUI;
            _playerMaxHealth = maxHealth;
            _currentPlayerHealth = _playerMaxHealth;
            playerUI.SetMaxHealth(_playerMaxHealth);
        }

        public void UpgradeHealth(int amount)
        {
            _playerMaxHealth += amount;
            _currentPlayerHealth = _playerMaxHealth;
            playerUI.SetMaxHealth(_playerMaxHealth);
            playerUI.SetHealth(_currentPlayerHealth);
        }

        public void TakeDamagePlayer(int damage)
        {
            if (isDead) return;

            _currentPlayerHealth -= damage;
            playerUI.SetHealth(_currentPlayerHealth);

            if (spriteRenderer != null)
            {
                StartCoroutine(FlashWhite());
            }

            if (_currentPlayerHealth <= 0)
            {
                HandlePlayerDeath();
            }
        }

        private void HandlePlayerDeath()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger("isDead");

            DisablePlayerComponents();

            StartCoroutine(ShowGameOverUIAfterDeath());
        }

        private IEnumerator ShowGameOverUIAfterDeath()
        {
            // Ждем окончания анимации смерти
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            gameOverUI.ShowGameOverUI();
        }

        private void DisablePlayerComponents()
        {
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null) collider.enabled = false;

            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null) rb2D.velocity = Vector2.zero;

            MonoBehaviour[] components = GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (component != this)
                {
                    component.enabled = false;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("FireBall"))
            {
                BaseEnemy enemy = collision.collider.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    enemyDamage = enemy.EnemyDamage;
                    TakeDamagePlayer(enemyDamage);
                }
            }
        }

        private IEnumerator FlashWhite()
        {
            for (int i = 0; i < flashCount; i++)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(flashDuration);
                spriteRenderer.color = originalColor;
                yield return new WaitForSeconds(flashDuration);
            }
        }
    }
}
