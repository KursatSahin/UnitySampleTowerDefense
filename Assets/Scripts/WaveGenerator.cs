using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using Lean.Pool;
using UnityEngine;
using Utils;

public class WaveGenerator : MonoBehaviour, IEventManagerHandling
{
    //public static WaveGenerator Instance;
    
    [SerializeField] private List<Wave> waves;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform unitsParent;

    private int currentWaveNumber = 0;

    public int TotalUnitCount()
    {
        int totalAmount = 0;
        
        foreach (var wave in waves)
        {
            totalAmount += wave.unitAmount;
        }
        
        return totalAmount;
    }

    private void Awake()
    {
        //Instance = this;
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.GetInstance().Subscribe(Events.TimeTickUpdated, OnTimeTickUpdated);
    }

    public void UnsubscribeEvents()
    {
        EventManager.GetInstance().Unsubscribe(Events.TimeTickUpdated, OnTimeTickUpdated);
    }

    private void OnTimeTickUpdated(object data)
    {
        var time = (float) data;

        var subList = waves.FindAll(x => x.startingTime < time);

        if (subList.Count > 0)
        {
            currentWaveNumber++;
            EventManager.GetInstance().Notify(Events.WaveStartedToBeGenerated, currentWaveNumber);
            
            foreach (var waveItem in subList)
            {
                StartCoroutine(GenerateWave(waveItem));
                waves.Remove(waveItem);
            }
        }
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
}
    
[System.Serializable]
public sealed class Wave
{
    public int unitAmount;
    public GameObject unitPrefab;
    public float startingTime;
    public float delayTimeBetweenSpawns;
}