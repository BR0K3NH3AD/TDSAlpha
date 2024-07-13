using UnityEngine;
using UnityEngine.UI;
using TDS.Scripts.UI;
using TDS.Scripts.Player;

namespace TDS.Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private PlayerShooting playerShooting;
        private EnemyDetection enemyDetection;
        private PlayerInputSystem playerInputSystem;
        private PlayerHealth playerHealth;
        private ScoreManager scoreManager;

        [Header("Player Settings")]
        [SerializeField] private int playerMaxHealth = 100;
        [SerializeField] private float _playerMoveSpeed = 40f;

        [Header("Player Attack Settings")]
        [SerializeField] private float _initialFireSpeed = 10f;
        [SerializeField] private float _playerAttackRadius = 10f;
        [SerializeField] private int _initialDamage = 10;

        [Header("Game Prefabs")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform weaponTransform; // Оружие, которое изначально используется

        [Header("Weapon Upgrade")]
        [SerializeField] private Transform newWeaponTransform; // Новое оружие
        [SerializeField] private Transform weaponHolder; // Добавляем Transform для контейнера оружия
        [SerializeField] private Transform newFirePoint;
        [SerializeField] private float newWeaponFireSpeed = 20f;
        [SerializeField] private int newWeaponDamage = 10;

        [Header("Enemy Layer")]
        [SerializeField] private LayerMask _enemyLayerMask;

        [Header("Player UI Component")]
        [SerializeField] private Slider playerHealthSlider;
        [SerializeField] private PlayerUI playerUI;

        private void Awake()
        {
            playerShooting = GetComponent<PlayerShooting>();
            enemyDetection = GetComponent<EnemyDetection>();
            playerInputSystem = GetComponent<PlayerInputSystem>();
            playerHealth = GetComponent<PlayerHealth>();
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        private void Start()
        {
            playerShooting.Initialize(bulletPrefab, firePoint, _initialFireSpeed, _initialDamage, weaponTransform);
            enemyDetection.Initialize(_playerAttackRadius, _enemyLayerMask);
            playerInputSystem.Initialize(_playerMoveSpeed);
            playerHealth.Initialize(playerHealthSlider, playerUI, playerMaxHealth);
        }

        private void Update()
        {
            Transform nearestEnemy = enemyDetection.GetNearestEnemy();
            playerShooting.HandleShooting(nearestEnemy);
        }

        public void AddScore(int amount)
        {
            scoreManager.AddScore(amount);
        }

        public void UpgradeHealth(int amount)
        {
            playerHealth.UpgradeHealth(amount);
        }

        public void UpgradeDamage(int amount)
        {
            playerShooting.SetDamage(playerShooting.Damage + amount);
        }

        public void UpgradeSpeed(float amount)
        {
            playerInputSystem.SetMoveSpeed(playerInputSystem.GetMoveSpeed() + amount);
        }

        public void SwitchWeapon()
        {
            playerShooting.SwitchWeapon(newWeaponTransform, newFirePoint, newWeaponFireSpeed, newWeaponDamage);
        }
    }
}
