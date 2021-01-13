using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject uiView;
        
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI gameTitle;
        [SerializeField] private Button startGameButton;

        #region Unity Events
        
        private void Awake()
        {
            startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            
            DOTween.timeScale = 1;
            Time.timeScale = 1;
            
            DOTween.KillAll(false);
        }

        private void OnDestroy()
        {
            startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
        }

        #endregion

        #region MainMenuController Methods

        private void OnStartGameButtonClicked()
        {
            uiView.SetActive(false);
            SceneManager.LoadScene("GameScene");
        }
        
        #endregion
    }
}