using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VictoryScreenController : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] private GameObject _uiView;
        
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _gameoverMessage;
        [SerializeField] private Button _restartGameButton;

        #region Unity Events
        
        private void Awake()
        {
            _restartGameButton.onClick.AddListener(OnRestartGameButtonClicked);
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            _restartGameButton.onClick.RemoveListener(OnRestartGameButtonClicked);
            UnsubscribeEvents();
        }

        #endregion

        #region Event Manager Handling Methods
        
        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.WonGame, OnWonGame);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.WonGame, OnWonGame);
        }

        #endregion

        #region VictoryScreenController Methods

        private void OnWonGame(object _)
        {
            Show();
        }
        
        private void Show()
        {
            _uiView.SetActive(true);
        }

        private void Hide()
        {
            _uiView.SetActive(false);
        }
        
        private void OnRestartGameButtonClicked()
        {
            EventManager.GetInstance().Notify(Events.RestartGame);
        }
        
        #endregion
    }
}