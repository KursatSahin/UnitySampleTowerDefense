using Common;
using Lean.Pool;
using UnityEngine;

namespace Coin
{
    public class CoinGenerator : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] private GameObject _coinPrefab;

        #region Unity Events
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        #endregion

        #region CoinGenerator Event Callbacks

        private void OnGenerateCoin(object data)
        {
            var coinData = data as CoinData;
            var coinView = LeanPool.Spawn(_coinPrefab).GetComponent<CoinView>();
            
            //Debug.Log("Coin is generated");

            coinView.transform.position = coinData.Position;
            coinView.Amount = coinData.Amount;
            LeanPool.Despawn(coinView.gameObject, 3f);
        }
        
        #endregion

        #region IEventManagerHandling Methods

        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.GenerateCoin, OnGenerateCoin);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.GenerateCoin, OnGenerateCoin);
        }

        #endregion
    }
}