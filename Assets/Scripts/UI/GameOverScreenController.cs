using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverScreenController : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] private GameObject uiView;
        
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI gameoverMessage;
        [SerializeField] private Button restartGameButton;

        #region Unity Events
        
        private void Awake()
        {
            restartGameButton.onClick.AddListener(OnRestartGameButtonClicked);
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            restartGameButton.onClick.RemoveListener(OnRestartGameButtonClicked);
            UnsubscribeEvents();
        }

        #endregion

        #region Event Manager Handling Methods
        
        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.GameIsOver, OnGameIsOver);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.GameIsOver, OnGameIsOver);
        }
        
        #endregion

        #region GameOverScreenController Methods

        private void OnGameIsOver(object obj)
        {
            Show();
        }
        
        private void Show()
        {
            uiView.SetActive(true);
        }

        private void Hide()
        {
            uiView.SetActive(false);
        }
        
        private void OnRestartGameButtonClicked()
        {
            EventManager.GetInstance().Notify(Events.RestartGame);
        }
        
        #endregion
    }
}