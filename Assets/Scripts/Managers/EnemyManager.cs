using System.Collections.Generic;
using UnityEngine;
using TDS.Scripts.Enemy;

namespace TDS.Scripts.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        private List<IEnemy> _enemies = new List<IEnemy>();

        public delegate void AllEnemiesDefeatedHandler();
        public event AllEnemiesDefeatedHandler OnAllEnemiesDefeated;

        public void RegisterEnemy(IEnemy enemy)
        {
            if (!_enemies.Contains(enemy))
            {
                _enemies.Add(enemy);
            }
        }

        public void UnregisterEnemy(IEnemy enemy)
        {
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
            }

            CheckAllEnemiesDefeated();
        }

        private void CheckAllEnemiesDefeated()
        {
            if (_enemies.Count == 0)
            {
                OnAllEnemiesDefeated?.Invoke();
            }
        }

        public void DamageAllEnemies(int damage)
        {
            foreach (var enemy in _enemies)
            {
                enemy.TakeDamage(damage);
            }
        }

        public void MoveAllEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Move();
            }
        }

        public void KillAllEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Die();
            }
        }
    }
}
