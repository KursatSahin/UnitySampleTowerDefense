using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "Assets", menuName = "UnitBase/TowerBase", order = 2)]
    public class TowerBase : ScriptableObject
    {
        public GameObject ShellPrefab;
    
        public float FireRate;
        public float Range;
        public float BulletSpeed;
        public float Damage;
        
        public ShootingTargetType TargetType;

        [Range(10, 90)] public float RotationSpeed = 10;
    }

    public enum ShootingTargetType
    {
        First,
        Closest
    }
}