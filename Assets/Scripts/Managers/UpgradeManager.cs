using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TDS.Scripts.Managers
{
    public class UpgradeManager : MonoBehaviour
    {
        [Header("Upgrade Buttons")]
        [SerializeField] private Button upgradeHPButton;
        [SerializeField] private Button upgradeDamageButton;
        [SerializeField] private Button upgradeSpeedButton;
        [SerializeField] private Button buyWeaponButton;

        [Header("Upgrade Costs")]
        [SerializeField] private int hpUpgradeCost = 50;
        [SerializeField] private int damageUpgradeCost = 50;
        [SerializeField] private int speedUpgradeCost = 50;
        [SerializeField] private int weaponPurchaseCost = 10;

        [Header("Upgrade Amounts")]
        [SerializeField] private int hpUpgradeAmount = 10;
        [SerializeField] private int damageUpgradeAmount = 5;
        [SerializeField] private float speedUpgradeAmount = 5f;

        [Header("Text Fields")]
        [SerializeField] private TextMeshProUGUI hpUpgradeText;
        [SerializeField] private TextMeshProUGUI damageUpgradeText;
        [SerializeField] private TextMeshProUGUI speedUpgradeText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private TextMeshProUGUI weaponPurchaseText;

        [Header("Upgrade Menu")]
        [SerializeField] private GameObject upgradeMenu;

        private PlayerManager playerManager;
        private ScoreManager scoreManager;
        private bool isMenuActive = false;

        private void Start()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            scoreManager = FindObjectOfType<ScoreManager>();

            upgradeHPButton.onClick.AddListener(UpgradeHP);
            upgradeDamageButton.onClick.AddListener(UpgradeDamage);
            upgradeSpeedButton.onClick.AddListener(UpgradeSpeed);
            buyWeaponButton.onClick.AddListener(BuyWeapon);

            UpdateUpgradeTexts();
            upgradeMenu.SetActive(isMenuActive);
            feedbackText.text = string.Empty;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                ToggleUpgradeMenu();
            }
        }

        private void ToggleUpgradeMenu()
        {
            isMenuActive = !isMenuActive;
            upgradeMenu.SetActive(isMenuActive);
            feedbackText.text = string.Empty;

            if (isMenuActive)
            {
                Time.timeScale = 0f; 
            }
            else
            {
                Time.timeScale = 1f; 
            }
        }

        private void UpdateUpgradeTexts()
        {
            hpUpgradeText.text = $"Upgrade HP +{hpUpgradeAmount}\nCost: {hpUpgradeCost} Points";
            damageUpgradeText.text = $"Upgrade Damage +{damageUpgradeAmount}\nCost: {damageUpgradeCost} Points";
            speedUpgradeText.text = $"Upgrade Speed +{speedUpgradeAmount}\nCost: {speedUpgradeCost} Points";
            weaponPurchaseText.text = $"Buy new weapon\nCost: {weaponPurchaseCost} Points";
        }

        private void UpgradeHP()
        {
            if (scoreManager.Score >= hpUpgradeCost)
            {
                playerManager.UpgradeHealth(hpUpgradeAmount);
                scoreManager.AddScore(-hpUpgradeCost);
                feedbackText.text = "Улучшение куплено";
            }
            else
            {
                feedbackText.text = "Не достаточно очков";
            }
        }

        private void UpgradeDamage()
        {
            if (scoreManager.Score >= damageUpgradeCost)
            {
                playerManager.UpgradeDamage(damageUpgradeAmount);
                scoreManager.AddScore(-damageUpgradeCost);
                feedbackText.text = "Улучшение куплено";
            }
            else
            {
                feedbackText.text = "Не достаточно очков";
            }
        }

        private void UpgradeSpeed()
        {
            if (scoreManager.Score >= speedUpgradeCost)
            {
                playerManager.UpgradeSpeed(speedUpgradeAmount);
                scoreManager.AddScore(-speedUpgradeCost);
                feedbackText.text = "Улучшение куплено";
            }
            else
            {
                feedbackText.text = "Не достаточно очков";
            }
        }

        private void BuyWeapon()
        {
            if (scoreManager.Score >= weaponPurchaseCost)
            {
                playerManager.SwitchWeapon();
                scoreManager.AddScore(-weaponPurchaseCost);
                feedbackText.text = "Новое оружие куплено";
            }
            else
            {
                feedbackText.text = "Не достаточно очков";
            }
        }
    }
}
