using UnityEngine;

namespace TechSample.Systems.Player
{
    /// <summary>
    /// 2Dプレイヤーキャラクターの移動制御
    /// WASD / 方向キーでの移動を処理する
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("移動設定")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float maxSpeed = 10f;
        
        [Header("デバッグ")]
        [SerializeField] private bool showDebugInfo = false;
        
        private Rigidbody2D rb2d;
        private Vector2 moveInput;
        private Vector2 currentVelocity;
        
        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            // 入力の取得
            HandleInput();
            
            // デバッグ情報の表示
            if (showDebugInfo)
            {
                ShowDebugInfo();
            }
        }
        
        private void FixedUpdate()
        {
            // 物理演算を使った移動処理
            HandleMovement();
        }
        
        /// <summary>
        /// プレイヤーの入力を処理
        /// </summary>
        private void HandleInput()
        {
            // WASD / 方向キーの入力を取得
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            moveInput = new Vector2(horizontal, vertical).normalized;
        }
        
        /// <summary>
        /// プレイヤーの移動処理
        /// </summary>
        private void HandleMovement()
        {
            // 移動力を計算
            Vector2 moveForce = moveInput * moveSpeed;
            
            // Rigidbody2Dに力を加える
            rb2d.AddForce(moveForce, ForceMode2D.Force);
            
            // 最大速度の制限
            if (rb2d.velocity.magnitude > maxSpeed)
            {
                rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
            }
            
            // 入力がない場合は減速
            if (moveInput.magnitude == 0)
            {
                rb2d.velocity = Vector2.Lerp(rb2d.velocity, Vector2.zero, Time.fixedDeltaTime * 5f);
            }
        }
        
        /// <summary>
        /// デバッグ情報の表示
        /// </summary>
        private void ShowDebugInfo()
        {
            currentVelocity = rb2d.velocity;
            Debug.Log($"Player - Input: {moveInput}, Velocity: {currentVelocity}, Speed: {currentVelocity.magnitude:F2}");
        }
        
        /// <summary>
        /// プレイヤーの現在位置を取得
        /// </summary>
        public Vector3 GetPosition()
        {
            return transform.position;
        }
        
        /// <summary>
        /// プレイヤーの現在速度を取得
        /// </summary>
        public Vector2 GetVelocity()
        {
            return rb2d.velocity;
        }
    }
}