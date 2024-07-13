using System.Collections;
using UnityEngine;
using TMPro;
using TDS.Scripts.Managers;

namespace TDS.Scripts.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private GameObject _bossPrefab;
        [SerializeField] private GameObject _portalPrefab;

        [SerializeField] private GameObject _activationObjectPrefab;


        [Header("Enemy Settings")]
        [SerializeField] private EnemySettings _enemySettings;
        [SerializeField] private EnemySettings _bossSettings;

        [Header("Enemy SpawnPoints")]
        [SerializeField] Transform[] _spawnPoints;

        [Header("Wave settings")]
        [SerializeField] private float _timeBetweenWaves = 5f;
        [SerializeField] private Wave[] waves;

        [Header("UI Component")]
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private TextMeshProUGUI _dynamicWaveText;

        [Header("Portal Settings")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform playerTeleportDestination;
        [SerializeField] private Transform megaBossSpawnPoint;

        [Header("Audio")]
        [SerializeField] private AudioSource _waveStartAudioSource;

        private int currentWaveIndex = 0;
        private EnemyManager enemyManager;

        private void Start()
        {
            enemyManager = FindObjectOfType<EnemyManager>();
            if (enemyManager != null)
            {
                enemyManager.OnAllEnemiesDefeated += OnAllEnemiesDefeated;
            }
            StartCoroutine(SpawnWaves());
        }

        IEnumerator SpawnWaves()
        {
            while (currentWaveIndex < waves.Length)
            {
                Wave currentWave = waves[currentWaveIndex];
                UpdateWaveText();

                ShowCenterText("Wave " + (currentWaveIndex + 1));

                _waveStartAudioSource.Play();

                yield return StartCoroutine(SpawnEnemies(currentWave));

                yield return new WaitUntil(() => allEnemiesDefeated);

                currentWaveIndex++;
                yield return new WaitForSeconds(_timeBetweenWaves);
            }
            SpawnActivationObject();
        }

        private bool allEnemiesDefeated = false;

        IEnumerator SpawnEnemies(Wave wave)
        {
            allEnemiesDefeated = false;

            float healthMultiplier = 1 + (currentWaveIndex * 0.1f);
            float damageMultiplier = 1 + (currentWaveIndex * 0.05f);

            for (int i = 0; i < wave.enemyCount; i++)
            {
                int health = Mathf.RoundToInt(_enemySettings.Health * healthMultiplier);
                int damage = Mathf.RoundToInt(_enemySettings.Damage * damageMultiplier);
                SpawnEnemy(health, damage);
                //Debug.Log($"Spawned Enemy with Health: {health}, Damage: {damage}");
                yield return new WaitForSeconds(1f);
            }
            for (int i = 0; i < wave.bossCount; i++)
            {
                int health = Mathf.RoundToInt(_bossSettings.Health * healthMultiplier);
                int damage = Mathf.RoundToInt(_bossSettings.Damage * damageMultiplier);
                SpawnBoss(health, damage);
                //Debug.Log($"Spawned Boss with Health: {health}, Damage: {damage}");
            }

            yield return new WaitUntil(() => allEnemiesDefeated);
        }

        private void SpawnEnemy(float health, float damage)
        {
            int spawnIndex = UnityEngine.Random.Range(0, _spawnPoints.Length);
            GameObject enemyPrefab = _enemyPrefabs[UnityEngine.Random.Range(0, _enemyPrefabs.Length)];

            GameObject enemyGO = Instantiate(enemyPrefab, _spawnPoints[spawnIndex].position, _spawnPoints[spawnIndex].rotation);
            IEnemy enemy = enemyGO.GetComponent<IEnemy>();

            if (enemy != null)
            {
                enemy.Initialize(GameObject.FindGameObjectWithTag("Player").transform, _enemySettings);
            }
        }

        private void SpawnBoss(float health, float damage)
        {
            int spawnIndex = UnityEngine.Random.Range(0, _spawnPoints.Length);
            GameObject bossGO = Instantiate(_bossPrefab, _spawnPoints[spawnIndex].position, _spawnPoints[spawnIndex].rotation);
            IEnemy boss = bossGO.GetComponent<IEnemy>();

            if (boss != null)
            {
                boss.Initialize(GameObject.FindGameObjectWithTag("Player").transform, _bossSettings);
            }
        }

        private void SpawnActivationObject()
        {
            Instantiate(_activationObjectPrefab, spawnPoint.position, Quaternion.identity);
        }

        //private void SpawnPortal()
        //{
        //    GameObject portalGO = Instantiate(_portalPrefab, portalSpawnPoint.position, Quaternion.identity);
        //    PortalManager portalManager = portalGO.GetComponent<PortalManager>();
        //    portalManager.SetDestinations(playerTeleportDestination, megaBossSpawnPoint);
        //}

        private void UpdateWaveText()
        {
            _waveText.text = "Wave " + (currentWaveIndex + 1);
        }

        private void OnAllEnemiesDefeated()
        {
            allEnemiesDefeated = true;
        }

        private void ShowCenterText(string text)
        {
            _dynamicWaveText.text = text;
            _dynamicWaveText.gameObject.SetActive(true);

            StartCoroutine(HideCenterTextDelay(2f));
        }

        IEnumerator HideCenterTextDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            _dynamicWaveText.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (enemyManager != null)
            {
                enemyManager.OnAllEnemiesDefeated -= OnAllEnemiesDefeated;
            }
        }

        [System.Serializable]
        public class Wave
        {
            public int enemyCount;
            public int bossCount;
        }
    }
}
