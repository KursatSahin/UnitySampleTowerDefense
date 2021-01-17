using System;
using System.Collections.Generic;
using Common;
using Shells;
using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected EnemyBase enemyBase;

        [SerializeField] protected GameObject _enemyView;
        [SerializeField] protected float _health;
        [SerializeField] protected Slider _healthBar;

        protected Queue<Waypoint> _movementPath;
        protected bool _isDied = false;
        protected int _lastPassedWaypointIndex = 0;

        #region Unity Events
        
        protected virtual void OnEnable()
        {
            Init();
        }
        
        protected abstract void OnTriggerEnter2D(Collider2D other);
        
        #endregion

        #region Enemy Methods
        
        protected virtual void Init()
        {
            _isDied = false;
            _healthBar.value = _healthBar.maxValue = _health = enemyBase.MaxHealth;
            _movementPath = new Queue<Waypoint>();
        }
        
        protected virtual void UpdateRemainingLife(int updateAmount)
        {
            EventManager.GetInstance().Notify(Events.ChangeRemainingLifeAmount, updateAmount);
        }
        
        protected abstract void AddWaypointToMovementPath(Waypoint waypoint);

        protected abstract void GetDamaged(ShellBase shell);
        protected abstract void Die(bool isKilled = false, float delay = 0);
        protected abstract void Move();

        #endregion
    }
}