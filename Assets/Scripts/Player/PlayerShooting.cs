using UnityEngine;

namespace TDS.Scripts.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        private GameObject bulletPrefab;
        private Transform firePoint;
        private float fireSpeed;
        private int damage;
        private float lastShotTime;
        private Transform weaponTransform;

        public GameObject BulletPrefab => bulletPrefab;
        public Transform FirePoint => firePoint;
        public float FireSpeed => fireSpeed;
        public int Damage => damage;

        public void Initialize(GameObject bulletPrefab, Transform firePoint, float fireSpeed, int damage, Transform weaponTransform)
        {
            this.bulletPrefab = bulletPrefab;
            this.firePoint = firePoint;
            this.fireSpeed = fireSpeed;
            this.damage = damage;
            this.weaponTransform = weaponTransform;
        }

        public void HandleShooting(Transform target)
        {
            if (target == null || bulletPrefab == null || firePoint == null) return;

            if (Time.time - lastShotTime > 1f / fireSpeed)
            {
                AimAtTarget(target);
                Shoot(target);
                lastShotTime = Time.time;
            }
        }

        private void AimAtTarget(Transform target)
        {
            Vector2 direction = (target.position - weaponTransform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        private void Shoot(Transform target)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetDamage(damage);
                bullet.Seek(target);
            }
        }

        public void SetDamage(int newDamage)
        {
            damage = newDamage;
        }

        public void SwitchWeapon(Transform newWeaponTransform, Transform newFirePoint, float newFireSpeed, int newDamage)
        {
            this.weaponTransform.gameObject.SetActive(false);
            this.weaponTransform = newWeaponTransform;
            this.firePoint = newFirePoint;
            this.weaponTransform.gameObject.SetActive(true);
            this.fireSpeed = newFireSpeed;
            this.damage = newDamage;
        }
    }
}
