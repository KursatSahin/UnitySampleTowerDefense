using Common;
using DG.Tweening;
using Lean.Pool;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] private GameValues _gameValues; 
        
        [SerializeField] private GameObject _splashScreen;

        private int _killedEnemy = 0;
        private int _lives = 0;
        private int _money = 0;
        
        private int _magicNumber = 0;

        public int Money
        {
            get => _money;
        }
        
        #region Unity Events

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void Start()
        {
            Init();

            _splashScreen.GetComponent<CanvasGroup>().DOFade(0,.5f).SetDelay(1.5f).
                OnComplete((() =>
                {
                    _splashScreen.SetActive(false);
                    EventManager.GetInstance().Notify(Events.StartGame);
                }));
        }

        #endregion

        #region GameManager Methods
        
        private void Init()
        {
            _lives = _gameValues.InitialLives;
            _money = _gameValues.InitialMoney;
            
            _killedEnemy = 0;
            _magicNumber = ServiceLocator.Instance.GetService<WaveGenerator>().TotalUnitCount();

            ServiceLocator.Instance.GetService<HudScreenController>().MoneyAmount = _money;
            ServiceLocator.Instance.GetService<HudScreenController>().RemainingLives = _lives;
            ServiceLocator.Instance.GetService<HudScreenController>().KilledEnemyAmount = _killedEnemy;
        }
        
        #endregion

        #region GameManager Event Callbacks

        private void OnEnemyKilled(object _)
        {
            ServiceLocator.Instance.GetService<HudScreenController>().KilledEnemyAmount = ++_killedEnemy;

            CheckGameIsEnd();
        }

        private void CheckGameIsEnd()
        {
            if (_lives <= 0)
            {
                Time.timeScale = 0;
                EventManager.GetInstance().Notify(Events.GameIsOver,null,true);
            }
            
            if (_killedEnemy + (_gameValues.InitialLives - _lives) >= _magicNumber)
            {
                Time.timeScale = 0;
                EventManager.GetInstance().Notify(Events.WonGame,null,true);
            }
        }

        private void OnUpdateRemainingLife(object data)
        {
            ServiceLocator.Instance.GetService<HudScreenController>().RemainingLives = _lives += (int) data;

            CheckGameIsEnd();
        }

        private void OnUpdateMoney(object data)
        {
            _money += (int) data;
            DOTween.To(()=> ServiceLocator.Instance.GetService<HudScreenController>().MoneyAmount, x=> ServiceLocator.Instance.GetService<HudScreenController>().MoneyAmount = x, _money, .5f);
        }

        private void OnRestartGame(object data)
        {
            DOTween.timeScale = 0;
            DOTween.KillAll(false);
            LeanPool.DespawnAll();
            EventManager.GetInstance().Destroy();
            EnemyManager.GetInstance().Destroy();
            SceneManager.LoadSceneAsync("EntryScene");
        }

        #endregion
        
        #region IEventManagerHandling Methods
        
        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.ChangeMoneyAmout, OnUpdateMoney);
            EventManager.GetInstance().Subscribe(Events.ChangeRemainingLifeAmount, OnUpdateRemainingLife);
            EventManager.GetInstance().Subscribe(Events.EnemyKilled, OnEnemyKilled);
            EventManager.GetInstance().Subscribe(Events.RestartGame, OnRestartGame);
        }
        
        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.ChangeMoneyAmout, OnUpdateMoney);
            EventManager.GetInstance().Unsubscribe(Events.ChangeRemainingLifeAmount, OnUpdateRemainingLife);
            EventManager.GetInstance().Unsubscribe(Events.EnemyKilled, OnEnemyKilled);
            EventManager.GetInstance().Unsubscribe(Events.RestartGame, OnRestartGame);
        }
        
        #endregion
    }
}