using Common;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "Assets", menuName = "TD Assets/EnemyBase", order = 0)]
    public class EnemyBase : ScriptableObject
    {
        [Range(10,200)]public float MovementSpeed;
        public float MaxHealth;
        public int Prize;
        public Transform Prefab;

        private void OnValidate()
        {
            MaxHealth = Mathf.Clamp(MaxHealth, Utils.MinPositiveFloat, float.MaxValue);
            Prize = Mathf.Clamp(Prize, 1, int.MaxValue);
            if (Prefab == null) Debug.LogError($"{nameof(EnemyBase)}.{nameof(Prefab)} is not assigned");
        }
    }
}