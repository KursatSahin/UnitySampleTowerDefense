using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _uiView;
        
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _gameTitle;
        [SerializeField] private Button _startGameButton;

        #region Unity Events
        
        private void Awake()
        {
            _startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            
            DOTween.timeScale = 1;
            Time.timeScale = 1;
            
            DOTween.KillAll(false);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
        }

        #endregion

        #region MainMenuController Methods

        private void OnStartGameButtonClicked()
        {
            _uiView.SetActive(false);
            SceneManager.LoadScene("GameScene");
        }
        
        #endregion
    }
}