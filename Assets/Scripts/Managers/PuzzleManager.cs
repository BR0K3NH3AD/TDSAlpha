using UnityEngine;
using TMPro;
using TDS.Scripts.Managers;

namespace TDS.Scripts.Fifteen
{
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] private GameObject puzzleUI;
        [SerializeField] private TextMeshProUGUI successText;
        [SerializeField] private GameObject portalPrefab;
        [SerializeField] private Transform portalSpawnPoint;
        public Transform playerTeleportDestination;
        public Transform megaBossSpawnPoint;

        public void ShowPuzzle()
        {
            puzzleUI.SetActive(true);
            Time.timeScale = 0f;
        }

        public void OnPuzzleSolved()
        {
            Time.timeScale = 1f;
            puzzleUI.SetActive(false);
            successText.text = "Успех";
            successText.gameObject.SetActive(true);
            GameObject portalGO = Instantiate(portalPrefab, portalSpawnPoint.position, Quaternion.identity);
            PortalManager portalManager = portalGO.GetComponent<PortalManager>();
            portalManager.SetDestinations(playerTeleportDestination, megaBossSpawnPoint);
        }
    }

}

