using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TDS.Scripts.Enemy
{
    [CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/EnemySettings", order = 1)]
    public class EnemySettings : ScriptableObject
    {
        public float Speed;
        public int Health;
        public int Damage;
        public int Points;
    }

}
