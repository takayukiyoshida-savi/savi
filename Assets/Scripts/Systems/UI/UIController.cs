using UnityEngine;
using UnityEngine.UI;

namespace TechSample.Systems.UI
{
    /// <summary>
    /// ゲーム中のUI表示を管理
    /// 経過時間や倒した敵数などの情報を画面に表示する
    /// </summary>
    public class UIController : MonoBehaviour
    {
        [Header("UI要素")]
        [SerializeField] private Text timeText;
        [SerializeField] private Text enemyCountText;
        [SerializeField] private Text scoreText;
        
        [Header("表示設定")]
        [SerializeField] private string timeFormat = "Time: {0:F1}s";
        [SerializeField] private string enemyCountFormat = "Enemies Defeated: {0}";
        [SerializeField] private string scoreFormat = "Score: {0}";
        
        [Header("スコア設定")]
        [SerializeField] private int pointsPerEnemy = 100;
        [SerializeField] private int timeBonus = 1; // 1秒あたりのボーナス
        
        // ゲーム状態
        private float gameStartTime;
        private int enemiesDefeated = 0;
        private int currentScore = 0;
        private bool gameActive = true;
        
        private void Start()
        {
            // ゲーム開始時間を記録
            gameStartTime = Time.time;
            
            // UI要素の初期化
            InitializeUI();
            
            // 敵破壊イベントの購読
            SubscribeToEnemyEvents();
        }
        
        private void Update()
        {
            if (gameActive)
            {
                // UI情報を更新
                UpdateUI();
            }
        }
        
        private void OnDestroy()
        {
            // イベントの購読解除
            UnsubscribeFromEnemyEvents();
        }
        
        /// <summary>
        /// UI要素の初期化
        /// </summary>
        private void InitializeUI()
        {
            // テキスト要素が設定されていない場合の警告
            if (timeText == null)
                Debug.LogWarning("UIController: Time Textが設定されていません");
            
            if (enemyCountText == null)
                Debug.LogWarning("UIController: Enemy Count Textが設定されていません");
            
            if (scoreText == null)
                Debug.LogWarning("UIController: Score Textが設定されていません");
            
            // 初期値の設定
            UpdateTimeDisplay(0f);
            UpdateEnemyCountDisplay(0);
            UpdateScoreDisplay(0);
        }
        
        /// <summary>
        /// UI情報を更新
        /// </summary>
        private void UpdateUI()
        {
            // 経過時間の更新
            float elapsedTime = Time.time - gameStartTime;
            UpdateTimeDisplay(elapsedTime);
            
            // スコアの更新（時間ボーナス含む）
            int timeScore = Mathf.FloorToInt(elapsedTime) * timeBonus;
            currentScore = (enemiesDefeated * pointsPerEnemy) + timeScore;
            UpdateScoreDisplay(currentScore);
        }
        
        /// <summary>
        /// 時間表示を更新
        /// </summary>
        private void UpdateTimeDisplay(float time)
        {
            if (timeText != null)
            {
                timeText.text = string.Format(timeFormat, time);
            }
        }
        
        /// <summary>
        /// 敵撃破数表示を更新
        /// </summary>
        private void UpdateEnemyCountDisplay(int count)
        {
            if (enemyCountText != null)
            {
                enemyCountText.text = string.Format(enemyCountFormat, count);
            }
        }
        
        /// <summary>
        /// スコア表示を更新
        /// </summary>
        private void UpdateScoreDisplay(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = string.Format(scoreFormat, score);
            }
        }
        
        /// <summary>
        /// 敵破壊イベントの購読
        /// </summary>
        private void SubscribeToEnemyEvents()
        {
            // 全ての敵コントローラーのイベントを購読
            // 実際の実装では、ゲームマネージャーを通じてイベントを管理することを推奨
            Enemy.EnemyController[] enemies = FindObjectsOfType<Enemy.EnemyController>();
            foreach (var enemy in enemies)
            {
                enemy.OnEnemyDestroyed += OnEnemyDefeated;
            }
        }
        
        /// <summary>
        /// 敵破壊イベントの購読解除
        /// </summary>
        private void UnsubscribeFromEnemyEvents()
        {
            Enemy.EnemyController[] enemies = FindObjectsOfType<Enemy.EnemyController>();
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.OnEnemyDestroyed -= OnEnemyDefeated;
                }
            }
        }
        
        /// <summary>
        /// 敵が倒された時の処理
        /// </summary>
        private void OnEnemyDefeated()
        {
            enemiesDefeated++;
            UpdateEnemyCountDisplay(enemiesDefeated);
            
            Debug.Log($"UIController: 敵を倒しました！ 合計: {enemiesDefeated}体");
        }
        
        /// <summary>
        /// ゲームを停止
        /// </summary>
        public void StopGame()
        {
            gameActive = false;
            Debug.Log($"UIController: ゲーム終了 - 最終スコア: {currentScore}");
        }
        
        /// <summary>
        /// ゲームを再開
        /// </summary>
        public void ResumeGame()
        {
            gameActive = true;
            gameStartTime = Time.time; // 時間をリセット
            Debug.Log("UIController: ゲームを再開しました");
        }
        
        /// <summary>
        /// 現在のスコアを取得
        /// </summary>
        public int GetCurrentScore()
        {
            return currentScore;
        }
        
        /// <summary>
        /// 倒した敵の数を取得
        /// </summary>
        public int GetEnemiesDefeated()
        {
            return enemiesDefeated;
        }
        
        /// <summary>
        /// 経過時間を取得
        /// </summary>
        public float GetElapsedTime()
        {
            return Time.time - gameStartTime;
        }
    }
}