using System;
using Towers;
using Units;
using UnityEngine;

namespace Common
{
    public static class Utils
    {
        public static float MinPositiveFloat = .001f;
        public static float Angle(Vector2 a, Vector2 b)
        {
            var an = a.normalized;
            var bn = b.normalized;
            var x = an.x * bn.x + an.y * bn.y;
            var y = an.y * bn.x - an.x * bn.y;
            return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        }
        
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        public static int GetRepairPrice(Tower tower)
        {
            var currentTowerHealth = tower.TowerBase.MaxHealth; // temp value
            var factor = (tower.TowerBase.MaxHealth - currentTowerHealth) / tower.TowerBase.MaxHealth;
            var repairPrice = factor * (tower.TowerBase.Price * .5f);

            return (int)repairPrice;
        }
        
        public static int GetSellPrice(Tower tower)
        {
            var sellPrice = (tower.TowerBase.Price * .5f);

            return (int)sellPrice;
        }

        public static int GetUpgradePrice(Tower tower)
        {
            var upgradePrice = (tower.TowerBase.Price * 1.5f);

            return (int)upgradePrice;
        }
    }
}