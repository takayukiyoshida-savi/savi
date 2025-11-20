using UnityEngine;

namespace TechSample.Core
{
    /// <summary>
    /// ゲーム開始時の初期化処理を行うエントリーポイント
    /// シーンに配置してゲーム全体の初期化を管理する
    /// </summary>
    public class GameBootstrap : MonoBehaviour
    {
        [Header("ゲーム設定")]
        [SerializeField] private bool enableDebugMode = false;
        
        private void Awake()
        {
            // ゲーム開始時の基本設定
            InitializeGame();
        }
        
        private void Start()
        {
            // 他のシステムが初期化された後の処理
            StartGame();
        }
        
        /// <summary>
        /// ゲームの基本設定を初期化
        /// </summary>
        private void InitializeGame()
        {
            // フレームレート設定
            Application.targetFrameRate = 60;
            
            // デバッグモード設定
            if (enableDebugMode)
            {
                Debug.Log("ゲームがデバッグモードで開始されました");
            }
            
            // その他の初期化処理をここに追加
            Debug.Log("GameBootstrap: ゲームシステムを初期化しました");
        }
        
        /// <summary>
        /// ゲーム開始処理
        /// </summary>
        private void StartGame()
        {
            Debug.Log("GameBootstrap: ゲームを開始します");
            
            // ゲーム開始時に必要な処理をここに追加
            // 例：BGM再生、UI表示、プレイヤー生成など
        }
    }
}