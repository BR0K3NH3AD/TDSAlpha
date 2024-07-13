using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace TDS.Scripts.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverUI;

        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;

        [SerializeField] private TextMeshProUGUI gameOverText; 
        [SerializeField] private TextMeshProUGUI restartButtonText; 
        [SerializeField] private TextMeshProUGUI quitButtonText; 

        private void Start()
        {
            gameOverUI.SetActive(false);

            restartButton.onClick.AddListener(RestartGame);
            quitButton.onClick.AddListener(QuitGame);

            gameOverText.text = "YOU'RE DEAD";
            restartButtonText.text = "RESTART";
            quitButtonText.text = "QUIT";
        }
        public void ShowGameOverUI()
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }


}

