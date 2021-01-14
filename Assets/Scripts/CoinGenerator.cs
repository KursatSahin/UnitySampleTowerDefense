using System;
using Lean.Pool;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Coin
{
    public class CoinGenerator : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] private GameObject _coinPrefab;
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        private void OnGenerateCoin(object data)
        {
            var coinData = data as CoinData;
            var coinView = LeanPool.Spawn(_coinPrefab).GetComponent<CoinView>();
            
            //Debug.Log("Coin is generated");

            coinView.transform.position = coinData.Position;
            coinView.Amout = coinData.Amount;
            LeanPool.Despawn(coinView.gameObject, 2f);
        }
        
        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.GenerateCoin, OnGenerateCoin);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.GenerateCoin, OnGenerateCoin);
        }
    }
}