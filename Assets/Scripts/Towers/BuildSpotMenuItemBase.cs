using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = "Assets", menuName = "TD Assets/BuildSpotMenuItemBase", order = 1)]
    public class BuildSpotMenuItemBase : ScriptableObject
    {
        public string UnitName;
        public int UnitPrice;
        public int UnitMaxHealth;
        public Transform UnitPrefab;
    }
}

