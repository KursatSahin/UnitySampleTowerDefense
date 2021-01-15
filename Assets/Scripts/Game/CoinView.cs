using System;
using System.Runtime.CompilerServices;
using Common;
using Lean.Pool;
using UnityEngine;

namespace Coin
{
    public class CoinView : MonoBehaviour
    {
        private int _amount = 0;

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }

        #region Unity Events
        
        private void OnDisable()
        {
            IncreaseMoney(_amount);
        }
        
        #endregion

        #region CoinView Methods
        
        private void IncreaseMoney(int changeAmount)
        {
            EventManager.GetInstance().Notify(Events.UpdateMoney, changeAmount);
        }
        
        #endregion
    }

    public class CoinData
    {
        public CoinData(Vector3 position, int amount)
        {
            Position = position;
            Amount = amount;
        }

        public Vector3 Position;
        public int Amount;
    }
}