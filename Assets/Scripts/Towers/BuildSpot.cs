using Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Towers
{
    public class BuildSpot : MonoBehaviour, IEventManagerHandling, IPointerClickHandler
    {
        private bool _isBuildMenuOpened;
        private bool _isMaintenanceMenuOpened;
        private bool _isUnitBuilded;

        #region Unity Events

        private void OnEnable()
        {
            Init();
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void ButtonClicked()
        {
            if (!_isBuildMenuOpened)
            {
                _isBuildMenuOpened = true;
                EventManager.GetInstance().Notify(Events.OpenBuildSpotMenu, this);
            }
        }
    
        #endregion

        #region BuildSpot Methods

        public void Init()
        {
            _isBuildMenuOpened = false;
            _isUnitBuilded = false;
        }

        #endregion

        #region BuildSpot Event Callbacks

        private void OnCloseBuildSpotMenu(object _)
        {
            _isBuildMenuOpened = false;
        }

        #endregion

        #region IEventManagerHandling Methods

        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.ClickBlockerClicked, OnCloseBuildSpotMenu);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.ClickBlockerClicked, OnCloseBuildSpotMenu);
        }

        #endregion

        public void OnPointerClick(PointerEventData eventData)
        {
            ButtonClicked();
            Debug.Log($"{nameof(BuildSpot)}.{nameof(OnPointerClick)}() is called");
        }
    }
}
