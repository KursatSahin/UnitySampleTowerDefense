using System;
using Common;
using UnityEngine;

namespace DefaultNamespace
{
    public class Timer : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] [Range(.1f, 1)] private float tickDuration;
        
        private float _currentGameTime;
        private float _tmpTime;

        public float CurrentGameTime { get=>_currentGameTime; }

        private bool _isPaused = false;

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
            _currentGameTime = 0;
            _tmpTime = 0;
            
            Pause();
        }

        private void Update()
        {
            if(_isPaused)
                return;
                
            _tmpTime += Time.deltaTime;
            
            if (_tmpTime > tickDuration)
            {
                _tmpTime -= tickDuration;
                _currentGameTime += tickDuration;
                EventManager.GetInstance().Notify(Events.TimeTickUpdated, _currentGameTime, false);
            }
        }        

        #endregion

        #region Timer Methods
        
        public void Pause()
        {
            _isPaused = true;
        }
        
        public void Continue()
        {
            _isPaused = false;
        }

        public void Play()
        {
            _isPaused = false;
            _currentGameTime = 0;
        }
        
        #endregion

        #region Timer Event Callbacks

        private void OnStartGame(object obj)
        {
            Play();
        }

        #endregion
        
        #region IEventManagerHandling Methods

        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.StartGame, OnStartGame);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.StartGame, OnStartGame);
        }
        
        #endregion
    }
}