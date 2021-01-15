using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using Lean.Pool;
using UnityEngine;

public class WaveGenerator : MonoBehaviour, IEventManagerHandling
{
    [SerializeField] private List<Wave> waves;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform unitsParent;

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
        
        foreach (var wave in waves)
        {
            totalAmount += wave.unitAmount;
        }
        
        return totalAmount;
    }
    
    private IEnumerator GenerateWave(Wave wave)
    {
        for (int i = 0; i < wave.unitAmount; i++)
        {
            var soldier = LeanPool.Spawn(wave.unitPrefab, spawnPoint.position, Quaternion.Euler(0,0,0), unitsParent);
            EnemyManager.GetInstance().AddEnemy(soldier);
            yield return new WaitForSeconds(wave.delayTimeBetweenSpawns);
        }
    }    

    #endregion
    
    #region WaveGenerator Event Callbacks

    private void OnTimeTickUpdated(object data)
    {
        var time = (float) data;

        var subList = waves.FindAll(x => x.startingTime < time);

        if (subList.Count > 0)
        {
            _currentWaveNumber++;
            EventManager.GetInstance().Notify(Events.WaveStartedToBeGenerated, _currentWaveNumber);
            
            foreach (var waveItem in subList)
            {
                StartCoroutine(GenerateWave(waveItem));
                waves.Remove(waveItem);
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