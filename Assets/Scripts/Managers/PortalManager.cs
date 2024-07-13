using UnityEngine;

namespace TDS.Scripts.Managers
{
    public class PortalManager : MonoBehaviour
    {
        [SerializeField] private GameObject megaBossPrefab;

        private Transform playerTeleportDestination;
        private Transform megaBossSpawnPoint;

        public void SetDestinations(Transform playerDestination, Transform bossSpawnPoint)
        {
            playerTeleportDestination = playerDestination;
            megaBossSpawnPoint = bossSpawnPoint;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                TeleportPlayer(collision.transform);
                SpawnMegaBoss();
                Destroy(gameObject);
            }
        }

        private void TeleportPlayer(Transform player)
        {
            player.position = playerTeleportDestination.position;
            foreach (Transform child in player)
            {
                child.position += playerTeleportDestination.position - player.position;
            }
        }

        private void SpawnMegaBoss()
        {
            Instantiate(megaBossPrefab, megaBossSpawnPoint.position, megaBossSpawnPoint.rotation);
        }
    }

}
