using System;
using System.Collections.Generic;
using System.Reflection;
using Coin;
using DG.Tweening;
using Lean.Pool;
using Lean.Touch;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Utils;

namespace Units
{
    public class Soldier : Enemy
    {
        [SerializeField] private GameObject _soldierView;
        [SerializeField] private float _health;
        [SerializeField] private Slider _healthBar;

        private Queue<Waypoint> _movementPath;
        
        private int _lastPassedWaypointIndex = 0;

        private void OnEnable()
        {
            Initialize();
        }

        public void Initialize()
        {
            _healthBar.value = _healthBar.maxValue = _health = enemyBase.maxHealth;
            _movementPath = new Queue<Waypoint>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log($"{this.name}.{nameof(OnTriggerEnter2D)}() is called");
            
            if (other.transform.TryGetComponent(out Waypoint waypoint))
                AddWaypointToMovementPath(waypoint);
            
            if (other.transform.TryGetComponent(out ShellBase shell))
                GetDamaged(shell);
        }

        private void GetDamaged(ShellBase shell)
        {
            //Debug.Log($"{this.name}.{nameof(GetDamaged)}() is called");
            // Despawn bullet
            LeanPool.Despawn(shell.gameObject);
            // Decrease health of soldier
            _healthBar.value = _health -= shell.Damage;
            // If necessary kill soldier
            if (_health <= 0)
            {
                Die(true);
            }
        }
        
        private void Die(bool isKilled = false, float delay = 0)
        {
            transform.DOKill(false);
            gameObject.SetActive(false);
            LeanPool.Despawn(this,delay);
            EnemyManager.GetInstance().RemoveEnemy(gameObject);
            
            if (isKilled)
            {
                EventManager.GetInstance().Notify(Events.EnemyKilled);
                EventManager.GetInstance().Notify(Events.GenerateCoin, 
                    new CoinData(this.transform.position, (int)enemyBase.maxHealth / 5));
            }
            // TODO : play animation or tween here;
        }

        private void AddWaypointToMovementPath(Waypoint waypoint)
        {
            if (waypoint.WType == Waypoint.WaypointType.Finish)
            {
                Die();
                UpdateRemainingLife(-1);
                return;
            }
            else if (waypoint.WType == Waypoint.WaypointType.Start)
            {
                _movementPath.Enqueue(waypoint.GetNextWaypoint());
                Move();
            }
            else
            {
                _movementPath.Enqueue(waypoint.GetNextWaypoint());
            }
            
        }

        private void UpdateRemainingLife(int updateAmount)
        {
            EventManager.GetInstance().Notify(Events.UpdateRemainingLife, updateAmount);
        }

        private void Move()
        {
            var target = _movementPath.Peek();
            var distance = Vector3.Distance(transform.position, target.transform.position);
            var duration = distance / enemyBase.movementSpeed;
            
            var diff = target.transform.position - transform.position;
            var angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            // Rotate Soldier View
            _soldierView.transform.DORotateQuaternion(Quaternion.Euler(0, 0, angle), .2f);
            // Move Soldier Container (View + Canvas)
            transform.DOMove(target.transform.position, duration).SetEase(Ease.Linear).
                OnUpdate(() =>
                {
                    var virtualDistanceCovered = ((_lastPassedWaypointIndex + 1) * 1000) - Vector3.Distance(transform.position, target.transform.position);
                    EnemyManager.GetInstance().UpdateEnemy(gameObject, virtualDistanceCovered);
                }).
                OnComplete((() =>
                {
                    _movementPath.Dequeue();
                    _lastPassedWaypointIndex++;
                    
                    if (_movementPath.Peek() != null)
                    {
                        Move();
                    }
                }));
        }
        
    }
}