using System;
using UnityEngine;

namespace TechSample.Systems.Enemy
{
    /// <summary>
    /// 敵キャラクターの基本的な挙動を制御
    /// プレイヤーの方向へゆっくりと移動する
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        [Header("移動設定")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float maxSpeed = 3f;
        [SerializeField] private float detectionRange = 10f;
        
        [Header("ライフ設定")]
        [SerializeField] private int maxHealth = 1;
        [SerializeField] private float lifeTime = 30f; // 自動消滅時間
        
        [Header("デバッグ")]
        [SerializeField] private bool showDebugInfo = false;
        
        // イベント
        public event Action OnEnemyDestroyed;
        
        private Transform playerTransform;
        private Rigidbody2D rb2d;
        private int currentHealth;
        private float spawnTime;
        
        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
            spawnTime = Time.time;
        }
        
        private void Start()
        {
            // プレイヤーの参照を取得
            FindPlayerReference();
        }
        
        private void Update()
        {
            // 自動消滅チェック
            CheckLifeTime();
            
            // デバッグ情報の表示
            if (showDebugInfo)
            {
                ShowDebugInfo();
            }
        }
        
        private void FixedUpdate()
        {
            // プレイヤーに向かって移動
            MoveTowardsPlayer();
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
                Debug.LogWarning("EnemyController: プレイヤーが見つかりません。Playerタグを設定してください。");
            }
        }
        
        /// <summary>
        /// プレイヤーに向かって移動する処理
        /// </summary>
        private void MoveTowardsPlayer()
        {
            if (playerTransform == null) return;
            
            // プレイヤーとの距離をチェック
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            
            if (distanceToPlayer <= detectionRange)
            {
                // プレイヤーへの方向を計算
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                
                // 移動力を適用
                Vector2 moveForce = directionToPlayer * moveSpeed;
                rb2d.AddForce(moveForce, ForceMode2D.Force);
                
                // 最大速度の制限
                if (rb2d.velocity.magnitude > maxSpeed)
                {
                    rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
                }
            }
            else
            {
                // 範囲外の場合は減速
                rb2d.velocity = Vector2.Lerp(rb2d.velocity, Vector2.zero, Time.fixedDeltaTime * 2f);
            }
        }
        
        /// <summary>
        /// ライフタイムをチェックして自動消滅
        /// </summary>
        private void CheckLifeTime()
        {
            if (Time.time - spawnTime >= lifeTime)
            {
                DestroyEnemy();
            }
        }
        
        /// <summary>
        /// ダメージを受ける処理
        /// </summary>
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            
            if (showDebugInfo)
            {
                Debug.Log($"Enemy: ダメージを受けました。残りHP: {currentHealth}");
            }
            
            if (currentHealth <= 0)
            {
                DestroyEnemy();
            }
        }
        
        /// <summary>
        /// 敵を破壊する
        /// </summary>
        private void DestroyEnemy()
        {
            // イベント通知
            OnEnemyDestroyed?.Invoke();
            
            if (showDebugInfo)
            {
                Debug.Log("Enemy: 敵が破壊されました");
            }
            
            // オブジェクトを破壊
            Destroy(gameObject);
        }
        
        /// <summary>
        /// デバッグ情報の表示
        /// </summary>
        private void ShowDebugInfo()
        {
            if (playerTransform != null)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                Debug.Log($"Enemy: プレイヤーまでの距離: {distance:F2}, HP: {currentHealth}");
            }
        }
        
        /// <summary>
        /// プレイヤーとの衝突処理
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // プレイヤーとの衝突時の処理をここに追加
                // 例：プレイヤーにダメージを与える、自分を破壊するなど
                Debug.Log("Enemy: プレイヤーと接触しました");
                DestroyEnemy();
            }
        }
        
        /// <summary>
        /// 検出範囲を可視化（エディタ用）
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}