using System;
using System.Runtime.CompilerServices;
using Lean.Pool;
using UnityEngine;
using Utils;

namespace Coin
{
    public class CoinView : MonoBehaviour
    {
        private int _amout = 0;

        public int Amout
        {
            get
            {
                return _amout;
            }
            set
            {
                _amout = value;
            }
        }

        private void OnDisable()
        {
            IncreaseMoney(_amout);
        }

        private void IncreaseMoney(int changeAmount)
        {
            EventManager.GetInstance().Notify(Events.UpdateMoney, changeAmount);
        }
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