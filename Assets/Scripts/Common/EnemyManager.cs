using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common
{
    public class EnemyManager
    {
        private static EnemyManager _instance;

        private Dictionary<GameObject, EnemyDistanceCoveredPair> _enemies = new Dictionary<GameObject, EnemyDistanceCoveredPair>();
    
        private readonly object _locker = new object();
    
        private EnemyManager()
        {
            
        }

        public static EnemyManager GetInstance()
        {
            if (_instance == null)
                _instance = new EnemyManager();
            
            return _instance;
        }


        public void AddEnemy(GameObject enemy)
        {
            lock (_locker)
            {
                _enemies.Add(enemy, new EnemyDistanceCoveredPair(enemy, 0));
            }
        }

        public void RemoveEnemy(GameObject enemy)
        {
            lock (_locker)
            {
                _enemies.Remove(enemy);
            }
        }

        public void UpdateEnemy(GameObject enemy, float distance)
        {
            lock (_locker)
            {
                _enemies[enemy].DistanceCovered = distance;
            }
        }

        public GameObject GetMostDistanceCoveredEnemyInRange(Vector2 position, float range)
        {
            if (_enemies.Count <= 0) return null;
            
            return _enemies.Values
                .Where(e => (Vector2.Distance(e.Enemy.transform.position, position) < range))
                .OrderByDescending(e => e.DistanceCovered)
                .Select(e => e.Enemy)
                .FirstOrDefault();
        }

        public GameObject GetClosestEnemyInRange(Vector2 position, float range)
        {
            if (_enemies.Count <= 0) return null;
            
            return _enemies.Values
                .Where(e => (Vector2.Distance(e.Enemy.transform.position, position) < range))
                .OrderBy(e => Vector2.Distance(e.Enemy.transform.position, position))
                .Select(e => e.Enemy)
                .FirstOrDefault();
        }
    
        public void Destroy()
        {
            lock (_locker)
            {
                _enemies.Clear();
            }
            
            _instance = null;
        }
    
    }
    
    internal class EnemyDistanceCoveredPair
    {
        public GameObject Enemy;
        public float DistanceCovered;

        public EnemyDistanceCoveredPair(GameObject enemy, float distanceCovered)
        {
            Enemy = enemy;
            DistanceCovered = distanceCovered;
        }
    }
}
