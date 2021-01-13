using System;
using UnityEngine;
using Utils;

namespace DefaultNamespace
{
    public class Timer : MonoBehaviour, IEventManagerHandling
    {
        [SerializeField] [Range(.1f, 1)] private float tickDuration;
        
        private float _currentGameTime;
        private float _tmpTime;

        public float CurrentGameTime { get; }

        private bool isPaused = false;

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
            if(isPaused)
                return;
                
            _tmpTime += Time.deltaTime;
            
            if (_tmpTime > tickDuration)
            {
                _tmpTime -= tickDuration;
                _currentGameTime += tickDuration;
                EventManager.GetInstance().Notify(Events.TimeTickUpdated, _currentGameTime, false);
            }
        }

        public void Pause()
        {
            isPaused = true;
        }
        
        public void Continue()
        {
            isPaused = false;
        }

        public void Play()
        {
            isPaused = false;
            _currentGameTime = 0;
        }

        private void OnStartGame(object obj)
        {
            Play();
        }
        
        public void SubscribeEvents()
        {
            EventManager.GetInstance().Subscribe(Events.StartGame, OnStartGame);
        }

        public void UnsubscribeEvents()
        {
            EventManager.GetInstance().Unsubscribe(Events.StartGame, OnStartGame);
        }
    }
}