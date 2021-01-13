using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "Assets", menuName = "UnitBase/EnemyBase", order = 0)]
    public class EnemyBase : ScriptableObject
    {
        public float movementSpeed;
        public float maxHealth;
        public Transform prefab;
    }
}