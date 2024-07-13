using UnityEngine;
using TDS.Scripts.UI;
using TDS.Scripts.Managers;


namespace TDS.Scripts.Enemy
{
    public class BaseEnemy : MonoBehaviour, IEnemy
    {
        protected float _enemySpeed;
        protected int _enemyHealth;
        protected int _enemyDamage;
        protected int _enemyScore;

        public int EnemyDamage => _enemyDamage;

        protected Transform player;
        protected Animator animator;
        protected bool isDead = false;
        protected SpriteRenderer spriteRenderer;

        public virtual void Initialize(Transform player, EnemySettings settings)
        {
            this.player = player;
            _enemySpeed = settings.Speed;
            _enemyHealth = settings.Health;
            _enemyDamage = settings.Damage;
            _enemyScore = settings.Points;

            //Debug.Log($"Enemy initialized with Health: {_enemyHealth}, Damage: {_enemyDamage}");

        }

        //public virtual void Initialize(Transform player, EnemySettings settings, int health, int damage)
        //{
        //    this.player = player;
        //    _enemySpeed = settings.Speed;
        //    _enemyHealth = health;
        //    _enemyDamage = damage;
        //    _enemyScore = settings.Points;

        //    //Debug.Log($"Enemy initialized with Health: {_enemyHealth}, Damage: {_enemyDamage}");

        //}

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
            if (enemyManager != null)
            {
                enemyManager.RegisterEnemy(this);
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!isDead)
            {
                Move();
            }
        }

        public virtual void Move()
        {
            if (player != null)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.Translate(direction * _enemySpeed * Time.deltaTime);

                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else if (direction.x < 0)
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
        }

        public virtual void TakeDamage(int damage)
        {
            _enemyHealth -= damage;
            if (_enemyHealth <= 0 && !isDead)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            if (isDead) return;

            isDead = true;
            animator.SetBool("isDie", true);
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 0.3f);

            PlayerManager playerManager = FindObjectOfType<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.AddScore(_enemyScore);
            }

            EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
            if (enemyManager != null)
            {
                enemyManager.UnregisterEnemy(this);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamagePlayer(_enemyDamage);
                }
            }
        }

        public int GetScore()
        {
            return _enemyScore;
        }
    }
}
