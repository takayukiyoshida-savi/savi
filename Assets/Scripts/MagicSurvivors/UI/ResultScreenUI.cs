using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MagicSurvivors.Core;
using MagicSurvivors.XP;

namespace MagicSurvivors.UI
{
    public class ResultScreenUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private Text timeSurvivedText;
        [SerializeField] private Text enemiesDefeatedText;
        [SerializeField] private Text levelReachedText;
        [SerializeField] private Text goldEarnedText;
        [SerializeField] private Button returnToHomeButton;
        [SerializeField] private Button retryButton;
        
        private int enemiesDefeated = 0;
        private int goldEarned = 0;
        
        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameEnd += ShowResultScreen;
            }
            
            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }
            
            if (returnToHomeButton != null)
            {
                returnToHomeButton.onClick.AddListener(OnReturnToHomeClicked);
            }
            
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryClicked);
            }
        }
        
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameEnd -= ShowResultScreen;
            }
        }
        
        private void ShowResultScreen()
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
                DisplayResults();
            }
        }
        
        private void DisplayResults()
        {
            if (GameManager.Instance != null && timeSurvivedText != null)
            {
                float time = GameManager.Instance.CurrentGameTime;
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                timeSurvivedText.text = $"Time Survived: {minutes:00}:{seconds:00}";
            }
            
            if (enemiesDefeatedText != null)
            {
                enemiesDefeatedText.text = $"Enemies Defeated: {enemiesDefeated}";
            }
            
            XPManager xpManager = FindObjectOfType<XPManager>();
            if (xpManager != null && levelReachedText != null)
            {
                levelReachedText.text = $"Level Reached: {xpManager.CurrentLevel}";
            }
            
            if (goldEarnedText != null)
            {
                goldEarnedText.text = $"Gold Earned: {goldEarned}";
            }
        }
        
        public void AddEnemyDefeated()
        {
            enemiesDefeated++;
        }
        
        public void AddGold(int amount)
        {
            goldEarned += amount;
        }
        
        private void OnReturnToHomeClicked()
        {
            SceneManager.LoadScene("HomeScene");
        }
        
        private void OnRetryClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
