using Common;
using Lean.Pool;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Coin
{
    public class CoinView : MonoBehaviour, IPointerClickHandler
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
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _amount *= 2;
            LeanPool.Despawn(gameObject);
        }
        
        #endregion

        #region CoinView Methods
        
        private void IncreaseMoney(int changeAmount)
        {
            EventManager.GetInstance().Notify(Events.ChangeMoneyAmout, changeAmount);
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