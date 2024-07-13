using System.Collections;
using System.Collections.Generic;
using TDS.Scripts.UI;
using UnityEngine;

namespace TDS.Scripts.LastMegaBoss
{
    public class MegaBoss : MonoBehaviour
    {
        [Header("Boss HealthPoint Settings"), Space]
        [SerializeField] private int phase1Health;
        [SerializeField] private int phase2Health;
        [SerializeField] private int phase3Health;

        [Space, Header("Boss PhaseSprites"), Space]
        [SerializeField] private Sprite phase1Sprite;
        [SerializeField] private Sprite phase2Sprite;
        [SerializeField] private Sprite phase3Sprite;

        [Space, Header("Boss Prefabs"), Space]
        [SerializeField] private GameObject fireBallPrefab;
        [SerializeField] private GameObject lightningPrefab;
        [SerializeField] private GameObject blackCirclePrefab;

        [Space, Header("Boos Movement & Attack Settings"), Space]
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float chargeSpeed = 10f;
        [SerializeField] private int BossCollDamage = 10;

        [Space, Header("Teleport&Attack Settings"), Space]
        [SerializeField] private float blinkInterval = 0.02f;
        [SerializeField] private float swordCooldown = 0.5f;
        [SerializeField] private int swordDamage = 20;

        [Space, Header("SpawnFireBallS Settings"), Space]
        [SerializeField] private Transform[] fireBallSpawnPoints;
        [SerializeField] private float fireBallCooldown = 0.5f;

        [Space, Header("LighningAttack Settings"), Space]
        [SerializeField] private int lightningDamage = 30;
        [SerializeField] private float lightningCooldown = 1f;
        [SerializeField] private int numberOfLightnings;
        [SerializeField] private float lightningDamageRadius;

        [Space, Header("Boss AudioClips")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip fireBallAttackSound;
        [SerializeField] private AudioClip lightningSound;
        [SerializeField] private AudioClip blinkAndAttackSound;
        [SerializeField] private AudioClip ChargeAndAttack;

        [Space, Header("Other"), Space]
        [SerializeField] private float DelayAfterInit = 1f;
        [SerializeField] private Transform swordHitBox;
        [SerializeField] private LayerMask playerLayer;

        private int currentHealth;
        
        private bool isAttacking;
        private bool isPhase2;
        private bool isPhase3;

        private SpriteRenderer spriteRenderer;
        private MegaBossHealthBar healthBar;
        private Transform playerTransform;
        private Animator animator;

        public delegate void MegaBossSpawned();
        public static event MegaBossSpawned OnMegaBossSpawned;

        public delegate void MegaBossDeath();
        public static event MegaBossDeath OnMegaBossDeath;

        private void Awake()
        {
            swordHitBox.gameObject.SetActive(false);
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            currentHealth = phase1Health; 
            UpdateSprite();

            healthBar = FindObjectOfType<MegaBossHealthBar>();
            if (healthBar != null)
            {
                healthBar.SetMaxHealth(currentHealth);
            }

            OnMegaBossSpawned?.Invoke();

            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            Invoke("StartPhaseOne", DelayAfterInit);
        }

        #region BossPhaseRoutine
        private IEnumerator PhaseOneAttackRoutine()
        {
            while (!isPhase2 && !isPhase3)
            {
                if (!isAttacking)
                {
                    Move(true);
                    int attackType = Random.Range(0, 2);

                    switch (attackType)
                    {
                        case 0:
                            yield return StartCoroutine(TeleportAndAttack());
                            break;
                        case 1:
                            yield return StartCoroutine(SpawnFireBalls());
                            break;
                    }

                    yield return new WaitForSeconds(Random.Range(1f, 3f));
                }
                else
                {
                    yield return null;
                }
            }
        }

        private IEnumerator PhaseTwoAttackRoutine()
        {
            while (isPhase2 && !isPhase3)
            {
                if (!isAttacking)
                {
                    Move(true);
                    int attackType = Random.Range(0, 2);

                    switch (attackType)
                    {
                        case 0:
                            yield return StartCoroutine(ChargeAttack());
                            break;
                        case 1:
                            yield return StartCoroutine(CastLightning());
                            break;
                    }

                    yield return new WaitForSeconds(Random.Range(1f, 3f));
                }
                else
                {
                    yield return null;
                }
            }
        }

        private IEnumerator PhaseThreeAttackRoutine()
        {
            while (isPhase3)
            {
                if (!isAttacking)
                {
                    Move(true);
                    
                    int attackType = Random.Range(0, 4);

                    switch (attackType)
                    {
                        case 0:
                            yield return StartCoroutine(TeleportAndAttack());
                            break;

                        case 1:
                            yield return StartCoroutine(SpawnFireBalls());
                            break;
                        case 2:
                            yield return StartCoroutine(ChargeAttack());
                            break;
                        case 3:
                            yield return StartCoroutine(CastLightning());
                            break;
                    }
                    yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
                }
                else
                {
                    yield return null;
                }
            }
        }

        #endregion 

        #region FirstBossPhase

        private IEnumerator TeleportAndAttack()
        {
            audioSource.PlayOneShot(blinkAndAttackSound);
            isAttacking = true;
            Move(false);


            Vector3 playerPosition = playerTransform.position;

            Vector3 behindPlayerPosition = playerPosition + (playerPosition - transform.position).normalized;

            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

            yield return StartCoroutine(Blink(1f));

            transform.position = behindPlayerPosition;

            UpdateMoveSprite(directionToPlayer);

            yield return new WaitForSeconds(blinkInterval);

            swordHitBox.gameObject.SetActive(true);

            animator.SetTrigger("Attack");

            swordHitBox.gameObject.SetActive(false);

            yield return new WaitForSeconds(swordCooldown);

            isAttacking = false;
        }

        private IEnumerator SpawnFireBalls()
        {
            audioSource.PlayOneShot(fireBallAttackSound);
            isAttacking = true;
            Move(false);

            animator.SetTrigger("Stop");

            yield return new WaitForSeconds(2f);

            animator.SetTrigger("CastFireball");

            foreach (var spawnPoint in fireBallSpawnPoints)
            {
                Vector2 direction = (spawnPoint.position - transform.position).normalized;
                GameObject fireball = Instantiate(fireBallPrefab, spawnPoint.position, Quaternion.identity);
                fireball.GetComponent<Fireball>().Initialize(direction);
            }

            yield return new WaitForSeconds(fireBallCooldown);

            isAttacking = false;
        }

        private IEnumerator Blink(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                yield return new WaitForSeconds(blinkInterval);
                elapsedTime += blinkInterval;
            }

            spriteRenderer.enabled = true;
        }

        #endregion 

        #region SecondBossPhase

        private IEnumerator ChargeAttack()
        {
            audioSource.PlayOneShot(ChargeAndAttack);
            isAttacking = true;
            animator.SetBool("isMoving", true);
            Move(false);



            Vector3 playerPosition = playerTransform.position;
            Vector3 direction = (playerPosition - transform.position).normalized;

            UpdateMoveSprite(direction);

            float elapsedTime = 0f;
            float maxChargeDuration = 5f;
            animator.SetTrigger("Charge");

            yield return new WaitForSeconds(0.1f);

            swordHitBox.gameObject.SetActive(true);

            while (elapsedTime < maxChargeDuration)
            {
                transform.position += direction * chargeSpeed * Time.deltaTime;
                elapsedTime += Time.deltaTime;

                if (Vector3.Distance(transform.position, playerPosition) < 0.5f)
                {
                    break;
                }

                yield return null;
            }

            swordHitBox.gameObject.SetActive(false);

            yield return new WaitForSeconds(swordCooldown);

            isAttacking = false;
            Move(false);
            animator.SetBool("isMoving", false);
        }

        private IEnumerator CastLightning()
        {
            isAttacking = true;
            animator.SetBool("isMoving", true);
            Move(false);



            List<GameObject> blackCircleList = new List<GameObject>();

            for (int i = 0; i < numberOfLightnings; i++)
            {
                Vector3 lightningPosition = new Vector3(Random.Range(-39f, 36f), Random.Range(-32f, 32f), -1);
                GameObject blackCircle = Instantiate(blackCirclePrefab, lightningPosition, Quaternion.identity);
                blackCircleList.Add(blackCircle);
            }

            yield return new WaitForSeconds(2f);

            foreach (GameObject blackCircle in blackCircleList)
            {
                Destroy(blackCircle);
            }
            audioSource.PlayOneShot(lightningSound);

            foreach (GameObject blackCircle in blackCircleList)
            {
                GameObject lightning = Instantiate(lightningPrefab, blackCircle.transform.position, Quaternion.identity);
                Destroy(lightning, 2f);
            }

            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, lightningDamageRadius, playerLayer);
            foreach (Collider2D playerCollider in hitPlayers)
            {
                PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamagePlayer(lightningDamage);
                }
            }

            yield return new WaitForSeconds(lightningCooldown);

            isAttacking = false;
            Move(false);
            animator.SetBool("isMoving", false);

        }

        #endregion

        #region Rescurrect

        private IEnumerator PlayDeathAndResurrectionAnimationToPhase2()
        {
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            spriteRenderer.sprite = phase2Sprite;

            animator.SetTrigger("Resurrect");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            StartCoroutine(PhaseTwoAttackRoutine());
        }

        private IEnumerator PlayDeathAndResurrectionAnimationToPhase3()
        {
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            spriteRenderer.sprite = phase3Sprite;

            animator.SetTrigger("Resurrect");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            StartCoroutine(PhaseThreeAttackRoutine());
        }

        #endregion

        #region Damage & SpriteLogick

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player") && swordHitBox.gameObject.activeSelf)
            {
                PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamagePlayer(swordDamage);
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
        if (collision.collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamagePlayer(BossCollDamage);
                }
            }
        }

        private void Update()
        {
            Move(true);
            

            if (currentHealth <= phase2Health && !isPhase2 && !isPhase3)
            {
                TransitionToPhase2();
            }
            else if (currentHealth <= phase3Health && !isPhase3)
            {
                TransitionToPhase3();
            }
        }

        private void UpdateSprite()
        {
            if (isPhase3)
            {
                spriteRenderer.sprite = phase3Sprite;
            }
            else if (isPhase2)
            {
                spriteRenderer.sprite = phase2Sprite;
            }
            else
            {
                spriteRenderer.sprite = phase1Sprite;
            }
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth);
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        #endregion

        #region DieLogick

        private void Die()
        {
            StartCoroutine(DieCoroutine());
        }

        private IEnumerator DieCoroutine()
        {
            animator.SetTrigger("Die");

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            OnMegaBossDeath?.Invoke();
        }

        private void Move(bool shouldMove)
        {
            if (shouldMove && playerTransform != null && !isAttacking)
            {
                animator.SetBool("isMoving", true);

                Vector2 direction = (playerTransform.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                UpdateMoveSprite(direction);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
        
        private void UpdateMoveSprite(Vector3 direction)
        {
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        #endregion

        #region TransitionToPhase & StartPhase

        private void TransitionToPhase2()
        {
            isPhase2 = true;
            animator.SetInteger("bossPhase", 2);
            StopCoroutine(PhaseOneAttackRoutine());
            StartCoroutine(PlayDeathAndResurrectionAnimationToPhase2());
            Invoke("StartPhaseTwo", 1f);
        }

        private void TransitionToPhase3()
        {
            isPhase3 = true;
            animator.SetInteger("bossPhase", 3);
            StopCoroutine(PhaseTwoAttackRoutine());
            StartCoroutine(PlayDeathAndResurrectionAnimationToPhase3());
            Invoke("StartPhaseThree", 1f);
        }

        private void StartPhaseOne()
        {
            StartCoroutine(PhaseOneAttackRoutine());
        }

        private void StartPhaseTwo()
        {
            StartCoroutine(PhaseTwoAttackRoutine()); 
        }

        private void StartPhaseThree()
        {
            StartCoroutine(PhaseThreeAttackRoutine()); 
        }

        #endregion
    }
}