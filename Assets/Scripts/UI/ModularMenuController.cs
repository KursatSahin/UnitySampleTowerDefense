using System;
using System.Collections.Generic;
using Common;
using Game;
using Lean.Pool;
using Towers;
using UnityEngine;

namespace UI
{
    public class ModularMenuController : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] private GameObject _buildMenu;
        [SerializeField] private GameObject _maintenanceMenu;

        private float _horizontalPaddingAmount = 150;
        private float _verticalPaddingAmount = 50;

        private BuildSpot _currentBuildSpot;
        private Tower _currentTower;

        public BuildSpot CurrentBuildSpot => _currentBuildSpot;
        public Tower CurrentTower => _currentTower;

        private Dictionary<Tower, BuildSpot> _towerBuildSpotPairs = new Dictionary<Tower, BuildSpot>();

        #region Unity Events

        private void Awake()
        {
            CalculatePaddingAmount();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region ModularMenuController Methods

        public void CloseBothMenu()
        {
            _buildMenu.SetActive(false);
            _maintenanceMenu.SetActive(false);
        }
        
        private void CalculatePaddingAmount()
        {
            _horizontalPaddingAmount = Math.Max(
                _maintenanceMenu.GetComponent<RectTransform>().rect.width / 2, 
                _buildMenu.GetComponent<RectTransform>().rect.width / 2 );
            
            _verticalPaddingAmount = _maintenanceMenu.GetComponent<RectTransform>().rect.width;
        }

        #endregion
        
        #region BuildSpotMenuController Event Callbacks

        private void OnOpenBuildSpotMenu(object data)
        {
            CloseBothMenu();
            
            BuildSpot buildSpot = data as BuildSpot;
            if (buildSpot == null) return; 
            
            var position = buildSpot.transform.position;
            var positionOffsetYAxis = 82f;

            transform.position = position + new Vector3(0, positionOffsetYAxis);

            _currentBuildSpot = buildSpot;

            _buildMenu.SetActive(true);
        }

        private void OnOpenTowerMenu(object data)
        {
            CloseBothMenu();
            
            Tower tower = data as Tower;
            if (tower == null) return;
            
            var position = tower.transform.position;
            var positionOffsetYAxis = 82f;
            
            transform.position = position + new Vector3(0, positionOffsetYAxis);

            _currentTower = tower;
            
            _maintenanceMenu.SetActive(true);
        }

        private void OnClickOutsideOfInteractiveArea(object obj)
        {
            CloseBothMenu();
        }

        #endregion
        
        #region IEventManagerHandling Methods
        
        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.OpenBuildSpotMenu, OnOpenBuildSpotMenu);
            EventManager.GetInstance().Subscribe(Events.OpenTowerMenu, OnOpenTowerMenu);
            EventManager.GetInstance().Subscribe(Events.ClickBlockerClicked, OnClickOutsideOfInteractiveArea);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.OpenBuildSpotMenu, OnOpenBuildSpotMenu);
            EventManager.GetInstance().Unsubscribe(Events.OpenTowerMenu, OnOpenTowerMenu);
            EventManager.GetInstance().Unsubscribe(Events.ClickBlockerClicked, OnClickOutsideOfInteractiveArea);
        }

        #endregion
    }
}