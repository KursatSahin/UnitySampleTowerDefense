using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class VictoryScreenController : MonoBehaviour, IEventManagerHandling
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