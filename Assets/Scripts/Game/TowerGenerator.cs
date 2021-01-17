using System.Collections.Generic;
using Common;
using Lean.Pool;
using Towers;
using UnityEngine;

namespace Game
{
    public class TowerGenerator : MonoBehaviour
    {
        private Dictionary<Tower, BuildSpot> _towerBuildSpotPairs;

        private void Awake()
        {
            _towerBuildSpotPairs = new Dictionary<Tower, BuildSpot>();
        }

        private void OnDestroy()
        {
            if (_towerBuildSpotPairs != null)
            { 
                _towerBuildSpotPairs.Clear();
            }
        }

        public void GenerateTower(TowerBase towerBase, BuildSpot buildSpot)
        {
            if (towerBase == null)
            {
                Debug.LogError($"Missing {nameof(towerBase)} parameter");
                return;
            }
            
            if (buildSpot == null)
            {
                Debug.LogError($"Missing {nameof(buildSpot)} parameter");
                return;
            }
            
            var tower = LeanPool.Spawn(towerBase.Prefab,buildSpot.transform.position,Quaternion.identity,null).GetComponent<Tower>();
            
            _towerBuildSpotPairs.Add(tower, buildSpot);
            
            buildSpot.gameObject.SetActive(false);
            
            EventManager.GetInstance().Notify(Events.ChangeMoneyAmout, -towerBase.Price);
        }

        public void DestroyTower(Tower tower)
        {
            if (tower == null)
            {
                Debug.LogError($"Missing {nameof(tower)} parameter");
                return;
            }

            if (!_towerBuildSpotPairs.ContainsKey(tower))
            {
                Debug.LogError("The tower to be destroyed is missing");
                return;
            }
            
            var buildSpot = _towerBuildSpotPairs[tower];
            buildSpot.gameObject.SetActive(true);
            
            _towerBuildSpotPairs.Remove(tower);
            LeanPool.Despawn(tower.gameObject);
        }
    }
}