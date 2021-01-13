using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] private int _initialLives = 3;
        [SerializeField] private int _initialMoney = 300;
        
        [SerializeField] private GameObject _splashScreen;

        private int _killedEnemy = 0;
        private int _lives = 0;
        private int _money = 0;

        private int _magicNumber = 0;
        
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

        private void Init()
        {
            _money =_initialMoney;
            _lives = _initialLives;
            _killedEnemy = 0;
            _magicNumber = ServiceLocator.Instance.GetService<WaveGenerator>().TotalUnitCount();

            ServiceLocator.Instance.GetService<HudScreenController>().MoneyAmount = _money;
            ServiceLocator.Instance.GetService<HudScreenController>().RemainingLives = _lives;
            ServiceLocator.Instance.GetService<HudScreenController>().KilledEnemyAmount = _killedEnemy;
        }

        private void OnEnemyKilled(object _)
        {
            ServiceLocator.Instance.GetService<HudScreenController>().KilledEnemyAmount = ++_killedEnemy;

            if (_killedEnemy + (_initialLives - _lives) >= _magicNumber)
            {
                Time.timeScale = 0;
                DOTween.timeScale = 0;
                EventManager.GetInstance().Notify(Events.WonGame,null,true);
            }
        }

        private void OnUpdateRemainingLife(object data)
        {
            ServiceLocator.Instance.GetService<HudScreenController>().RemainingLives = _lives += (int) data;

            if (_lives <= 0)
            {
                Time.timeScale = 0;
                EventManager.GetInstance().Notify(Events.GameIsOver,null,true);
            }
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
            EventManager.GetInstance().Destroy();
            SceneManager.LoadSceneAsync("EntryScene");
        }

        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.UpdateMoney, OnUpdateMoney);
            EventManager.GetInstance().Subscribe(Events.UpdateRemainingLife, OnUpdateRemainingLife);
            EventManager.GetInstance().Subscribe(Events.EnemyKilled, OnEnemyKilled);
            EventManager.GetInstance().Subscribe(Events.RestartGame, OnRestartGame);
        }
        
        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.UpdateMoney, OnUpdateMoney);
            EventManager.GetInstance().Unsubscribe(Events.UpdateRemainingLife, OnUpdateRemainingLife);
            EventManager.GetInstance().Unsubscribe(Events.EnemyKilled, OnEnemyKilled);
            EventManager.GetInstance().Unsubscribe(Events.RestartGame, OnRestartGame);
        }
    }
}