using System.Collections.Generic;
using Coin;
using Common;
using DG.Tweening;
using Lean.Pool;
using Shells;
using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public class Soldier : Enemy
    {
        #region Unity Events

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.TryGetComponent(out Waypoint waypoint))
                AddWaypointToMovementPath(waypoint);
            
            if (other.transform.TryGetComponent(out ShellBase shell))
                GetDamaged(shell);
        }
        
        #endregion

        #region Soldier Methods

        protected override void GetDamaged(ShellBase shell)
        {
            if (_isDied)
                return;
                
            // Despawn bullet
            LeanPool.Despawn(shell.gameObject);   
            
            // Decrease health of soldier
            _healthBar.value = _health -= shell.damage;
            
            // If necessary kill soldier
            if (_health <= 0)
            {
                _isDied = true;
                Die(true);
            }
        }
        
        protected override void Die(bool isKilled = false, float delay = 0)
        {
            transform.DOKill(false);
            gameObject.SetActive(false);
            LeanPool.Despawn(this,delay);
            EnemyManager.GetInstance().RemoveEnemy(gameObject);
            
            if (isKilled)
            {
                EventManager.GetInstance().Notify(Events.EnemyKilled);
                EventManager.GetInstance().Notify(Events.GenerateCoin, new CoinData(this.transform.position, enemyBase.Prize));
            }
            
            // TODO : play animation or tween here;
        }

        protected override void AddWaypointToMovementPath(Waypoint waypoint)
        {
            if (waypoint.WType == Waypoint.WaypointType.Finish)
            {
                UpdateRemainingLife(-1);
                Die();
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

        protected override void Move()
        {
            var target = _movementPath.Peek();
            var distance = Vector3.Distance(transform.position, target.transform.position);
            var duration = distance / enemyBase.MovementSpeed;
            
            var diff = target.transform.position - transform.position;
            var angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            // Rotate Soldier View
            _enemyView.transform.DORotateQuaternion(Quaternion.Euler(0, 0, angle), .2f);
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
        
        #endregion
        
    }
}