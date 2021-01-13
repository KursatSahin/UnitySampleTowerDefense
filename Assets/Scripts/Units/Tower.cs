using System.Collections.Generic;
using Lean.Pool;
using Units;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerBase _towerBase;
    
    [SerializeField] private List<GameObject> _enemiesInRange;
    
    [SerializeField] private GameObject bulletFireEffect;

    private float timeToShoot = 0.0f;

    private void Awake()
    {
        // Generate Queue
        _enemiesInRange = new List<GameObject>();
        
        // Set Range
        this.GetComponent<CircleCollider2D>().radius = _towerBase.Range;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy _))
        {
            _enemiesInRange.Add(other.gameObject);    
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy _))
        {
            if (_enemiesInRange.Contains(other.gameObject))
            { 
                _enemiesInRange.Remove(other.gameObject);
            }
        }
    }

    private GameObject GetClosestEnemy()
    {
        GameObject closest = null;
        float minDistance = float.PositiveInfinity;
        
        if (_enemiesInRange.Count < 1)
        {
            return null;
        }
        else
        {
            foreach (var enemy in _enemiesInRange)
            {
                var distance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);
                
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = enemy;
                }
            }
            return closest;
        }
    }

    private GameObject GetOldestEnemy()
    {
        return _enemiesInRange.Count > 0 ? _enemiesInRange[0] : null;
    }

    void FixedUpdate ()
    {
        var enemy = _towerBase.TargetType == ShootingTargetType.First ? GetOldestEnemy() : GetClosestEnemy();

        if (enemy != null)
        {
            var angle = TurnToEnemy(enemy.transform.position);
            
            if (timeToShoot < 0 && Mathf.Abs(angle) < 45)
            {
                var shell = LeanPool.Spawn(_towerBase.ShellPrefab);
                shell.transform.position = transform.position; // + (transform.up * 40) 
                shell.transform.rotation = transform.rotation;

                var shellData = shell.GetComponent<ShellBase>();
                shellData.Speed = _towerBase.BulletSpeed;
                shellData.Range = _towerBase.Range;
                shellData.Direction = transform.transform.up;
                shellData.Damage = _towerBase.Damage;
                shellData.Target = enemy;
                //bulletScript.EnemyTags = _towerBase.enemyTags;
                shellData.Turret = transform;

                shell.SetActive(true);

                Debug.Log("bullet fired");
                
                timeToShoot = _towerBase.FireRate;

                if(bulletFireEffect != null) bulletFireEffect.SetActive(true);
                
                return;
            }
            
        }
        else
        {
            //TurnToEnemy(gameObject);
        }

        if (bulletFireEffect != null && timeToShoot < _towerBase.FireRate * 0.7f && bulletFireEffect.activeSelf)
        {
            bulletFireEffect.SetActive(false);
        }
        
        timeToShoot -= Time.deltaTime;
    }
    
    private float TurnToEnemy(Vector2 position)
    {
        var direction = position - (Vector2)transform.position;
        var angle = Utils.Utils.Angle(direction, transform.up);
        transform.Rotate(0, 0, Mathf.Clamp(angle, -10, 10) * _towerBase.RotationSpeed * Time.deltaTime);
        return angle;   
    }
}