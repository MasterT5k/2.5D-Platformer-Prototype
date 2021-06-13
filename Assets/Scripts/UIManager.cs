using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KWP
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _coinText = null, _livesText = null;

        public void UpdateCoins(int amount)
        {
            if (_coinText != null)
            {
                _coinText.text = "Coin: " + amount;
            }
        }

        public void UpdateLives(int amount)
        {
            if (_livesText != null)
            {
                _livesText.text = "Lives: " + amount;
            }
        }
    } 
}
