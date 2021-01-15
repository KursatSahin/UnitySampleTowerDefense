using System;
using System.Collections.Generic;
using Common;
using Lean.Pool;
using Units;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerBase _towerBase;

    [SerializeField] private GameObject _bulletFireEffect;

    [SerializeField] private GameObject _rangeCircle;

    private float timeToShoot = 0.0f;

    private void Awake()
    {
        // Set Range
        var transformLocalScale = _rangeCircle.transform.localScale;
        transformLocalScale.x = transformLocalScale.y = _towerBase.Range;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }

    void FixedUpdate ()
    {
        var enemy = _towerBase.TargetType == ShootingTargetType.First
            ? EnemyManager.GetInstance().GetMostDistanceCoveredEnemyInRange(transform.position, _towerBase.Range)
            : EnemyManager.GetInstance().GetClosestEnemyInRange(transform.position, _towerBase.Range);

        if (enemy != null)
        {
            var angle = TurnToEnemy(enemy.transform.position);
            
            if (timeToShoot < 0 && Mathf.Abs(angle) < 45)
            {
                var shell = LeanPool.Spawn(_towerBase.ShellPrefab);
                shell.transform.position = transform.position; // + (transform.up * 40) 
                shell.transform.rotation = transform.rotation;

                var shellData = shell.GetComponent<ShellBase>();
                shellData.speed = _towerBase.BulletSpeed;
                shellData.range = _towerBase.Range;
                shellData.direction = transform.transform.up;
                shellData.damage = _towerBase.Damage;
                shellData.target = enemy;
                //bulletScript.EnemyTags = _towerBase.enemyTags;
                shellData.gunBarrel = transform;

                shell.SetActive(true);

                //Debug.Log("bullet fired");
                
                timeToShoot = _towerBase.FireRate;

                if(_bulletFireEffect != null) _bulletFireEffect.SetActive(true);
                
                return;
            }
            
        }
        else
        {
            var possibleTarget = EnemyManager.GetInstance().GetClosestEnemyInRange(transform.position, _towerBase.Range * 2);
            if (possibleTarget != null)
            {
                TurnToEnemy(possibleTarget.transform.position);
            }
        }

        if (_bulletFireEffect != null && timeToShoot < _towerBase.FireRate * 0.7f && _bulletFireEffect.activeSelf)
        {
            _bulletFireEffect.SetActive(false);
        }
        
        timeToShoot -= Time.deltaTime;
    }
    
    private float TurnToEnemy(Vector2 position)
    {
        var direction = position - (Vector2)transform.position;
        var angle = Utils.Angle(direction, transform.up);
        transform.Rotate(0, 0, Mathf.Clamp(angle, -10, 10) * _towerBase.RotationSpeed * Time.deltaTime);
        return angle;   
    }
}