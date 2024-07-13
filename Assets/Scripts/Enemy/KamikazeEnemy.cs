using System.Collections;
using TDS.Scripts.UI;
using UnityEngine;


namespace TDS.Scripts.Enemy
{
    public class KamikazeEnemy : BaseEnemy
    {
        [SerializeField] private float _triggerDistance = 5f;
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private int _explosionDamage = 50;

        [SerializeField] private float _blinkDuration = 2f;
        [SerializeField] private float _blinkInterval = 0.3f;
        [SerializeField] private float _elapsedTime = 0f;

        [SerializeField] private GameObject explosionPrefab;

        private bool isVisible = true;
        private bool isTriggered = false;

        protected override void FixedUpdate()
        {
            if(!isDead)
            {
                float distacneToPlayer = Vector2.Distance(transform.position, player.position);
                if(distacneToPlayer <= _triggerDistance && !isTriggered)
                {
                    StartCoroutine(StartExposionSequence());
                    isTriggered = false;
                }
                else
                {
                    Move();
                }
            }
        }

        private IEnumerator StartExposionSequence()
        {
            StopMovement();

            while(_elapsedTime < _blinkDuration)
            {
                spriteRenderer.enabled = isVisible;
                isVisible = !isVisible;
                _elapsedTime += _blinkInterval;
                yield return new WaitForSeconds(_blinkInterval);
            }

            Explode();
        }

        private void StopMovement()
        {
            _enemySpeed = 0f;
        }

        private void Explode()
        {
            if (explosionPrefab != null)
            {
                GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosionInstance, 1f);
            }

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
            foreach(var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                    if(playerHealth != null)
                    {
                        playerHealth.TakeDamagePlayer(_explosionDamage);
                    }
                }
            }

            Die();
        }

        public override void Die()
        {
            base.Die();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _triggerDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }

}
