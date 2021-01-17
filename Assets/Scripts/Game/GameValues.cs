using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Assets", menuName = "TD Assets/GameValues", order = 0)]
    public class GameValues : ScriptableObject
    {
        public int InitialLives;
        public int InitialMoney;
        
        private void OnValidate()
        {
            InitialLives = Mathf.Clamp(InitialLives, 1, int.MaxValue);
            InitialMoney = Mathf.Clamp(InitialMoney, 1, int.MaxValue);
        }
    }
}