using Common;
using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = "Assets", menuName = "TD Assets/TowerBase", order = 2)]
    public class TowerBase : ScriptableObject
    {
        [Header("Catalog Data")]
        public string Name;
        public int Price;
        public GameObject Prefab;

        [Header("Health Data")]
        public int MaxHealth;
        
        [Header("Combat Data")]
        public GameObject ShellPrefab;
        public float FireRate;
        public float Range;
        public float BulletSpeed;
        public float Damage;
        
        public ShootingTargetType TargetType;

        [Range(10, 90)] public float RotationSpeed = 10;

        private void OnValidate()
        {
            Price = Mathf.Clamp(Price, 1, int.MaxValue);
            MaxHealth = Mathf.Clamp(MaxHealth, 1, int.MaxValue);
            FireRate = Mathf.Clamp(FireRate, Utils.MinPositiveFloat, float.MaxValue);
            Range = Mathf.Clamp(Range, Utils.MinPositiveFloat, float.MaxValue);
            BulletSpeed = Mathf.Clamp(BulletSpeed, Utils.MinPositiveFloat, float.MaxValue);
            Damage = Mathf.Clamp(Damage, Utils.MinPositiveFloat, float.MaxValue);
            
            if (Prefab == null) Debug.LogError($"{nameof(TowerBase)}.{nameof(Prefab)} is not assigned");
            if (ShellPrefab == null) Debug.LogError($"{nameof(TowerBase)}.{nameof(ShellPrefab)} is not assigned");
        }
    }

    public enum ShootingTargetType
    {
        First,
        Closest
    }
}