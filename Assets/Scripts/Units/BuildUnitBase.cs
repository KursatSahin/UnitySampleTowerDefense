using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "Assets", menuName = "UnitBase/BuildUnitBase", order = 1)]
    public class BuildUnitBase : ScriptableObject
    {
        public string unitName;
        public int unitPrice;
        public int unitMaxHealth;
        public Transform unitPrefab;
    }
}

