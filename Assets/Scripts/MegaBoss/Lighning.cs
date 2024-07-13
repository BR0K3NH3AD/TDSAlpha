using TDS.Scripts.UI;
using UnityEngine;

namespace TDS.Scripts.LastMegaBoss
{
    public class Lightning : MonoBehaviour
    {
        [SerializeField] private int damage = 30;

        private void Start()
        {
            Invoke("DestroySelf", 2f);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if(playerHealth != null)
                {
                    playerHealth.TakeDamagePlayer(damage);
                }
            }
        }

        private void OnDestroySelf()
        {
            Destroy(gameObject);
        }
    }

}
