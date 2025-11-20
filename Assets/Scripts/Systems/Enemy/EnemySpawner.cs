using System.Collections;
using UnityEngine;

namespace TechSample.Systems.Enemy
{
    /// <summary>
    /// 敵オブジェクトを一定間隔で生成するスポナー
    /// 指定された範囲内にランダムに敵を配置する
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("スポーン設定")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private int maxEnemyCount = 10;
        
        [Header("スポーン範囲")]
        [SerializeField] private float spawnRangeX = 8f;
        [SerializeField] private float spawnRangeY = 4f;
        [SerializeField] private float minDistanceFromPlayer = 3f;
        
        [Header("デバッグ")]
        [SerializeField] private bool showSpawnArea = true;
        
        private Transform playerTransform;
        private int currentEnemyCount = 0;
        private Coroutine spawnCoroutine;
        
        private void Start()
        {
            // プレイヤーの参照を取得
            FindPlayerReference();
            
            // スポーン開始
            StartSpawning();
        }
        
        private void OnDestroy()
        {
            // コルーチンの停止
            StopSpawning();
        }
        
        /// <summary>
        /// プレイヤーの参照を取得
        /// </summary>
        private void FindPlayerReference()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("EnemySpawner: プレイヤーが見つかりません。Playerタグを設定してください。");
            }
        }
        
        /// <summary>
        /// スポーン開始
        /// </summary>
        public void StartSpawning()
        {
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnEnemyCoroutine());
            }
        }
        
        /// <summary>
        /// スポーン停止
        /// </summary>
        public void StopSpawning()
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }
        
        /// <summary>
        /// 敵を定期的にスポーンするコルーチン
        /// </summary>
        private IEnumerator SpawnEnemyCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                
                if (currentEnemyCount < maxEnemyCount && enemyPrefab != null)
                {
                    SpawnEnemy();
                }
            }
        }
        
        /// <summary>
        /// 敵を1体スポーンする
        /// </summary>
        private void SpawnEnemy()
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            
            if (spawnPosition != Vector3.zero)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                
                // 敵が破壊された時のコールバックを設定
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.OnEnemyDestroyed += OnEnemyDestroyed;
                }
                
                currentEnemyCount++;
                Debug.Log($"EnemySpawner: 敵をスポーンしました。現在の敵数: {currentEnemyCount}");
            }
        }
        
        /// <summary>
        /// 有効なスポーン位置を取得
        /// </summary>
        private Vector3 GetValidSpawnPosition()
        {
            int maxAttempts = 10;
            
            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(-spawnRangeX, spawnRangeX),
                    Random.Range(-spawnRangeY, spawnRangeY),
                    0f
                );
                
                // プレイヤーから十分離れているかチェック
                if (playerTransform == null || 
                    Vector3.Distance(randomPosition, playerTransform.position) >= minDistanceFromPlayer)
                {
                    return randomPosition;
                }
            }
            
            Debug.LogWarning("EnemySpawner: 有効なスポーン位置が見つかりませんでした。");
            return Vector3.zero;
        }
        
        /// <summary>
        /// 敵が破壊された時の処理
        /// </summary>
        private void OnEnemyDestroyed()
        {
            currentEnemyCount--;
            Debug.Log($"EnemySpawner: 敵が破壊されました。現在の敵数: {currentEnemyCount}");
        }
        
        /// <summary>
        /// スポーン範囲を可視化（エディタ用）
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (showSpawnArea)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, new Vector3(spawnRangeX * 2, spawnRangeY * 2, 0));
                
                if (playerTransform != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(playerTransform.position, minDistanceFromPlayer);
                }
            }
        }
    }
}