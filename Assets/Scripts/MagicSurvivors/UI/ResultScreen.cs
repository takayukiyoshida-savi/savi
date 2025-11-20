using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using MagicSurvivors.Core;
using MagicSurvivors.MetaProgression;
using MagicSurvivors.Data;

namespace MagicSurvivors.UI
{
    public class ResultScreen : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TextMeshProUGUI resultTitleText;
        [SerializeField] private TextMeshProUGUI elapsedTimeText;
        [SerializeField] private TextMeshProUGUI enemiesDefeatedText;
        [SerializeField] private TextMeshProUGUI skillsAcquiredText;
        [SerializeField] private TextMeshProUGUI synergiesTriggeredText;
        [SerializeField] private TextMeshProUGUI goldEarnedText;
        [SerializeField] private TextMeshProUGUI magicStonesEarnedText;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button titleButton;
        
        private GameStats gameStats;
        
        private void Start()
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }
            
            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryClicked);
            }
            
            if (titleButton != null)
            {
                titleButton.onClick.AddListener(OnTitleClicked);
            }
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameEnd += ShowResults;
            }
        }
        
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameEnd -= ShowResults;
            }
        }
        
        public void ShowResults()
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
            }
            
            Time.timeScale = 0f;
            
            gameStats = GameStatsTracker.Instance?.GetStats();
            if (gameStats != null)
            {
                DisplayStats();
                CalculateAndAwardRewards();
            }
        }
        
        private void DisplayStats()
        {
            if (resultTitleText != null)
            {
                resultTitleText.text = gameStats.isVictory ? "VICTORY!" : "DEFEATED";
            }
            
            if (elapsedTimeText != null)
            {
                int minutes = Mathf.FloorToInt(gameStats.elapsedTime / 60f);
                int seconds = Mathf.FloorToInt(gameStats.elapsedTime % 60f);
                elapsedTimeText.text = $"Time: {minutes:00}:{seconds:00}";
            }
            
            if (enemiesDefeatedText != null)
            {
                enemiesDefeatedText.text = $"Enemies Defeated: {gameStats.enemiesDefeated}";
            }
            
            if (skillsAcquiredText != null)
            {
                skillsAcquiredText.text = $"Skills Acquired: {gameStats.skillsAcquired}";
            }
            
            if (synergiesTriggeredText != null)
            {
                synergiesTriggeredText.text = $"Synergies Triggered: {gameStats.synergiesTriggered}";
            }
        }
        
        private void CalculateAndAwardRewards()
        {
            int goldEarned = CalculateGoldReward();
            int magicStonesEarned = CalculateMagicStoneReward();
            
            if (goldEarnedText != null)
            {
                goldEarnedText.text = $"Gold: +{goldEarned}";
            }
            
            if (magicStonesEarnedText != null)
            {
                magicStonesEarnedText.text = $"Magic Stones: +{magicStonesEarned}";
            }
            
            if (MetaProgressionManager.Instance != null)
            {
                MetaProgressionManager.Instance.AddGold(goldEarned);
            }
        }
        
        private int CalculateGoldReward()
        {
            int baseGold = gameStats.enemiesDefeated * 2;
            
            if (gameStats.miniBossesDefeated > 0)
            {
                baseGold += gameStats.miniBossesDefeated * 50;
            }
            
            if (gameStats.isVictory)
            {
                baseGold += 200;
            }
            
            return baseGold;
        }
        
        private int CalculateMagicStoneReward()
        {
            int magicStones = 0;
            
            magicStones += gameStats.miniBossesDefeated * 1;
            
            if (gameStats.isVictory)
            {
                magicStones += 5;
            }
            
            return magicStones;
        }
        
        private void OnRetryClicked()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private void OnTitleClicked()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("TitleScene");
        }
    }
    
    [System.Serializable]
    public class GameStats
    {
        public bool isVictory;
        public float elapsedTime;
        public int enemiesDefeated;
        public int skillsAcquired;
        public int synergiesTriggered;
        public int miniBossesDefeated;
    }
    
    public class GameStatsTracker : MonoBehaviour
    {
        public static GameStatsTracker Instance { get; private set; }
        
        private GameStats stats = new GameStats();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void RecordEnemyDefeated()
        {
            stats.enemiesDefeated++;
        }
        
        public void RecordSkillAcquired()
        {
            stats.skillsAcquired++;
        }
        
        public void RecordSynergyTriggered()
        {
            stats.synergiesTriggered++;
        }
        
        public void RecordMiniBossDefeated()
        {
            stats.miniBossesDefeated++;
        }
        
        public void SetVictory(bool victory)
        {
            stats.isVictory = victory;
        }
        
        public void SetElapsedTime(float time)
        {
            stats.elapsedTime = time;
        }
        
        public GameStats GetStats()
        {
            return stats;
        }
    }
}
