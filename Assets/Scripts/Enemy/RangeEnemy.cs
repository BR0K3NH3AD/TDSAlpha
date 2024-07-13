using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDS.Scripts.Enemy
{
    public class RangeEnemy : BaseEnemy
    {
        [SerializeField] private float _attackDistance = 10f;
        [SerializeField] private float _fireRate = 1f;

        [SerializeField] private int _bulletDamage;

        [SerializeField] GameObject bulletPrefab;

        [SerializeField] private Transform _firePoint;

        private bool isAttacking = false;
        private float nextFireTime = 0f;
        private float originalSpeed;

        protected override void Start()
        {
            base.Start();
            originalSpeed = _enemySpeed;
        }

        protected override void FixedUpdate()
        {
            if (!isDead)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                if (distanceToPlayer <= _attackDistance)
                {
                    if (!isAttacking)
                    {
                        isAttacking = true;
                        StopMovement();
                        StartCoroutine(ShootAtPlayer());
                    }
                }
                else
                {
                    if (isAttacking)
                    {
                        isAttacking = false;
                    }
                    ResumeMovement();
                    Move();
                }
            }
        }

        private void StopMovement()
        {
            _enemySpeed = 0f;
        }

        private void ResumeMovement()
        {
            _enemySpeed = originalSpeed;
        }

        private IEnumerator ShootAtPlayer()
        {
            while (isAttacking)
            {
                if(Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + 1f/ _fireRate;
                    Shoot();
                }

                yield return null;
            }
        }

        //private void Shoot()
        //{
        //    if(bulletPrefab != null && _firePoint != null)
        //    {
        //        GameObject bulletInstance = Instantiate(bulletPrefab, _firePoint.position, _firePoint.rotation);
        //        BulletRangeEnemy bullet = bulletInstance.GetComponent<BulletRangeEnemy>();
        //        if(bullet != null)
        //        {
        //            bullet.Initialize(player.position, _bulletDamage);
        //        }
        //    } 
        //}

        private void Shoot()
        {
            if (bulletPrefab != null && _firePoint != null)
            {
                GameObject bulletInstance = Instantiate(bulletPrefab, _firePoint.position, _firePoint.rotation);
                BulletRangeEnemy bullet = bulletInstance.GetComponent<BulletRangeEnemy>();
                if (bullet != null)
                {
                    Vector2 direction = player.position - _firePoint.position;
                    bullet.Initialize(direction, _bulletDamage);
                }
            }
        }
    }

}

