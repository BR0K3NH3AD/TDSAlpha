using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TDS.Scripts.LastMegaBoss;
using TDS.Scripts.Managers;
using TMPro;

namespace TDS.Scripts.UI
{
    public class GameWinnerUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameWinnerUI;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private ScoreManager scoreManager;

        private float gameTime;

        private void Start()
        {
            MegaBoss.OnMegaBossDeath += ShowGameWinnerUI;
            gameTime = 0f;
        }

        private void Update()
        {
            gameTime += Time.deltaTime;
        }

        private void OnDestroy()
        {
            MegaBoss.OnMegaBossDeath -= ShowGameWinnerUI;
        }

        private void ShowGameWinnerUI()
        {
            gameWinnerUI.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;

            timeText.text = "Time " + FormatTime(gameTime);
            scoreText.text = "Score " + scoreManager.Score;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            return string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

}

