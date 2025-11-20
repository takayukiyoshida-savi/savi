using UnityEngine;
using MagicSurvivors.Data;

namespace MagicSurvivors.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("Game State")]
        [SerializeField] private CharacterClass selectedCharacter = CharacterClass.FireMage;
        
        private float gameStartTime;
        private float currentGameTime;
        private bool gameActive = false;
        private StageType currentStage = StageType.Forest;
        
        private bool miniBoss1Spawned = false;
        private bool miniBoss2Spawned = false;
        private bool miniBoss3Spawned = false;
        private bool finalBossSpawned = false;
        
        public float CurrentGameTime => currentGameTime;
        public bool IsGameActive => gameActive;
        public StageType CurrentStage => currentStage;
        public CharacterClass SelectedCharacter => selectedCharacter;
        
        public delegate void GameTimeEvent(float time);
        public event GameTimeEvent OnMiniBoss1Time;
        public event GameTimeEvent OnMiniBoss2Time;
        public event GameTimeEvent OnMiniBoss3Time;
        public event GameTimeEvent OnFinalBossTime;
        public event GameTimeEvent OnStageTransition;
        public event System.Action OnGameStart;
        public event System.Action OnGameEnd;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Update()
        {
            if (gameActive)
            {
                UpdateGameTime();
                CheckGameEvents();
            }
        }
        
        public void StartGame(CharacterClass character)
        {
            selectedCharacter = character;
            gameStartTime = Time.time;
            currentGameTime = 0f;
            gameActive = true;
            currentStage = StageType.Forest;
            
            miniBoss1Spawned = false;
            miniBoss2Spawned = false;
            miniBoss3Spawned = false;
            finalBossSpawned = false;
            
            OnGameStart?.Invoke();
            Debug.Log($"GameManager: Game started with {character}");
        }
        
        public void EndGame()
        {
            gameActive = false;
            OnGameEnd?.Invoke();
            Debug.Log("GameManager: Game ended");
        }
        
        public void PauseGame()
        {
            Time.timeScale = 0f;
        }
        
        public void ResumeGame()
        {
            Time.timeScale = 1f;
        }
        
        private void UpdateGameTime()
        {
            currentGameTime = Time.time - gameStartTime;
            
            if (currentGameTime >= GameConstants.GAME_DURATION)
            {
                EndGame();
            }
        }
        
        private void CheckGameEvents()
        {
            if (!miniBoss1Spawned && currentGameTime >= GameConstants.MINIBOSS_1_TIME)
            {
                miniBoss1Spawned = true;
                OnMiniBoss1Time?.Invoke(currentGameTime);
                Debug.Log("GameManager: Mini-Boss 1 spawn time reached");
            }
            
            if (!miniBoss2Spawned && currentGameTime >= GameConstants.MINIBOSS_2_TIME)
            {
                miniBoss2Spawned = true;
                currentStage = StageType.Cave;
                OnStageTransition?.Invoke(currentGameTime);
                OnMiniBoss2Time?.Invoke(currentGameTime);
                Debug.Log("GameManager: Stage transition to Cave and Mini-Boss 2 spawn");
            }
            
            if (!miniBoss3Spawned && currentGameTime >= GameConstants.MINIBOSS_3_TIME)
            {
                miniBoss3Spawned = true;
                OnMiniBoss3Time?.Invoke(currentGameTime);
                Debug.Log("GameManager: Mini-Boss 3 spawn time reached");
            }
            
            if (!finalBossSpawned && currentGameTime >= GameConstants.FINAL_BOSS_TIME)
            {
                finalBossSpawned = true;
                OnFinalBossTime?.Invoke(currentGameTime);
                Debug.Log("GameManager: Final Boss spawn time reached");
            }
        }
        
        public float GetSpawnDensityMultiplier()
        {
            float t = currentGameTime / GameConstants.GAME_DURATION;
            return Mathf.Lerp(1f, 2.5f, t * t);
        }
    }
}
