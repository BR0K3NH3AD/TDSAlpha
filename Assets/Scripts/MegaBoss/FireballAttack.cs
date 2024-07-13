using TDS.Scripts.UI;
using UnityEngine;


namespace TDS.Scripts.LastMegaBoss
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] private float fireBallSpeed = 5f;
        [SerializeField] private int fireBallDamage = 10;

        private Vector2 direction;

        public void Initialize(Vector2 direction)
        {
            this.direction = direction.normalized;
        }

        private void Update()
        {
            transform.Translate(direction * fireBallSpeed * Time.deltaTime, Space.World);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamagePlayer(fireBallDamage);
                }
                Destroy(gameObject);
            }
            else if (collision.collider.CompareTag("GameMap"))
            {
                Destroy(gameObject);
            }
        }
    }
}
