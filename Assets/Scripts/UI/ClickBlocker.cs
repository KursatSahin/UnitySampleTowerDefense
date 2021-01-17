using Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ClickBlocker : MonoBehaviour, IEventManagerHandling, IPointerClickHandler
    {

        #region Unity Events
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        #endregion
        
        public void OnPointerClick(PointerEventData eventData)
        {
            EventManager.GetInstance().Notify(Events.ClickBlockerClicked,null,true);
            DisableBlocker();
        }


        #region ClickBlocker Methods

        private void EnableBlocker(object _)
        {
            GetComponent<Collider2D>().enabled = true;
        }
    
        public void DisableBlocker()
        {
            GetComponent<Collider2D>().enabled = false;
        }

        #endregion
        
        #region IEventManagerHandling Methods
        
        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.OpenTowerMenu,EnableBlocker);
            EventManager.GetInstance().Subscribe(Events.OpenBuildSpotMenu,EnableBlocker);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.OpenTowerMenu,EnableBlocker);
            EventManager.GetInstance().Unsubscribe(Events.OpenBuildSpotMenu,EnableBlocker);
        }
        
        #endregion
    }
}
