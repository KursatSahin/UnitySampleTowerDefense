using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using Lean.Pool;
using UnityEngine;

namespace Game
{
    public class WaveGenerator : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] private List<Wave> _waves;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _unitsParent;

        private int _currentWaveNumber = 0;

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
    
        #region WaveGenerator Methods

        public int TotalUnitCount()
        {
            int totalAmount = 0;
        
            foreach (var wave in _waves)
            {
                totalAmount += wave.unitAmount;
            }
        
            return totalAmount;
        }
    
        private IEnumerator GenerateWave(Wave wave)
        {
            for (int i = 0; i < wave.unitAmount; i++)
            {
                var soldier = LeanPool.Spawn(wave.unitPrefab, _spawnPoint.position, Quaternion.Euler(0,0,0), _unitsParent);
                EnemyManager.GetInstance().AddEnemy(soldier);
                yield return new WaitForSeconds(wave.delayTimeBetweenSpawns);
            }
        }    

        #endregion
    
        #region WaveGenerator Event Callbacks

        private void OnTimeTickUpdated(object data)
        {
            var time = (float) data;

            var subList = _waves.FindAll(x => x.startingTime < time);

            if (subList.Count > 0)
            {
                _currentWaveNumber++;
                EventManager.GetInstance().Notify(Events.WaveStartedToBeGenerated, _currentWaveNumber);
            
                foreach (var waveItem in subList)
                {
                    StartCoroutine(GenerateWave(waveItem));
                    _waves.Remove(waveItem);
                }
            }
        }

        #endregion
    
        #region EventManagerHandling Methods

        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.TimeTickUpdated, OnTimeTickUpdated);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.TimeTickUpdated, OnTimeTickUpdated);
        }
    
        #endregion
    }
    
    [System.Serializable]
    public sealed class Wave
    {
        public int unitAmount;
        public GameObject unitPrefab;
        public float startingTime;
        public float delayTimeBetweenSpawns;
    }
}