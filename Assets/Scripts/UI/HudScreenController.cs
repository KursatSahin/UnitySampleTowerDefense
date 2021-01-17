using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HudScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject _uiView;
    
        [SerializeField] private TextMeshProUGUI _moneyAmountText;
        [SerializeField] private TextMeshProUGUI _remainingLifeText;
        [SerializeField] private TextMeshProUGUI _killedEnemyAmountText;

        private int _moneyAmount;
        private int _remainingLife;
        private int _killedEnemyAmount;
        
        public int MoneyAmount
        {
            get
            {
                return _moneyAmount;
            }
            set
            {
                _moneyAmount = value;
                _moneyAmountText.text = $"${_moneyAmount}";
            }
        }

        public int RemainingLives
        {
            set
            {
                _remainingLife = value;
                _remainingLifeText.text = $"{_remainingLife}";
            }
        }

        public int KilledEnemyAmount
        {
            set
            {
                _killedEnemyAmount = value;
                _killedEnemyAmountText.text = $"{_killedEnemyAmount}";
            }
        }
        
    }
}
