using Common;
using Game;
using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ModularMenuBuyPanelItemView : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] private TowerBase _towerBase;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _price;

        private Button _button;
        private BuildSpot _currentBuildSpot;

        #region Unity Events
        
        private void Awake()
        {
            _button = transform.GetComponent<Button>();
        }

        private void OnEnable()
        {
            SubscribeEvents();

            _button.onClick.AddListener(BuyButtonClicked);
            
            SetView();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(BuyButtonClicked);
            UnsubscribeEvents();
        }
        
        #endregion

        #region ModularMenuBuyPanelItemView Event Callbacks

        private void OnMoneyUpdated(object obj)
        {
            CheckEnoughMoney();
        }
        
        private void BuyButtonClicked()
        {
            ServiceLocator.Instance.GetService<TowerGenerator>().GenerateTower(_towerBase,_currentBuildSpot);
            EventManager.GetInstance().Notify(Events.MoneyUpdated, -_towerBase.Price);
            
            ServiceLocator.Instance.GetService<ClickBlocker>().OnPointerClick(null);
        }

        #endregion
        
        #region ModularMenuBuyPanelItemView Methods
        
        private void CheckEnoughMoney()
        {
            _button.interactable = (ServiceLocator.Instance.GetService<GameManager>().Money >= _towerBase.Price);
        }
        
        public void SetView()
        {
            _currentBuildSpot = ServiceLocator.Instance.GetService<ModularMenuController>().CurrentBuildSpot;

            _title.text = _towerBase.Name;
            _price.text = $"${_towerBase.Price}";

            CheckEnoughMoney();
        }
        
        #endregion
        
        #region IEventManagerHandling Methods
        
        public void SubscribeEvents()
        { 
            EventManager.GetInstance().Subscribe(Events.MoneyUpdated, OnMoneyUpdated);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.MoneyUpdated, OnMoneyUpdated);
        }
        
        #endregion
    }
}