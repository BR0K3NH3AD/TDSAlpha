using UnityEngine;
using UnityEngine.EventSystems;


namespace TDS.Scripts.Fifteen
{
    public class ActivationObject : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PuzzleManager puzzleManager;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (puzzleManager != null)
            {
                puzzleManager.ShowPuzzle();
            }
            else
            {
                Debug.LogError("PuzzleManager is not assigned in ActivationObject.");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                puzzleManager.ShowPuzzle();
                Destroy(gameObject);
            }
        }
    }

}
