using TDS.Scripts.UI;
using UnityEngine;

namespace TDS.Scripts.Enemy
{
    public class BulletRangeEnemy : MonoBehaviour
    {
        [SerializeField] private float _bulletSpeed = 10f;

        private int damage;
        private Vector2 direction;

        //private Vector2 target;


        //public void Initialize(Vector2 target, int damage)
        //{
        //    this.target = target;
        //    this.damage = damage;
        //}

        public void Initialize(Vector2 direction, int damage)
        {
            this.direction = direction.normalized;
            this.damage = damage;
        }

        //private void Update()
        //{
        //    float step = _bulletSpeed * Time.deltaTime;
        //    transform.position = Vector2.MoveTowards(transform.position, target, step);

        //    if((Vector2)transform.position == target)
        //    {
        //        Destroy(gameObject);
        //    }
        //}

        private void Update()
        {
            transform.Translate(direction * _bulletSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if(playerHealth != null)
                {
                    playerHealth.TakeDamagePlayer(damage);
                }
                Destroy(gameObject);
            }
            else if (other.CompareTag("GameMap"))
            {
                Destroy(gameObject);
            }
        }
    }

}

