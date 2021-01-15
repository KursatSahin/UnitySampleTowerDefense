using System;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using Lean.Pool;
using Units;
using UnityEngine;

public class Tower : MonoBehaviour, IEventManagerHandling
{
    [SerializeField] private TowerBase _towerBase;

    [SerializeField] private GameObject _bulletFireEffect;

    [SerializeField] private GameObject _rangeCircle;

    private float _timeToShoot = 0.0f;

    private Vector3 _rangeCircleScaleValue;
    
    #region Unity Events
    
    private void OnEnable()
    {
        // Set Range
        _rangeCircleScaleValue = new Vector3(_towerBase.Range, _towerBase.Range, _towerBase.Range);
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void OnMouseDown()
    {
        if (!_rangeCircle.activeSelf)
        {
            SetVisibilityOfRangeCircle(true);
        }
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
            
            if (_timeToShoot < 0 && Mathf.Abs(angle) < 45)
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
                
                _timeToShoot = _towerBase.FireRate;

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

        if (_bulletFireEffect != null && _timeToShoot < _towerBase.FireRate * 0.7f && _bulletFireEffect.activeSelf)
        {
            _bulletFireEffect.SetActive(false);
        }
        
        _timeToShoot -= Time.deltaTime;
    }
    
    #endregion

    #region Tower Methods

    private float TurnToEnemy(Vector2 position)
    {
        var direction = position - (Vector2)transform.position;
        var angle = Utils.Angle(direction, transform.up);
        transform.Rotate(0, 0, Mathf.Clamp(angle, -10, 10) * _towerBase.RotationSpeed * Time.deltaTime);
        return angle;   
    }

    private void SetVisibilityOfRangeCircle(bool status)
    {
        if (status )
        {
            _rangeCircle.transform.DOScale(_rangeCircleScaleValue, .3f).
                OnStart(() =>
                {
                    _rangeCircle.SetActive(true);
                    _rangeCircle.GetComponent<SpriteRenderer>().DOFade(.4f, .3f);
                });
        }
        else
        {
            _rangeCircle.transform.DOScale(Vector3.one, .3f).
                OnStart(() =>
                {
                    _rangeCircle.GetComponent<SpriteRenderer>().DOFade(0, .3f);
                }).
                OnComplete((() =>
                {
                    _rangeCircle.SetActive(false);
                }));
        }
    }

    #endregion

    #region Towers Event Callbacks

    private void OnClickOutsideOfInteractiveArea(object obj)
    {
        if (_rangeCircle.activeSelf)
        {
            SetVisibilityOfRangeCircle(false);
        }
    }
    
    #endregion
    
    #region IEventManagerHandling Methods

    public void SubscribeEvents()
    {
        EventManager.GetInstance().Subscribe(Events.ClickOutsideOfInteractiveArea, OnClickOutsideOfInteractiveArea);
    }

    public void UnsubscribeEvents()
    {
        EventManager.GetInstance().Unsubscribe(Events.ClickOutsideOfInteractiveArea, OnClickOutsideOfInteractiveArea);
    }

    #endregion
}