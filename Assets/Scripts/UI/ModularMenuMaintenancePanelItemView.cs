using System;
using Common;
using DefaultNamespace;
using Game;
using TMPro;
using Towers;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ModularMenuMaintenancePanelItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private MaintenanceType _maintenanceType = MaintenanceType.Repair;

        private Button _button;
        private Tower _currentTower;
        private int _eventPrice;

        #region Unity Events

        private void Awake()
        {
            _button = transform.GetComponent<Button>();
        }

        private void OnEnable()
        {
            SetView();
            
            _button.onClick.AddListener(ButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ButtonClicked);
        }
        
        #endregion

        
        
        private void ButtonClicked()
        {
            switch (_maintenanceType)
            {
                case MaintenanceType.Sell:
                    Sell(); break;
                case MaintenanceType.Repair:
                    Repair(); break;
                case MaintenanceType.Upgrade:
                    Upgrade(); break;
            }
            
            ServiceLocator.Instance.GetService<ClickBlocker>().OnPointerClick(null);
        }

        private void Sell()
        {
            ServiceLocator.Instance.GetService<TowerGenerator>().DestroyTower(_currentTower);
            EventManager.GetInstance().Notify(Events.ChangeMoneyAmout, -_eventPrice);
        }

        [Obsolete]
        private void Repair()
        {
            // TODO: will be added soon.
            EventManager.GetInstance().Notify(Events.ChangeMoneyAmout, -_eventPrice);
        }

        [Obsolete]
        private void Upgrade()
        {
            // TODO: will be added soon.
            // _currentTower.UpgradeTower();
            EventManager.GetInstance().Notify(Events.ChangeMoneyAmout, -_eventPrice);
        }
        
        public void SetView()
        {
            _currentTower = ServiceLocator.Instance.GetService<ModularMenuController>().CurrentTower;

            switch (_maintenanceType)
            {
                case MaintenanceType.Sell:
                    _eventPrice = -Utils.GetSellPrice(_currentTower); break;
                case MaintenanceType.Repair:
                    _eventPrice = Utils.GetRepairPrice(_currentTower); break;
                case MaintenanceType.Upgrade:
                    _eventPrice = Utils.GetUpgradePrice(_currentTower); break;
            }
            
            _title.text = $"{nameof(_maintenanceType)}";
            _price.text = $"${-_eventPrice}";

            CheckEnoughMoney();
        }
        
        private void OnMoneyUpdated(object obj)
        {
            CheckEnoughMoney();
        }
        
        private void CheckEnoughMoney()
        {
            if (_maintenanceType == MaintenanceType.Repair)
            {
                _button.interactable = (ServiceLocator.Instance.GetService<GameManager>().Money >= _eventPrice) 
                    && _currentTower.Health < _currentTower.TowerBase.MaxHealth;
            }
            else
            { 
                _button.interactable = (ServiceLocator.Instance.GetService<GameManager>().Money >= _eventPrice);
            }
        }
        
        public void SubscribeEvents()
        { 
            EventManager.GetInstance().Subscribe(Events.MoneyUpdated, OnMoneyUpdated);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.MoneyUpdated, OnMoneyUpdated);
        }
    }

    public enum MaintenanceType
    {
        Repair,
        Sell,
        Upgrade
    }
}