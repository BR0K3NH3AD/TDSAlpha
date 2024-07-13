using UnityEngine;
namespace TDS.Scripts.Enemy
{
    public interface IEnemy
    {
        int GetScore();

        void TakeDamage(int damage);
        void Move();
        void Die();
        void Initialize(Transform player, EnemySettings settings);
    }

}
