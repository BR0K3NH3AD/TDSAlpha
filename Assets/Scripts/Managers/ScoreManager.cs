using System;
using UnityEngine;
using TMPro;


namespace TDS.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private int _score = 0;
        [SerializeField] private TextMeshProUGUI _textScore;
        public event Action<int> OnScoreChange;

        public int Score => _score;

        private void Start()
        {
            UpdateScoreText();
        }

        public void AddScore(int amount)
        {
            _score += amount;
            UpdateScoreText();
            OnScoreChange?.Invoke(_score);
        }

        private void UpdateScoreText()
        {
            if (_textScore != null)
            {
                _textScore.text = "Score " + _score.ToString();
            }
        }
    }

}
